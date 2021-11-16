using System;
using System.Data.SQLite;

namespace ConsoleApp1
{
    public class SqliteProcess
    {
        private Random _random = null;
        private SQLiteConnection _conn = null;
        private readonly string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public SqliteProcess()
        {
            _random = new Random();
        }

        public void Execute(string connString, int newProductRows, bool reset = false)
        {
            try
            {
                string ini = DateTime.Now.ToString();

                if (reset)
                    CreateDatabase(connString);

                _conn = new SQLiteConnection(connString);
                _conn.Open();
                
                if (reset)
                {
                    CreateTableProducts();
                    CreateTableCategories();
                    CreateTableCategoryGroups();
                }

                InsertMultiProduct(newProductRows);

                if (reset)
                {
                    InsertMultiCategory();
                    InsertMultiCategoryGroup();
                }

                _conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: [" + ex.Message + "]");
            }

        }

        private void CreateDatabase(string connString)
        {
            try
            {
                int pos1 = connString.IndexOf("=");
                int pos2 = connString.IndexOf(";");
                if (pos2 == -1)
                    pos2 = connString.Length;

                string fileName = connString.Substring(pos1 + 1, pos2 - pos1 - 1);

                //if (!System.IO.File.Exists(fileName))
                {
                    Console.WriteLine("Create '" + fileName + "'");
                    SQLiteConnection.CreateFile(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR CreateDatabase:" + ex.Message);
                throw;
            }
        }

        private void CreateTableProducts()
        {
            //string sql = @"CREATE TABLE IF NOT EXISTS Products (
            string sql = @"CREATE TABLE Products (
                Id integer PRIMARY KEY AUTOINCREMENT,
                Guid text NOT NULL,
                Name text NOT NULL,
                Price integer NOT NULL,
                Validation text NOT NULL,
                CategoryId integer NOT NULL)";

            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, _conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR CreateTableProducts:" + ex.Message);
                throw;
            }
        }

        private void CreateTableCategories()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS Categories (
                Id integer PRIMARY KEY AUTOINCREMENT,
                Name text NOT NULL,
                CategoryGroupId integer NOT NULL)";

            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, _conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR CreateTableProducts:" + ex.Message);
                throw;
            }
        }

        private void CreateTableCategoryGroups()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS CategoryGroups (
                Id integer PRIMARY KEY AUTOINCREMENT,
                Name text NOT NULL)";

            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, _conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR CreateTableCategoryGroups:" + ex.Message);
                throw;
            }
        }

        private void InsertProduct(string guid, string name, int price, string validation, int categoryId)
        {
            string sql = "INSERT INTO products VALUES (NULL, '" + guid + "', '" + name + "', " + price.ToString() + ", '" + validation + "', " + categoryId.ToString() + ")";

            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, _conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR InsertProduct:" + ex.Message);
                Console.ReadKey();

                throw;
            }
        }

        private void InsertCategory(string name, int categoryGroupId)
        {
            string sql = "INSERT INTO Categories VALUES (NULL, '" + name + "', " + categoryGroupId.ToString() + ")";

            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, _conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR InsertCategory:" + ex.Message);
                throw;
            }
        }
        private void InsertCategoryGroup(string name)
        {
            string sql = "INSERT INTO CategoryGroups VALUES (NULL, '" + name + "')";

            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, _conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR InsertCategory:" + ex.Message);
                throw;
            }
        }

        private void InsertMultiProduct(int numRows)
        {
            for (int x = 0; x < numRows; x++)
            {
                string guid = Guid.NewGuid().ToString();
                string name = RandomString();
                int price = _random.Next(1, Constants.MaxPrice);
                string validation = RandomDateTime().ToString("yyyy/MM/dd");
                int categoryId = _random.Next(1, Constants.MaxCategory);

                InsertProduct(guid, name, price, validation, categoryId);

                Console.WriteLine("InsertProduct:" + x.ToString() + "|" + categoryId.ToString());
            }
        }

        private void InsertMultiCategory()
        {
            for (int x = 1; x <= Constants.MaxCategory; x++)
            {
                string name = "Category - " + x.ToString("0000");
                int categoryGroupId = _random.Next(1, Constants.MaxCategoryGroup);

                InsertCategory(name, categoryGroupId);
            }
        }


        private void InsertMultiCategoryGroup()
        {
            for (int x = 1; x <= Constants.MaxCategoryGroup; x++)
            {
                string name = "Group - " + x.ToString("00");

                InsertCategoryGroup(name);
            }
        }

        private string RandomString()
        {
            var stringChars = new char[255];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = _chars[_random.Next(_chars.Length)];
            }

            return new String(stringChars);
        }

        private DateTime RandomDateTime()
        {
            int days = _random.Next(1, Constants.MaxDays);
            DateTime newDate = DateTime.Now;
            newDate.AddDays(System.Convert.ToDouble(days));

            return newDate;
        }

    }
}
