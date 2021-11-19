using MySql.Data.MySqlClient;
using System;


namespace ConsoleApp1
{
    public class MySqlProcess
    {
        private Random _random = null;
        private MySqlConnection _conn = null;
        private readonly string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public MySqlProcess()
        {
            _random = new Random();
        }

        public void Execute(string connString, int newProductRows, bool reset = false)
        {
            try
            {
                _conn = new MySqlConnection(connString);
                _conn.Open();

                if (reset)
                    CreateDatabase(connString);

                if (reset)
                {
                    CreateTableProducts();
                    CreateTableCategories();
                    CreateTableCategoryGroups();
                    //InsertMultiCategory(); //import categories.sql
                    InsertMultiCategoryGroup();
                }

                InsertMultiProduct(newProductRows);

                _conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: [" + ex.Message + "]");
            }

        }

        private void CreateDatabase(string connString)
        {
            //string sql = @"CREATE DATABASE IF NOT EXISTS TeraDataBase";

            //try
            //{
            //    MySqlCommand comm = _conn.CreateCommand();
            //    comm.CommandText = sql;
            //    //comm.Parameters.Add("@person", "Myname");
            //    //comm.Parameters.Add("@address", "Myaddress");
            //    comm.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("ERROR CreateDatabase:" + ex.Message);
            //    throw;
            //}
        }

        private void CreateTableProducts()
        {
            //string sql = @"CREATE TABLE Products (
            string sql = @"CREATE TABLE IF NOT EXISTS Products (
                Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                Guid VARCHAR(36) NOT NULL,
                Name VARCHAR(255) NOT NULL,
                Price INT NOT NULL,
                Validation VARCHAR(10) NOT NULL,
                CategoryId INT NOT NULL)";

            try
            {
                MySqlCommand comm = _conn.CreateCommand();
                comm.CommandText = sql;
                comm.ExecuteNonQuery();
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
                Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                Name VARCHAR(255) NOT NULL,
                CategoryGroupId INT NOT NULL)";

            try
            {
                MySqlCommand comm = _conn.CreateCommand();
                comm.CommandText = sql;
                comm.ExecuteNonQuery();
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
                Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                Name VARCHAR(255) NOT NULL)";

            try
            {
                MySqlCommand comm = _conn.CreateCommand();
                comm.CommandText = sql;
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR CreateTableCategoryGroups:" + ex.Message);
                throw;
            }
        }

        private void InsertProduct(string guid, string name, int price, string validation, int categoryId)
        {
            string sql = "INSERT INTO Products VALUES (NULL, '" + guid + "', '" + name + "', " + price.ToString() + ", '" + validation + "', " + categoryId.ToString() + ")";

            try
            {
                MySqlCommand comm = _conn.CreateCommand();
                comm.CommandText = sql;
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR InsertProduct:" + ex.Message);
                Console.ReadKey();

                throw;
            }
        }
        //private void UpdateProduct(int id, string guid, string name, int price, string validation, int categoryId)
        private void UpdateProduct(int id, int categoryId)
        {
            string sql = "UPDATE Products SET categoryId = @CategoryId WHERE Id = @Id";

            try
            {
                MySqlCommand comm = _conn.CreateCommand();
                comm.Parameters.AddWithValue("@CategoryId", categoryId);
                comm.Parameters.AddWithValue("@Id", id);
                comm.CommandText = sql;
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR UpdateProduct:" + ex.Message);
                Console.ReadKey();

                throw;
            }
        }
        private void InsertCategory(string name, int categoryGroupId)
        {
            string sql = "INSERT INTO Categories VALUES (NULL, '" + name + "', " + categoryGroupId.ToString() + ")";

            try
            {
                MySqlCommand comm = _conn.CreateCommand();
                comm.CommandText = sql;
                comm.ExecuteNonQuery();
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
                MySqlCommand comm = _conn.CreateCommand();
                comm.CommandText = sql;
                comm.ExecuteNonQuery();
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
                int categoryId = _random.Next(1, Constants.MaxCategory + 1);

                InsertProduct(guid, name, price, validation, categoryId);

                Console.WriteLine("InsertProduct:" + x.ToString());
            }
        }

        private void InsertMultiCategory()
        {
            for (int x = 1; x <= Constants.MaxCategory; x++)
            {
                string name = "Category - " + x.ToString("0000");
                int categoryGroupId = _random.Next(1, Constants.MaxCategoryGroup + 1);

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
            DateTime nowDate = DateTime.Now;
            DateTime newDate = nowDate.AddDays(System.Convert.ToDouble(days));

            return newDate;
        }

    }
}
