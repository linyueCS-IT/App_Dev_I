using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data.SQLite;
namespace Sqlite_Practice
{
    internal class Program
    {
        static void Main(string[] args)
        {

            GetVersion();

            CreateTable();

            PreparedStatements();

            SqlDataReader();

            ColumnHeaders();
        }

        //=====================================
        // C# SQLite version
        //=====================================
        public static void GetVersion()
        {
            string cs = "Data Source=:memory:";
            string stm = "SELECT SQLITE_VERSION()";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(stm, con);
            string version = cmd.ExecuteScalar().ToString();

            Console.WriteLine($"SQLite version: {version}");
        }

        //=====================================
        // C# SQLite create table
        //=====================================
        public static void CreateTable()
        {
            string cs = @"URI=file:C:\sqlite\chinook.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DROP TABLE IF EXISTS cars";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"create table cars(
                                id integer primary key,
                                name text,
                                price int
                                )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "insert into cars (name, price) values ('Audi', 52642)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Mercedes',57127)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Skoda',9000)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Volvo',29000)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Bentley',350000)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Citroen',21000)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Hummer',41400)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Volkswagen',21600)";
            cmd.ExecuteNonQuery();

            Console.WriteLine("Table cars created");
        }
        //==============================================================================================
        // When we write prepared statements, we use placeholders instead of directly writing the values
        // into the statements. Prepared statements are faster and guard against SQL injection attacks.
        // The @name and @price are placeholders, which are going to be filled later.
        //==============================================================================================
        public static void PreparedStatements()
        {
            string cs = @"URI=file:C:\sqlite\chinook.db";
            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "insert into cars(name, price) values (@name, @price)";

            cmd.Parameters.AddWithValue("@name", "BMW");
            cmd.Parameters.AddWithValue("@price", 36600);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Console.WriteLine("row inserted");
        }
        //==============================================================================================
        // We create an instance of the SQLiteDataReader by calling the ExecuteReader method of the
        // SQLiteCommand object. While the SqlDataReader is being used, the associated SQLiteConnection serves
        // the SqlDataReader. No other operations can be performed on the SQLiteConnection other than closing it.
        //==============================================================================================
        public static void SqlDataReader()
        {
            string cs = @"URI=file:C:\sqlite\chinook.db";
            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = "select * from cars limit 5";

            using var cmd = new SQLiteCommand(stm,con);
            using SQLiteDataReader rdr = cmd.ExecuteReader(); 
            
            while (rdr.Read())
            {
                Console.WriteLine($"{rdr.GetInt32(0)} {rdr.GetString(1)} {rdr.GetInt32(2)}");
            }
        }
        //==============================================================================================
        // In the following example we print column headers with the data from a database table.
        //==============================================================================================
        public static void ColumnHeaders()
        {
            string cs = @"URI=file:C:\sqlite\chinook.db";
            using var con = new SQLiteConnection (cs);
            con.Open();

            string stm = "select * from cars limit 5";

            using var cmd = new SQLiteCommand (stm,con);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            Console.WriteLine($"{rdr.GetName(0),-3} {rdr.GetName(1),-8} {rdr.GetName(2),8}");

            while (rdr.Read())
            {
                Console.WriteLine($"{rdr.GetInt32(0),-3} {rdr.GetString(1),-8} {rdr.GetInt32(2),8}");
            }
        }
    }
}
