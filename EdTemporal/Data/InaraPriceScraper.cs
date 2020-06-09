using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EdTemporal.Data {
    public class InaraPriceScraper {
        ILogger Logger { get; } = ApplicationLogging.CreateLogger<InaraPriceScraper>();

        public List<InaraCommodity> LoadPrices() {
            using (Logger.BeginScope($"=>{ nameof(LoadPrices) }")) {
                Logger.LogInformation("Loading pricing from Inara...");
                WebClient wc = new WebClient();
                string htmlContent = wc.DownloadString("https://inara.cz/galaxy-commodities/");

                var parser = new HtmlParser();
                var document = parser.ParseDocument(htmlContent);

                var query = document.QuerySelectorAll(".mainblock.maintable table tbody tr:not(.subheader)");

                foreach (var elem in query) {
                    var link = elem.QuerySelector("a");
                    var commodityText = link.TextContent;
                    var commodityUrl = link.GetAttribute("href");
                    var commodityId = commodityUrl.Replace("/galaxy-commodity/", "").Split('/')[0].Trim();

                    var x = 0;
                }
            }

            return new List<InaraCommodity>();
        }

        public class InaraCommodity {
            public long Id { get; set; }

            public long AvgSell { get; set; }
            public long AvgBuy { get; set; }
            public long AvgProfit { get; set; }

            public long MaxSell { get; set; }
            public long MaxBuy { get; set; }
            public long MaxProfit { get; set; }
        }
    }
}
