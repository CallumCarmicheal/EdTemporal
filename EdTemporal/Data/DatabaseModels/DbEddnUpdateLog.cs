using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdTemporal.Data.DatabaseModels {
    [TableName("eddn_update_log")]
    [PrimaryKey(primaryKey: "id", AutoIncrement = true)]
    class DbEddnUpdateLog {

        [Column("id")]
        public long Id { get; set; }

        [Column("Type")]
        public string Type { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }
    }
}
