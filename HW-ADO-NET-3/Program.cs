using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace HW_ADO_NET_3
{
    class Program
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private static void RunMainMenu(Database databaseProvider)
        {
            try
            {
                int input;
                do
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("------- Provision Database -------");
                    Console.ResetColor();
                    Console.WriteLine("Options:\n\t1 - Output all\n\t2 - Show min amount of calories\n\t3 - Output products by type\n\t4 - Show all products with cal below indicated\n\t0 - Exit");
                    Console.Write("Input: ");
                    input = Convert.ToInt32(Console.ReadLine());
                    switch (input)
                    {
                        case 0:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Exiting . . .");
                            Console.ResetColor();
                            break;
                        case 1:
                            OutputAllRecords(databaseProvider);
                            break;
                        case 2:
                            OutputMinCal(databaseProvider);
                            break;
                        case 3:
                            Console.Write("Enter type: ");
                            string type = Console.ReadLine();
                            OutputByType(databaseProvider, type);
                            break;
                        case 4:
                            Console.Write("Enter calories: ");
                            int cal = Convert.ToInt32(Console.ReadLine());
                            OutputBelowCal(databaseProvider, cal);
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No such option present");
                            Console.ResetColor();
                            break;
                    }
                    Console.ReadKey();
                    Console.Clear();
                }
                while (input != 0);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message + ". Closing connection");
            }
        }
        static void Main(string[] args)
        {
            Database databaseProvider = new SqlDatabase(connectionString);
            RunMainMenu(databaseProvider);
        }
        private static void OutputBelowCal(Database databaseProvider, int cal)
        {
            try
            {
                using (IDbConnection connection = databaseProvider.CreateConnection())
                {
                    string query = "Select * from Items where Items.Calories < @Cal";
                    IDbCommand command = databaseProvider.CreateCommand(query, connection); 

                    command.Parameters.Add(databaseProvider.CreateParameter("@Cal", cal));

                    connection.Open();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Database connection is open");
                    Console.ResetColor();

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"Inquired calories amount: '{cal}'");
                        Console.ResetColor();
                        Console.WriteLine("{0, -5} {1, -10} {2, -10} {3, -10} {4, -10}", "Id", "Name", "Type", "Color", "Calories");
                        Console.WriteLine(new string('-', 60));
                        while (reader.Read())
                        {
                            Console.WriteLine("{0, -5} {1, -10} {2, -10} {3, -10} {4, -10}", reader["Id"], reader["Name"], reader["Type"], reader["Color"], reader["Calories"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message + ". Closing connection");
                Console.ResetColor();
            }
        }
        private static void OutputMinCal(Database databaseProvider)
        {
            try
            {
                using (IDbConnection connection = databaseProvider.CreateConnection())
                {
                    string query = "Select MIN(Items.Calories) from Items";
                    IDbCommand command = databaseProvider.CreateCommand(query, connection);

                    connection.Open();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Database connection is open");
                    Console.ResetColor();

                    int minCal = Convert.ToInt32(command.ExecuteScalar());
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("Minimum of calories present in the table: " + minCal);
                    Console.ResetColor();
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message + ". Closing connection");
                Console.ResetColor();
            }
        }
        private static void OutputByType(Database databaseProvider, string type)
        {
            try
            {
                if (type.ToLower() != "vegetable" && type.ToLower() != "fruit")
                {
                    throw new Exception("No such type available exception");
                }
                using (IDbConnection connection = databaseProvider.CreateConnection())
                {
                    string query = "Select * From Items Where Type=@Type";
                    string query2 = "Select Count(*) from Items Where Items.Type = @Type";
                    IDbCommand command = databaseProvider.CreateCommand(query, connection);
                    IDbCommand command2 = databaseProvider.CreateCommand(query2, connection);

                    command.Parameters.Add(databaseProvider.CreateParameter("@Type", type));
                    command2.Parameters.Add(databaseProvider.CreateParameter("@Type", type));

                    connection.Open();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Database connection is open");
                    Console.ResetColor();

                    int count = Convert.ToInt32(command2.ExecuteScalar());
                    Console.WriteLine("\nCount: " + count);
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"Inquired type: '{type}'");
                        Console.ResetColor();
                        Console.WriteLine("{0, -5} {1, -10} {2, -10} {3, -10} {4, -10}", "Id", "Name", "Type", "Color", "Calories");
                        Console.WriteLine(new string('-', 60));
                        while (reader.Read())
                        {
                            Console.WriteLine("{0, -5} {1, -10} {2, -10} {3, -10} {4, -10}", reader["Id"], reader["Name"], reader["Type"], reader["Color"], reader["Calories"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message + ". Closing connection");
                Console.ResetColor();
            }

        }
        private static void OutputAllRecords(Database databaseProvider)
        {
            try
            {
                using (IDbConnection connection = databaseProvider.CreateConnection())
                {
                    string query = "Select * from Items";
                    IDbCommand command = databaseProvider.CreateCommand(query, connection);

                    connection.Open();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Database connection is open");
                    Console.ResetColor();

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("\n{0, -5} {1, -10} {2, -10} {3, -10} {4, -10}", "Id", "Name", "Type", "Color", "Calories");
                        Console.WriteLine(new string('-', 60));
                        while (reader.Read())
                        {
                            Console.WriteLine("{0, -5} {1, -10} {2, -10} {3, -10} {4, -10}", reader["Id"], reader["Name"], reader["Type"], reader["Color"], reader["Calories"]);
                        }
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message + ". Closing connection");
                Console.ResetColor();
            }
        }
    }
}
