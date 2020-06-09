using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Org.BouncyCastle.Asn1;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EdTemporal.Data {
    class EddnInformation {
        static readonly Regex whitespaceTrimmer = new Regex(@"\s\s+", RegexOptions.Compiled);


        ILogger Logger { get; } = ApplicationLogging.CreateLogger<EddnInformation>();



        public DateTime PricesUpdatedAt { get; set; }
        public DateTime CommoditiesUpdatedAt { get; set; }

        public EddnInformation() {
            using (Logger.BeginScope($"=>{ nameof(EddnInformation) }")) {
                Logger.LogInformation("Downloading EDDN dates from API webpage...");

                WebClient wc = new WebClient();
                string apiContent = wc.DownloadString("https://eddb.io/api");

                var parser = new HtmlParser();
                var document = parser.ParseDocument(apiContent);

                Logger.LogInformation("Parsing dates.");

                // Queries
                var qryPricesUpdatedAt = whitespaceTrimmer.Replace(getDateFromTable(document, "listings.csv", 4).Replace("\n", ""), "").Split(new string[] { "| " }, StringSplitOptions.None)[1];
                var qryCommsUpdatedAt = whitespaceTrimmer.Replace(getDateFromTable(document, "commodities.json", 2).Replace("\n", ""), "").Split(new string[] { "| " }, StringSplitOptions.None)[1];

                // Get the dates
                var relDateParser = new EdTemporal.Helpers.RelativeDateParser();

                DateTime datePrices = relDateParser.Parse(qryPricesUpdatedAt);
                DateTime dateComms = relDateParser.Parse(qryCommsUpdatedAt);

                PricesUpdatedAt = datePrices;
                CommoditiesUpdatedAt = dateComms;
            }
        }


        private string getDateFromTable(IDocument doc, string file, int column) {
            var query = string.Format("table.table-publicArchive > tbody > tr:contains(\"{0}\") > td:nth-child({1}) > div", file, column);
            var res = doc.QuerySelector(query);

            return res?.TextContent;
        }
    }
}
