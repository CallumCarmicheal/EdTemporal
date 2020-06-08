using System;
using System.Collections.Generic;

using System.Globalization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace EdTemporal.Data.EDDN.V6 {
    public partial class JsonCommodity {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category_id")]
        public long CategoryId { get; set; }

        [JsonProperty("average_price")]
        public long AveragePrice { get; set; }

        [JsonProperty("is_rare")]
        public long IsRare { get; set; }

        [JsonProperty("max_buy_price")]
        public long MaxBuyPrice { get; set; }

        [JsonProperty("max_sell_price")]
        public long MaxSellPrice { get; set; }

        [JsonProperty("min_buy_price")]
        public long MinBuyPrice { get; set; }

        [JsonProperty("min_sell_price")]
        public long MinSellPrice { get; set; }

        [JsonProperty("buy_price_lower_average")]
        public long BuyPriceLowerAverage { get; set; }

        [JsonProperty("sell_price_upper_average")]
        public long SellPriceUpperAverage { get; set; }

        [JsonProperty("is_non_marketable")]
        public long IsNonMarketable { get; set; }

        [JsonProperty("ed_id")]
        public long EdId { get; set; }

        [JsonProperty("category")]
        public JsonCommodityCategory Category { get; set; }
    }
}
