﻿using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdTemporal.Data.DatabaseModels {
    [TableName("commodity")]
    [PrimaryKey(primaryKey: "id", AutoIncrement = false)]

    public class DbCommodity {

        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("category_id")]
        public long CategoryId { get; set; }

        [Column("is_rare")]
        public bool IsRare { get; set; }

        [Column("is_non_marketable")]
        public bool IsNonMarketable { get; set; }

        [Column("ed_id")]
        public long EdId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // ==============================
        // Relationship

        [Ignore]
        public DbCommodityCategory Category { get; set; } = null;

        [Ignore]
        public IEnumerable<DbCommodityEddnPrice> Prices { get; set; } = null;
    }
}
