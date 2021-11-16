using System;
using System.Data.SQLite;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Program Started...");

                //Console.WriteLine("...---...");
                //Console.WriteLine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                //Console.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory);
                //Console.WriteLine(System.Environment.CurrentDirectory);
                //Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
                //Console.WriteLine(Environment.CurrentDirectory);
                //Console.WriteLine(System.AppContext.BaseDirectory);
                //Console.WriteLine("---...---");
                //Console.ReadKey();

                                            
                string connString = "Server=192.168.1.9;Port=6606;Database=TeraDataBase;Uid=root;Pwd=mikemike+qnas-1965;"; //mysql - mariadb
                //string connString = @"Data Source=../../../TeraDataBase.sqlite; version=3;";//sqlite
                int rows = 100; //10 000 000
                
                bool reset = false;
                if (args.Length == 3)
                {
                    connString = args[0];
                    rows = System.Convert.ToInt32(args[1]);
                    reset = System.Convert.ToBoolean(args[2]);
                }

                MySqlProcess process = new MySqlProcess();
                //SqliteProcess process = new SqliteProcess();
                process.Execute(connString, rows, reset);

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: [" + ex.Message + "]");
            }

            Console.WriteLine("Program Finished...");
            Console.ReadKey();
        }

    }
}

