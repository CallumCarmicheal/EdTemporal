using PetaPoco;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdTemporal.Data.DatabaseModels {
    [TableName("commodity_eddn_prices")]
    [PrimaryKey(primaryKey: "id", AutoIncrement = true)]
    public class DbCommodityEddnPrices {
        [Column("id")]
        public long Id { get; set; }
        
        [Column("commodity_id")]
        public long CommodityId { get; set; }

        [Column("avg_price")]
        public long? AveragePrice { get; set; }

        [Column("max_buy_price")]
        public long? PriceBuyMax { get; set; }

        [Column("max_sell_price")]
        public long? PriceSellMax { get; set; }

        [Column("min_buy_price")]
        public long? PriceBuyMin { get; set; }

        [Column("min_sell_price")]
        public long? PriceSellMin { get; set; }

        [Column("buy_price_lower_average")]
        public long PriceBuyLowerAverge { get; set; }

        [Column("sell_price_upper_average")]
        public long PriceSellUpperAverge { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }


        // ==============================
        // Relationship

        [Ignore]
        public DbCommodity Commodity { get; set; } = null;
    }
}
