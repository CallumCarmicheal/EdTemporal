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
    class CommodityHandler {
        ILogger Logger { get; } = ApplicationLogging.CreateLogger<CommodityHandler>();

        public CommodityHandler() {
            // ...
        }

        public void UpdateAllCommodities() {
            using (Logger.BeginScope($"=>{ nameof(UpdateAllCommodities) }")) {
                Logger.LogInformation("Connecting to database...");

                // Connect to database
                var dbCon = new DBConnection();
                var db = dbCon.Get();

                Logger.LogInformation("Loading listing data...");

                Logger.LogInformation("Downloading commodities...");

                // Download the commodities into a string
                string jsonCommodities = "";

                WebClient wc = new WebClient();
                jsonCommodities = wc.DownloadString("https://eddb.io/archive/v6/commodities.json");
                var result = JsonConvert.DeserializeObject<List<JsonCommodity>>(jsonCommodities);

                // Update the categories
                Logger.LogInformation("Updating categories...");
                List<JsonCommodityCategory> uniqueCategories = result.DistinctBy(x => x.CategoryId).Select(x => x.Category).ToList();
                updateCategories(db, uniqueCategories);

                Logger.LogInformation("Updating eddn prices...");
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
                    var cc = new DbCommodityCategory { Id = x.Id, Name = x.Name };
                    db.Save(cc);

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
