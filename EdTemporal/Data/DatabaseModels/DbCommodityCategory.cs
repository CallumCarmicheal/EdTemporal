using PetaPoco;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdTemporal.Data.DatabaseModels {
    [TableName("commodity_category")]
    [PrimaryKey(primaryKey: "id", AutoIncrement = false)]
    public class DbCommodityCategory {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }


        // ==============================
        // Relationship

        [Ignore]
        public IEnumerable<DbCommodity> Commodity { get; set; } = null;
    }
}
