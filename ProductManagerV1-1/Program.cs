using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading;

namespace ProductManagerV1_1
{
    class Program
    {
        private static string connectionString =
            @"server=SAIID-PC\SQL19; database=WebMagasin; Integrated Security=SSPI";

        static void Main(string[] args)
        {


            //Console.ReadLine();

            Console.Clear();

            Console.SetCursorPosition(38, 1);
            Console.WriteLine(">>  Product Manager Central Menu  <<");

            Console.SetCursorPosition(0, 5);
            Console.WriteLine($@"
        1. Categories

        2. Articles

        3. Exit

        Please Choose One of Options above: ");

            Regex menuOptionRegex = new Regex(@"[1-3]$");
            string menuOption;
            do
            {
                Console.SetCursorPosition(44, 12);
                menuOption = Console.ReadKey().KeyChar.ToString();
            } while (!menuOptionRegex.IsMatch(menuOption));

            Console.Clear();

            MenuOptions menuOptions = (MenuOptions)int.Parse(menuOption) - 1;

            switch (menuOptions)
            {
                case MenuOptions.Categories:

                    Console.SetCursorPosition(0, 5);
                    Console.WriteLine($@"
        1. Add category

        2. List categories

        3. Add product to category

        Please Choose One of Options above: ");

                    //Regex menuOptionRegex = new Regex(@"[1-3]$");
                    string categoryMenuOption = string.Empty;
                    do
                    {
                        Console.SetCursorPosition(44, 12);
                        categoryMenuOption = Console.ReadKey().KeyChar.ToString();
                    } while (!menuOptionRegex.IsMatch(menuOption));

                    Console.Clear();
                    CategoryMenuOption categoryMenuOptions = (CategoryMenuOption)int.Parse(categoryMenuOption) - 1;

                    switch (categoryMenuOptions)
                    {
                        case CategoryMenuOption.Addcategory:
                            AddCategoryMenu();
                            break;
                        case CategoryMenuOption.Listcategories:
                            break;
                        case CategoryMenuOption.AddProductToCategory:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    
                    break;
                case MenuOptions.Articles:
                    break;
                case MenuOptions.Exit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Console.ReadLine();
            Console.SetCursorPosition(45, 1);
            Console.WriteLine(">>  Categories Table  <<");
            PrintView(5, 4);

            Console.ReadLine();
        }

        private static void AddCategoryMenu()
        {
            Console.SetCursorPosition(5, 2);
            Console.WriteLine("Please Insert a Category Name: ");

            PrintView(5, 10);

            Console.SetCursorPosition(36, 2);
            Categories.Category = Console.ReadLine();

            Console.SetCursorPosition(5, 4);
            Console.WriteLine("Would you Confirm to add this category? Y/N: ");

            
            Regex confirmRegex = new Regex(@"[YN]$");
            ConsoleKeyInfo confirm;
            do
            {
                Console.SetCursorPosition(50, 4);
                confirm = Console.ReadKey();

            } while (!confirmRegex.IsMatch(confirm.Key.ToString()));

            if (confirm.Key == ConsoleKey.Y)
            {
               int addToDB = AddCategories(Categories.Category);

               if (addToDB > 0)
               {
                   Console.SetCursorPosition(5, 4);
                   Console.WriteLine("                                                     ");
                   Console.SetCursorPosition(5, 4);
                    Console.WriteLine("A category added to the database.");
               }

            }
            else
            {
                Console.SetCursorPosition(5, 4);
                Console.WriteLine("                                                     ");
                Console.SetCursorPosition(5, 4);
                Console.WriteLine("No category added to the database.");
                Thread.Sleep(2000);
            }

            

            PrintView(5, 10);
        }

        private static int AddCategories(string category)
        {
            string sqlCommandText = "spAddCategory";
            int addToDB = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlCommandText, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Category", category);
                command.Parameters.AddWithValue("@Inventory", 0);
                connection.Open();
                addToDB = command.ExecuteNonQuery();
            }

            return addToDB;
        }

        private static void PrintView(int left, int top)
        {
            DataSet dataSet = new DataSet();

            string sqlCommandText = "Select * From Categories";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommandText, connection);

                dataAdapter.Fill(dataSet, "Categories");
            }

            Console.SetCursorPosition(left, top);
            Console.WriteLine($"{"ID",-5}{"Category",-15}{"Inventory",-10}");

            Console.SetCursorPosition(left, top + 1);
            Console.WriteLine(new string('=', 30));

            int n = 0;
            foreach (DataRow dataRow in dataSet.Tables["Categories"].Rows)
            {
                Console.SetCursorPosition(left, top + 2 + n);
                Console.WriteLine($"{dataRow["ID"],-5}{dataRow["Category"],-15}{dataRow["Inventory"],-10}");
                n++;
            }
        }

        private enum MenuOptions
        {
            Categories,
            Articles,
            Exit
        }

        private enum CategoryMenuOption
        {
            Addcategory,
            Listcategories,
            AddProductToCategory
        }
    }
}
