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


                //string connString = "Server=192.168.1.9;Port=6606;Database=TeraDataBase;Uid=root;Pwd=mikemike+qnas-1965;"; //mysql - mariadb
                ////string connString = @"Data Source=../../../TeraDataBase.sqlite; version=3;";//sqlite
                string connString = @"Data Source=F:/TeraDataBase/TeraDataBase.sqlite; version=3;";//sqlite
                int rows = 500000; //10 000 000  ----  sqlite insert 500000 regs demora 39min
                                    
                bool reset = false;
                if (args.Length == 3)
                {
                    connString = args[0];
                    rows = System.Convert.ToInt32(args[1]);
                    reset = System.Convert.ToBoolean(args[2]);
                }

                DateTime iniDt = DateTime.Now;
                Console.WriteLine("Begin At: [" + iniDt.ToString() + "]");

                //Begin process
                //MySqlProcess process = new MySqlProcess();
                SqliteProcess process = new SqliteProcess();
                process.Execute(connString, rows, reset);

                DateTime endDt = DateTime.Now;
                Console.WriteLine("End   At: [" + endDt.ToString() + "]");
                TimeSpan diffResult = endDt.Subtract(iniDt);
                Console.WriteLine("Interval: [" + diffResult.ToString() + "]");

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

