using PetaPoco;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdTemporal.Data.DatabaseModels {
    [TableName("commodity_eddn_prices")]
    [PrimaryKey("id")]
    public class DbCommodityEddnPrices {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("commodity_id")]
        public int CommodityId { get; set; }


        [Column("max_buy_price")]
        public int PriceBuyMax { get; set; }

        [Column("max_sell_price")]
        public int PriceSellMax { get; set; }

        [Column("min_buy_price")]
        public int PriceBuyMin { get; set; }

        [Column("min_sell_price")]
        public int PriceSellMin { get; set; }

        [Column("buy_price_lower_average")]
        public int PriceBuyLowerAverge { get; set; }

        [Column("sell_price_upper_average")]
        public int PriceSellUpperAverge { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }


        // ==============================
        // Relationship

        [Ignore]
        public DbCommodity Commodity { get; set; } = null;
    }
}
