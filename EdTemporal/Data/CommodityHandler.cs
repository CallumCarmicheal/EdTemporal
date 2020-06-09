using EdTemporal.Data.DatabaseModels;
using EdTemporal.Data.EDDN.V6;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using PetaPoco;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace EdTemporal.Data {
    public class CommodityHandler {
        ILogger Logger { get; } = ApplicationLogging.CreateLogger<CommodityHandler>();

        public CommodityHandler() {
            // ...
        }

        public List<JsonCommodity> GetEddbCommodities() {
            using (Logger.BeginScope($"=>{ nameof(GetEddbCommodities) }")) {

                Logger.LogInformation("Downloading commodities...");

                WebClient wc = new WebClient();
                string jsonCommodities = wc.DownloadString("https://eddb.io/archive/v6/commodities.json");
                var result = JsonConvert.DeserializeObject<List<JsonCommodity>>(jsonCommodities);
                return result;
            }
        }

        public void UpdateAllCommodities() {
            using (Logger.BeginScope($"=>{ nameof(UpdateAllCommodities) }")) {

                // Connect to database
                Logger.LogInformation("Connecting to database...");
                var dbCon = new DBConnection();
                var db = dbCon.Get();

                Logger.LogInformation("Loading listing data...");
                var eddn = new EddnInformation();

                // Download the commodities into a string
                var result = GetEddbCommodities();

                // Update the categories
                Logger.LogInformation("Updating categories...");
                List<JsonCommodityCategory> uniqueCategories = result.DistinctBy(x => x.CategoryId).Select(x => x.Category).ToList();
                updateCategories(db, uniqueCategories);


                // Check if we need to update commodities
                DateTime checkTime = eddn.PricesUpdatedAt; // Remove minute and second
                checkTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, 0, 0, 0);          
                
                if (commoditiesRequireUpdate(db, checkTime)) {
                    updateAllCommodities(db, result, checkTime);
                } else {
                    Logger.LogInformation($"Eddn already updated for commodities ({checkTime})...");
                }
            }
        }

        public void GetPriceComparisonList() {
            using (Logger.BeginScope($"=>{ nameof(GetEddbCommodities) }")) {
                // Update Information from Inara
                var inara = new InaraPriceScraper();
                var prices = inara.LoadPrices();


            }
        }

        private bool commoditiesRequireUpdate(IDatabase db, DateTime eddnPricesTime) {
            return !db.Exists<DbEddnUpdateLog>(@"type = 'eddn_commodity_update' AND date = @0", eddnPricesTime);
        }

        private void updateAllCommodities(IDatabase db, List<JsonCommodity> commodities, DateTime eddnPricesTime) {
            using (Logger.BeginScope($"=>{ nameof(updateAllCommodities) }")) {
                Logger.LogInformation($"Updating eddn prices ({commodities.Count} prices to be added)...");

                try {
                    db.BeginTransaction();

                    int stats_newCommodities = 0;

                    // Add our update log
                    var updateLog = new DbEddnUpdateLog { Type = "eddn_commodity_update", Date = eddnPricesTime };
                    db.Insert(updateLog);

                    // Add our information
                    int idx = 0;
                    foreach (var x in commodities) {
                        // Query for the commodity
                        var dbCommodity = db.Query<DbCommodity>("SELECT * FROM commodity WHERE id = @0", x.Id).FirstOrDefault();

                        if (dbCommodity == null) {
                            stats_newCommodities++;
                            dbCommodity = new DbCommodity() {
                                Id = x.Id,
                                CategoryId = x.CategoryId,

                                Name = x.Name,
                                IsRare = x.IsRare == 1,
                                IsNonMarketable = x.IsNonMarketable == 1,
                                EdId = x.EdId,

                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            };

                            Logger.LogInformation($"New commodity: Inserted information for {dbCommodity.Name}");
                            db.Insert(dbCommodity);
                        } else {
                            bool changed = false;

                            if (dbCommodity.Name != x.Name) {
                                dbCommodity.Name = x.Name;
                                changed = true;
                            }

                            if (dbCommodity.EdId != x.EdId) {
                                dbCommodity.EdId = x.EdId;
                                changed = true;
                            }

                            if (dbCommodity.IsRare != (x.IsRare == 1)) {
                                dbCommodity.IsRare = x.IsRare == 1;
                                changed = true;
                            }

                            if (dbCommodity.IsNonMarketable != (x.IsNonMarketable == 1)) {
                                dbCommodity.IsNonMarketable = x.IsNonMarketable == 1;
                                changed = true;
                            }

                            if (dbCommodity.CategoryId != (x.CategoryId)) {
                                dbCommodity.CategoryId = x.CategoryId;
                                changed = true;
                            }

                            if (changed) {
                                Logger.LogInformation($"Changed commodity: Updated information for {dbCommodity.Name}");
                                db.Save(dbCommodity);
                            }
                        }

                        // Update the price
                        var dbc = new DbCommodityEddnPrice {
                            CommodityId = x.Id,
                            AveragePrice = x.AveragePrice,

                            PriceBuyMax = x.MaxBuyPrice,
                            PriceBuyMin = x.MinBuyPrice,
                            PriceBuyLowerAverge = x.BuyPriceLowerAverage,
                            
                            PriceSellMax = x.MaxSellPrice,
                            PriceSellMin = x.MinSellPrice,
                            PriceSellUpperAverge = x.SellPriceUpperAverage,
                            
                            CreatedAt = eddnPricesTime,
                            InsertedAt = DateTime.Now
                        };

                        db.Insert(dbc);

                        if (idx % 100 == 0) 
                            Logger.LogInformation($"Updating eddn prices, Progress Update [{idx}/{commodities.Count-1}].");

                        idx++;
                    }

                    db.CompleteTransaction();
                } catch (Exception ex) {
                    Logger.LogError(ex, $"Failed to insert eddn commodity price data...");
                    db.AbortTransaction();
                }
            }
        }

        private void updateCategories(IDatabase db, List<JsonCommodityCategory> categories) {
            using (Logger.BeginScope($"=>{ nameof(updateCategories) }")) {
                Logger.LogInformation("Getting categories...");
                var results = db.Query<DbCommodityCategory>("SELECT * FROM commodity_category").ToList();
                var categoryIds = results.Select(x => x.Id).ToArray();

                // Add new categories
                Logger.LogInformation("Adding new categories...");
                var missingCategories = categories.Where(x => !categoryIds.Contains(x.Id));
                Logger.LogInformation($"New categories that need added: {missingCategories.Count()}");
                
                foreach (var x in missingCategories) {
                    var cc = new DbCommodityCategory { Id = x.Id, Name = x.Name, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
                    db.Insert(cc);

                    results.Add(cc);
                }

                // Now to update categories if the name has changed 
                Logger.LogInformation("Updating changed categories...");
                var updateNames = results.Where(x => categories.Where(y => y.Id == x.Id).First().Name != x.Name);
                Logger.LogInformation($"Changed categories that need updated: {updateNames.Count()}");
                
                foreach (var x in updateNames) {
                    x.Name = categories.Where(y => y.Id == x.Id).First().Name;
                    db.Save(x);
                }

                Logger.LogInformation($"Done updating categories.");
            }
        }
    }
}
