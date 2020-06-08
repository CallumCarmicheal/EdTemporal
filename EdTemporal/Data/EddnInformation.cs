using AngleSharp;
using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdTemporal.Data {
    class EddnInformation {
        ILogger Logger { get; } = ApplicationLogging.CreateLogger<EddnInformation>();



        public DateTime PricesUpdatedAt { get; set; }
        public DateTime Commodities { get; set; }

        public EddnInformation() {
            using (Logger.BeginScope($"=>{ nameof(EddnInformation) }")) {

                var config = Configuration.Default.WithDefaultLoader();
                var address = "https://eddb.io/api";
                var context = BrowsingContext.New(config);
                var documentTask = context.OpenAsync(address);
                documentTask.Wait();

                var document = documentTask.Result;

                // Queries
                var qryPricesUpdatedAt = getDateFromTable(document, "listings.csv", 3).Replace("\n", "");
                var qryCommsUpdatedAt = getDateFromTable(document, "commodities.json", 2).Replace("\n", "");


            }
        }


        private string getDateFromTable(IDocument doc, string file, int column) {
            var query = $"table.table-publicArchive > tbody > tr:contains(\"{file}\") > td:nth-child({column}) > div'";
            return doc.QuerySelector(query).TextContent;
        }
    }
}
