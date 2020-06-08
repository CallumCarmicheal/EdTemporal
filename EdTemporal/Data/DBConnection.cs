using MySql.Data.MySqlClient;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdTemporal.Data {
    public class DBConnection {

        //public MySqlConnection Get(bool open = true) {
        //    const string SERVER = "localhost";
        //    const string DATABASE = "application__games__elitedangerous_market_data";
        //    const string USERNAME = "";
        //    const string PASSWORD = "";

        //    string connStr = $"server={SERVER};user={USERNAME};database={DATABASE};password={PASSWORD};";
        //    MySqlConnection conn = new MySqlConnection(connStr);

        //    if (open)
        //        conn.Open();
        //    return conn;
        //}

        public IDatabase Get() {
            const string SERVER   = "localhost";
            const string DATABASE = "application__games__elitedangerous_market_data";
            const string USERNAME = "root";
            const string PASSWORD = "";

            string connStr = $"server={SERVER};user={USERNAME};database={DATABASE};password={PASSWORD};";

            var db = DatabaseConfiguration.Build()
                 .UsingConnectionString(connStr)
                 .UsingProvider<MySqlDatabaseProvider>()
                 .UsingDefaultMapper<ConventionMapper>(m => {
                     m.InflectTableName = (inflector, s) => inflector.Pluralise(inflector.Underscore(s));
                     m.InflectColumnName = (inflector, s) => inflector.Underscore(s);
                 })
                 .Create();

            return db;
        }
    }
}
