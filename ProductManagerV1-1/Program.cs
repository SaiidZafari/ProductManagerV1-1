using Microsoft.Data.SqlClient;
using ProductManagerV1_1.Models.Domain;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;

namespace ProductManagerV1_1
{
    class Program
    {
        private static readonly string connectionString =
            @"server=SAIID-PC\SQL19; database=WebMagasin; Integrated Security=SSPI";

        static void Main()
        {

            //    Console.ReadLine();

            bool leaveHeadMenu = false;

            do
            {

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

                        bool doCaseOneAgain = false;

                        do
                        {
                            Console.Clear();

                            Console.SetCursorPosition(37, 1);
                            Console.WriteLine(">>  Add Categories and Products  <<");

                            Console.SetCursorPosition(95, 1);
                            Console.WriteLine("Push [Esc] to Exit ");

                            Console.SetCursorPosition(0, 5);
                            Console.WriteLine($@"
        1. Add category

        2. List categories

        3. Add product to category

        4. Add category to category

        Please Choose One of Options above: ");

                            Regex menuOption2Regex = new Regex(@"[12345]$");
                            ConsoleKeyInfo categoryMenuOption;
                            do
                            {
                                Console.SetCursorPosition(44, 14);
                                categoryMenuOption = Console.ReadKey();

                                if (categoryMenuOption.Key == ConsoleKey.Escape)
                                {
                                    categoryMenuOption = new ConsoleKeyInfo('5', ConsoleKey.D5, false, false, false);
                                }

                            } while (!menuOption2Regex.IsMatch(categoryMenuOption.KeyChar.ToString()));

                            Console.Clear();
                            CategoryMenuOption categoryMenuOptions = (CategoryMenuOption)int.Parse(categoryMenuOption.KeyChar.ToString()) - 1;

                            switch (categoryMenuOptions)
                            {
                                case CategoryMenuOption.Addcategory:
                                    AddCategoryMenu();
                                    doCaseOneAgain = true;
                                    Console.WriteLine($@"



     Press any key to continue ...");
                                    Console.ReadKey();
                                    break;
                                case CategoryMenuOption.Listcategories:
                                    Console.Clear();
                                    Console.SetCursorPosition(45, 1);
                                    Console.WriteLine(">>  Categories Table  <<");

                                    PrintCategoriesView(45, 8);
                                    doCaseOneAgain = true;

                                    Console.SetCursorPosition(5, 24);
                                    Console.WriteLine($@"Press Esc to continue ...");
                                    //Regex escapeRegex = new Regex(@"[12345]$");
                                    ConsoleKeyInfo escape;
                                    do
                                    {
                                        Console.SetCursorPosition(30, 24);
                                        Console.WriteLine("       ");
                                        Console.SetCursorPosition(30, 24);
                                        escape = Console.ReadKey();

                                    } while (escape.Key != ConsoleKey.Escape);


                                    break;
                                case CategoryMenuOption.AddProductToCategory:
                                    Console.Clear();
                                    Console.SetCursorPosition(37, 1);
                                    Console.WriteLine(">>  Add Product To Category  <<");

                                    PrintCategoriesView(5, 8);

                                    PrintProductView(71, 8);

                                    Console.SetCursorPosition(5, 24);
                                    Console.WriteLine($@"Selected ID>");

                                    Regex idOptionRegex = new Regex(@"\d$");
                                    string idOption;
                                    do
                                    {
                                        Console.SetCursorPosition(18, 24);
                                        Console.WriteLine("       ");
                                        Console.SetCursorPosition(18, 24);
                                        idOption = Console.ReadLine();

                                    } while (!idOptionRegex.IsMatch(idOption ?? throw new InvalidOperationException()));

                                    DataSet dataSetCategory = new DataSet();
                                    int numberOfRows;
                                    string sqlCommandText = "Select * from Categories";
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommandText, connection);

                                        numberOfRows = dataAdapter.Fill(dataSetCategory, "Categories");
                                    }

                                    if (numberOfRows > 0)
                                    {

                                        foreach (DataRow dataRow in dataSetCategory.Tables["Categories"].Rows)
                                        {
                                            if (idOption == dataRow["ID"].ToString())
                                            {
                                                Console.SetCursorPosition(5, 25);
                                                Console.WriteLine($@"[A] Add product");
                                                do
                                                {
                                                    Console.SetCursorPosition(20, 25);
                                                    Console.WriteLine("       ");
                                                    Console.SetCursorPosition(20, 25);
                                                    escape = Console.ReadKey();

                                                } while (escape.Key != ConsoleKey.A);

                                                Console.SetCursorPosition(42, 8);
                                                Console.WriteLine($"{"<<< Categories",17} ");

                                                Console.SetCursorPosition(42, 10);
                                                Console.WriteLine($"{"ID",10} : {dataRow["ID"],-10}");
                                                Console.SetCursorPosition(42, 12);
                                                Console.WriteLine($"{"Category",10} : {dataRow["Category"],-10}");
                                                
                                                Console.SetCursorPosition(42, 14);
                                                Console.WriteLine($"{"Proucts >>>",17} ");

                                                Console.SetCursorPosition(42, 16);
                                                Console.WriteLine($"{"ID",10} :");
                                                Console.SetCursorPosition(39, 18);
                                                Console.WriteLine($"{"FK_Categories",10} : {idOption}");

                                                Regex nameRegex = new Regex(@"^[A-Za-z][A-Za-z0-9]+");
                                                
                                                Regex FKCategoryRegex = new Regex(@"^[0-9]+");
                                                string id;
                                                do
                                                {
                                                    Console.SetCursorPosition(55, 16);
                                                    Console.WriteLine("                      ");
                                                    Console.SetCursorPosition(55, 16);
                                                    id = Console.ReadLine();

                                                } while (!FKCategoryRegex.IsMatch(id ?? throw new InvalidOperationException()));

                                               
                                                string sqlGetRowWithId = "use WebMagasin; Update Products Set FK_Categories = @FkCategory where ID = @IDProduct";

                                                using (SqlConnection connection = new SqlConnection(connectionString))
                                                {
                                                    Products.ID = int.Parse(id);
                                                    Products.FK_Categories = int.Parse(idOption);

                                                    SqlCommand command = new SqlCommand(sqlGetRowWithId, connection);
                                                    command.Parameters.AddWithValue("@IDProduct", Products.ID.ToString());
                                                    command.Parameters.AddWithValue("@FkCategory", Products.FK_Categories);

                                                    connection.Open();

                                                    command.ExecuteNonQuery();

                                                }


                                                Console.SetCursorPosition(40, 25);
                                                Console.WriteLine("Do you verify this change? Y/N: ");
                                                do
                                                {
                                                    Console.SetCursorPosition(79, 25);
                                                    Console.WriteLine("                                            ");
                                                    Console.SetCursorPosition(79, 25);
                                                    escape = Console.ReadKey();
                                                    Console.SetCursorPosition(79, 25);
                                                    Console.WriteLine("                                            ");

                                                } while (escape.Key == ConsoleKey.A);



                                                //DataSet productDataSet = new DataSet();

                                                //string sqlQueryCategory = "Select * From Products";

                                                //using (SqlConnection connection = new SqlConnection(connectionString))
                                                //{
                                                //    SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQueryCategory, connection);

                                                //    dataAdapter.Fill(productDataSet, "Products");
                                                //}

                                                //using (SqlConnection connection = new SqlConnection(connectionString))
                                                //{
                                                //    SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQueryCategory, connection);

                                                //    SqlCommandBuilder builder = new SqlCommandBuilder(dataAdapter);

                                                //    productDataSet.Tables["Products"].Rows.Add(newDataRow);

                                                //    dataAdapter.Update(productDataSet, "Products");
                                                //}

                                                PrintProductView(71, 8);

                                                Console.SetCursorPosition(20, 25);
                                                Console.WriteLine("       ");

                                                Console.ReadKey();
                                                break;
                                            }
                                            else
                                            {
                                                Console.SetCursorPosition(5, 24);
                                                Console.WriteLine("Invalid ID ...   ");
                                                Thread.Sleep(2000);

                                                Console.SetCursorPosition(5, 24);
                                                Console.WriteLine("                  ");
                                            }
                                        }
                                    }


                                    doCaseOneAgain = true;
                                    break;
                                case CategoryMenuOption.AddCategoryToCategory:

                                    Console.Clear();
                                    Console.SetCursorPosition(37, 1);
                                    Console.WriteLine(">>  Add Category To Category  <<");

                                    PrintCategoriesView(5, 8);

                                    Console.SetCursorPosition(5, 24);
                                    Console.WriteLine("Presss any key to continue...");
                                    Console.ReadKey();
                                    break;
                                case CategoryMenuOption.Exit:
                                    doCaseOneAgain = false;
                                    leaveHeadMenu = true;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                        } while (doCaseOneAgain);

                        break;
                    case MenuOptions.Articles:

                        string sqlQuery = "Select * From Products";

                        Console.Clear();


                        DataSet dataSet = ConnectionToSql(connectionString, sqlQuery);
                        //Products products = new Products();

                        int successRow;

                        int left = 2;
                        int top = 3;

                        bool doItAgain;

                        ConsoleKeyInfo keySelectEdit = default;

                        do
                        {
                            Console.Clear();

                            Console.SetCursorPosition(45, 1);
                            Console.WriteLine(">>  Articles Central Menu  <<");

                            Console.WriteLine($@"

        1. Add article

        2. Search article

        3. Exit

        Please Choose one of the options above: ");

                            Regex optionRegex = new Regex(@"[1-3]$");
                            string option;
                            do
                            {
                                Console.SetCursorPosition(48, 9);
                                Console.WriteLine("  ");
                                Console.SetCursorPosition(48, 9);
                                option = Console.ReadKey().KeyChar.ToString().ToUpper();
                            } while (!optionRegex.IsMatch(option));

                            CentralMenu centralMenu = (CentralMenu)int.Parse(option) - 1;

                            switch (centralMenu)
                            {
                                case CentralMenu.AddArticle:

                                    do
                                    {

                                        Console.Clear();

                                        Console.SetCursorPosition(45, 1);
                                        Console.WriteLine(">>  Add Article  <<");

                                        Console.SetCursorPosition(left + 2, top);
                                        Console.WriteLine($"Please insert information for the felts below and press Enter.{"To Central Menu (Exit)",50}");

                                        Console.SetCursorPosition(left, top + 2);
                                        Console.WriteLine($"{"Article number:",17}");

                                        Console.SetCursorPosition(left, top + 3);
                                        Console.WriteLine($"{"(A123B45)",16}");

                                        Console.SetCursorPosition(left + 40, top + 2);
                                        Console.WriteLine($"{"Material:",17}");

                                        Console.SetCursorPosition(left, top + 4);
                                        Console.WriteLine($"{"Name:",17}");

                                        Console.SetCursorPosition(left + 40, top + 4);
                                        Console.WriteLine($"{"Color:",17}");

                                        Console.SetCursorPosition(left, top + 6);
                                        Console.WriteLine($"{"Price:",17}");

                                        Console.SetCursorPosition(left + 40, top + 6);
                                        Console.WriteLine($"{"Description:",17}");


                                        Console.SetCursorPosition(left, top + 12);
                                        ProductsView(ConnectionToSql(connectionString, sqlQuery));


                                        Regex articleNumberRegex = new Regex(@"^[A-Za-z][0-9][0-9][0-9][a-zA-Z][0-9][0-9]$");
                                        do
                                        {
                                            Console.SetCursorPosition(left + 18, top + 2);
                                            Console.WriteLine("                    ");
                                            Console.SetCursorPosition(left + 18, top + 2);
                                            Products.ArticleNumber = Console.ReadLine().ToUpper();

                                            if (Products.ArticleNumber == "EXIT")
                                            {

                                                Products.ArticleNumber = "Z999Z99";
                                            }

                                            foreach (DataRow dataRow in dataSet.Tables["Products"].Rows)
                                            {
                                                if (dataRow["ArticleNumber"].ToString() == Products.ArticleNumber)
                                                {
                                                    Console.SetCursorPosition(left, top + 10);
                                                    Console.WriteLine("                                              ");
                                                    Console.SetCursorPosition(left, top + 10);
                                                    Console.Write(" This Article is already registered !");
                                                    Thread.Sleep(2000);
                                                    Console.SetCursorPosition(left, top + 10);
                                                    Console.WriteLine("                                      ");
                                                    Products.ArticleNumber = "Invalid";
                                                }
                                            }

                                        } while (!articleNumberRegex.IsMatch(Products.ArticleNumber ?? string.Empty));

                                        if (Products.ArticleNumber == "Z999Z99")
                                        {
                                            doItAgain = true;
                                            break;
                                        }

                                        Console.SetCursorPosition(left + 18, top + 2);
                                        Console.WriteLine(Products.ArticleNumber);

                                        Products.Name = AddName(left, top);

                                        Products.Price = AddPrice(left, top);

                                        Products.Material = AddMaterial(left, top);

                                        Products.Color = AddColor(left, top);

                                        Products.Description = AddDescription(left, top);


                                        Console.SetCursorPosition(left, top + 10);
                                        Console.WriteLine($"Would you please confirm the Information above? Y/N ");

                                        //ConsoleKeyInfo answer = Console.ReadKey();



                                        Regex confirmRegex = new Regex(@"[YN]$");
                                        ConsoleKeyInfo confirm;
                                        do
                                        {
                                            Console.SetCursorPosition(left + 52, top + 10);
                                            confirm = Console.ReadKey(true);

                                        } while (!confirmRegex.IsMatch(confirm.Key.ToString()));


                                        Console.SetCursorPosition(left, top + 10);
                                        Console.WriteLine($"                                                     ");

                                        DataRow newDataRow = dataSet.Tables["Products"].NewRow();

                                        newDataRow["ArticleNumber"] = Products.ArticleNumber;
                                        newDataRow["Name"] = Products.Name;
                                        newDataRow["Price"] = Products.Price;
                                        newDataRow["Material"] = Products.Material;
                                        newDataRow["Color"] = Products.Color;
                                        newDataRow["Description"] = Products.Description;


                                        if (confirm.Key == ConsoleKey.Y && newDataRow != null)
                                        {
                                            successRow = AddArticle(connectionString, sqlQuery, dataSet, newDataRow);
                                            MessageByAction(successRow, left, top, "Add");
                                            doItAgain = true;
                                        }
                                        else
                                        {
                                            doItAgain = true;
                                        }

                                    } while (doItAgain);

                                    break;
                                case CentralMenu.SearchArticle:

                                    do
                                    {
                                        doItAgain = false;
                                        Console.Clear();
                                        Console.SetCursorPosition(left, top);
                                        Console.Write(" Please Enter Articel number you want to find:");

                                        Console.SetCursorPosition(left, top + 26);
                                        Console.WriteLine($"                    [EXIT] Main menu");

                                        Console.SetCursorPosition(left, top + 12);
                                        ProductsView(ConnectionToSql(connectionString, sqlQuery));

                                        Regex articleNumberRegex = new Regex(@"^[A-Za-z][0-9][0-9][0-9][a-zA-Z][0-9][0-9]$");
                                        do
                                        {
                                            Console.SetCursorPosition(left + 47, top);
                                            Console.WriteLine("                     ");
                                            Console.SetCursorPosition(left + 47, top);
                                            Products.ArticleNumber = Console.ReadLine()?.ToUpper();

                                            if (Products.ArticleNumber == "EXIT")
                                            {
                                                doItAgain = false;
                                                Products.ArticleNumber = "Z999Z99";
                                            }


                                        } while (!articleNumberRegex.IsMatch(Products.ArticleNumber ?? string.Empty));

                                        if (Products.ArticleNumber != "Z999Z99")
                                        {
                                            int articleIndex = 0;

                                            foreach (DataRow dataRow in dataSet.Tables["Products"].Rows)
                                            {
                                                if (dataRow["ArticleNumber"].ToString() == Products.ArticleNumber)
                                                {
                                                    articleIndex = dataSet.Tables["Products"].Rows.IndexOf(dataRow);
                                                }
                                            }

                                            DataRow dataRows = dataSet.Tables["Products"].Rows[articleIndex];
                                            Console.Clear();
                                            Console.SetCursorPosition(left + 30, top);
                                            Console.WriteLine($"Search result for Article Number {Products.ArticleNumber} :");

                                            if (dataSet.Tables["Products"].Rows.Count == 0)
                                            {
                                                Console.SetCursorPosition(left, top + 2);
                                                Console.WriteLine("Article didn't find in this database.                       ");

                                                Console.SetCursorPosition(left, top + 4);
                                                Console.Write("Press any key to continue .... ");

                                                Console.SetCursorPosition(left + 31, top + 4);
                                                Console.ReadKey();
                                            }
                                            else
                                            {

                                                Console.SetCursorPosition(left, top + 2);
                                                Console.WriteLine($"{"Article number : ",17}{dataRows["ArticleNumber"]}");

                                                Console.SetCursorPosition(left + 40, top + 2);
                                                Console.WriteLine($"{"Material : ",17}{dataRows["Material"]}");

                                                Console.SetCursorPosition(left, top + 4);
                                                Console.WriteLine($"{"Name : ",17}{dataRows["Name"]}");

                                                Console.SetCursorPosition(left+40, top + 4);
                                                Console.WriteLine($"{"Color : ",17}{dataRows["Color"]}");

                                                Console.SetCursorPosition(left, top + 6);
                                                Console.WriteLine($"{"Price : ",17}{dataRows["Price"]}");

                                                Console.SetCursorPosition(left + 40, top + 6);
                                                Console.WriteLine($"{"Description : ",17}{dataRows["Description"]}");

                                                Console.SetCursorPosition(left, top + 12);
                                                ProductsView(ConnectionToSql(connectionString, sqlQuery));

                                                Console.SetCursorPosition(left, top + 26);
                                                Console.WriteLine($"[E] Edit [D] Delete [Esc] Main menu");

                                                Console.SetCursorPosition(left + 36, top + 26);

                                                Regex keySelectEditRegex = new Regex(@"(E|D|Escape)$");

                                                do
                                                {
                                                    keySelectEdit = Console.ReadKey(true);

                                                } while (!keySelectEditRegex.IsMatch(keySelectEdit.Key.ToString()));

                                                if (keySelectEdit.Key == ConsoleKey.Escape)
                                                {
                                                    //doItAgain = true;
                                                    break;
                                                }
                                                else if (keySelectEdit.Key == ConsoleKey.D)
                                                {
                                                    Console.SetCursorPosition(left, top + 26);
                                                    Console.Write("Are you sue you want to delete this article? Y/N: ");

                                                    Regex keySelectYoNRegex = new Regex(@"[YN]$");
                                                    ConsoleKeyInfo keyYoN;
                                                    do
                                                    {
                                                        Console.SetCursorPosition(left + 50, top + 26);
                                                        keyYoN = Console.ReadKey(true);

                                                    } while (!keySelectYoNRegex.IsMatch(keyYoN.Key.ToString()));


                                                    Console.SetCursorPosition(left, top + 26);
                                                    Console.WriteLine(
                                                        $"[E] Edit [D] Delete [Esc] Main menu                           ");


                                                    if (keyYoN.Key == ConsoleKey.Y)
                                                    {
                                                        DeleteArticle(connectionString, sqlQuery, Products.ArticleNumber);

                                                    }
                                                }
                                                else if (keySelectEdit.Key == ConsoleKey.E)
                                                {


                                                    DataRow editDataRow = dataSet.Tables["Products"].Rows[articleIndex];

                                                    bool keyOption = false;
                                                    do
                                                    {
                                                        Products.Name = dataRows["Name"].ToString();

                                                        Products.Price = Convert.ToInt32(dataRows["Price"]);

                                                        Products.Description = dataRows["Description"].ToString();


                                                        Console.SetCursorPosition(left, top + 26);
                                                        Console.WriteLine(      
                                                            $"[N]ame [P]rice [M]aterial [C]olor [D]escription  [Q]uit  Please choose your Option to Edit: ");
                                                        Console.SetCursorPosition(left + 92, top + 26);
                                                        ConsoleKeyInfo keyInfo = Console.ReadKey();

                                                        if (keyInfo.Key == ConsoleKey.Q)
                                                        {
                                                            doItAgain = false;
                                                            keyOption = false;
                                                            //break;
                                                        }
                                                        else
                                                        {
                                                            if (keyInfo.Key == ConsoleKey.N)
                                                            {
                                                                Products.Name = AddName(left - 1, top);
                                                                keyOption = true;
                                                            }
                                                            else if (keyInfo.Key == ConsoleKey.P)
                                                            {
                                                                Products.Price = AddPrice(left - 1, top);
                                                                keyOption = true;
                                                            }
                                                            else if (keyInfo.Key == ConsoleKey.M)
                                                            {
                                                                Products.Material = AddMaterial(left - 1, top);
                                                                keyOption = true;
                                                            }
                                                            else if (keyInfo.Key == ConsoleKey.C)
                                                            {
                                                                Products.Color = AddColor(left - 1, top);
                                                                keyOption = true;
                                                            }
                                                            else if (keyInfo.Key == ConsoleKey.D)
                                                            {
                                                                Products.Description = AddDescription(left - 1, top);
                                                                keyOption = true;
                                                            }
                                                            else
                                                            {
                                                                Console.SetCursorPosition(left + 30, top + 26);
                                                                Console.WriteLine("Invalid Option. Try again...");
                                                                keyOption = true;
                                                            }
                                                        }


                                                        if (keyInfo.Key != ConsoleKey.Q)
                                                        {
                                                            editDataRow["ArticleNumber"] = Products.ArticleNumber;
                                                            editDataRow["Name"] = Products.Name;
                                                            editDataRow["Price"] = Products.Price;
                                                            editDataRow["Material"] = Products.Material;
                                                            editDataRow["Color"] = Products.Color;
                                                            editDataRow["Description"] = Products.Description;

                                                            Console.SetCursorPosition(left, top + 26);
                                                            Console.Write(
                                                                "Would you verify this change? Y/N :                                                     ");

                                                            Regex keySelectYoNRegex = new Regex(@"[YN]$");
                                                            ConsoleKeyInfo keyYoN;
                                                            do
                                                            {
                                                                Console.SetCursorPosition(left + 37, top + 26);
                                                                keyYoN = Console.ReadKey(true);

                                                            } while (!keySelectYoNRegex.IsMatch(keyYoN.Key.ToString()));

                                                            if (keyYoN.Key == ConsoleKey.Y)
                                                            {
                                                                using (SqlConnection connection = new SqlConnection(connectionString))
                                                                {
                                                                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, connection);

                                                                    SqlCommandBuilder builder = new SqlCommandBuilder(dataAdapter);

                                                                    //dataSet.Clear();

                                                                    dataAdapter.Fill(dataSet, "Products");

                                                                    successRow = dataAdapter.Update(dataSet, "Products");
                                                                }

                                                                if (successRow > 0)
                                                                {
                                                                    Console.SetCursorPosition(left, top + 10);
                                                                    Console.Write(" This Article is registered !");
                                                                    Thread.Sleep(2000);
                                                                }
                                                                else
                                                                {
                                                                    Console.SetCursorPosition(left, top + 10);
                                                                    Console.Write(" This Article didn't registered !");
                                                                    Thread.Sleep(2000);
                                                                }

                                                                Console.SetCursorPosition(left, top + 10);
                                                                Console.WriteLine("                                      ");
                                                                Console.SetCursorPosition(left, top + 12);
                                                                ProductsView(ConnectionToSql(connectionString, sqlQuery));
                                                                doItAgain = true;
                                                            }
                                                            else
                                                            {
                                                                doItAgain = true;
                                                            }
                                                        }


                                                    } while (keyOption);
                                                }

                                            }
                                        }
                                        else
                                        {
                                            doItAgain = false;
                                        }

                                        if (keySelectEdit.Key.ToString() != "0")
                                        {
                                            Console.Clear();

                                            Console.SetCursorPosition(left + 2, top);
                                            Console.Write("Do you wish to search for any other Article ? Y/N :");

                                            Console.SetCursorPosition(left, top + 12);
                                            ProductsView(ConnectionToSql(connectionString, sqlQuery));

                                            Regex answerRegex = new Regex(@"[YN]$");
                                            ConsoleKeyInfo answer = default;
                                            do
                                            {
                                                Console.SetCursorPosition(left + 53, top);
                                                answer = Console.ReadKey(true);

                                            } while (!answerRegex.IsMatch(answer.Key.ToString()));

                                            doItAgain = answer.Key == ConsoleKey.Y;
                                        }

                                    } while (doItAgain);

                                    doItAgain = true;
                                    break;
                                case CentralMenu.Exit:
                                    doItAgain = false;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                        } while (doItAgain);

                        Console.Clear();
                        Console.WriteLine($@"

                     Product Manager v1.0 
                                  ADO.NET

                                 The End");

                        Console.SetCursorPosition(left, top + 12);
                        ProductsView(ConnectionToSql(connectionString, sqlQuery));

                        Thread.Sleep(3000);

                        //Console.SetCursorPosition(45, 1);
                        //Console.WriteLine(">>  Articles  <<");

                        //PrintProductView(35,8);

                        //Console.SetCursorPosition(5, 24);
                        //Console.WriteLine("Press any key to continue...");
                        //Console.ReadKey();

                        leaveHeadMenu = true;
                        break;
                    case MenuOptions.Exit:

                        leaveHeadMenu = false;

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

            } while (leaveHeadMenu);



            //Console.ReadLine();
            Console.SetCursorPosition(45, 1);
            Console.WriteLine(">>  Database Tables  <<");
            PrintCategoriesView(5, 4);
            PrintProductView(71, 4);
            Console.SetCursorPosition(0, 24);
            //Console.ReadLine();
        }


        private static void AddCategoryMenu()
        {
            Console.SetCursorPosition(5, 2);
            Console.WriteLine("Please Insert a Category Name: ");

            PrintCategoriesView(5, 10);

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


                int addToDb = ControlAndAddCategories(Categories.Category);

                if (addToDb > 0)
                {
                    Console.SetCursorPosition(5, 4);
                    Console.WriteLine("                                                     ");
                    Console.SetCursorPosition(5, 4);
                    Console.WriteLine("A category added to the database.");
                }
                else
                {
                    Console.SetCursorPosition(5, 4);
                    Console.WriteLine("                                                     ");
                    Console.SetCursorPosition(5, 4);
                    Console.WriteLine("Category aleady exists.");
                    Thread.Sleep(2000);
                }

            }




            PrintCategoriesView(5, 10);
        }

        private static int ControlAndAddCategories(string category)
        {
            DataSet dataSet = new DataSet();

            int addToDb = 2;

            string sqlCommandText = "Select * From Categories";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommandText, connection);

                dataAdapter.Fill(dataSet, "Categories");
            }


            foreach (DataRow dataRow in dataSet.Tables["Categories"].Rows)
            {

                if (dataRow["Category"].ToString() == category)
                {
                    addToDb = 0;
                    break;
                }

            }

            if (addToDb > 0)
            {
                addToDb = AddCategories(category);
            }

            return addToDb;
        }

        private static int AddCategories(string category)
        {
            string sqlCommandText = "spAddCategory";
            int addToDb;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlCommandText, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Category", category);
                command.Parameters.AddWithValue("@Inventory", 0);
                connection.Open();
                addToDb = command.ExecuteNonQuery();
            }

            return addToDb;
        }

        private static void PrintCategoriesView(int left, int top)
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

        private static void PrintProductView(int left, int top)
        {
            DataSet productDataSet = new DataSet();

            string sqlQuery = "Select * From Products";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, connection);

                dataAdapter.Fill(productDataSet, "Products");
            }
            Console.SetCursorPosition(left, top);
            Console.WriteLine($"{"ID",-4}{"Name",-9}{"Material",-11}{"Color",-8}{"FK_Categories",-10}");
            Console.SetCursorPosition(left, top + 1);
            Console.WriteLine(new string('=', 45));
            int n = 0;
            foreach (DataRow pDataRow in productDataSet.Tables["Products"].Rows)
            {
                Console.SetCursorPosition(left, top + 2 + n);
                Console.WriteLine($"{pDataRow["ID"],-4}{pDataRow["Name"],-9}{pDataRow["Material"],-11}{pDataRow["Color"],-8}{pDataRow["FK_Categories"],-10}");
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
            AddProductToCategory,
            AddCategoryToCategory,
            Exit
        }


        private static void MessageByAction(int successRow, int left, int top, string action)
        {

            if (successRow > 0)
            {
                Console.SetCursorPosition(left, top + 10);
                Console.Write($" This Article is {action} !");
                Thread.Sleep(2000);
            }
            else
            {
                Console.SetCursorPosition(left, top + 10);
                Console.Write($" This Article dosen't {action } !");
                Thread.Sleep(2000);
            }

            Console.SetCursorPosition(left, top + 10);
            Console.WriteLine("                                      ");
        }

        public static int AddArticle(string connectionString, string sqlQuarry, DataSet dataSet, DataRow newDataRow)
        {
            if (newDataRow == null)
            {
                throw new NullReferenceException("Article cant be Null");
            }
            int successRowNumber;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuarry, connection);

                SqlCommandBuilder builder = new SqlCommandBuilder(dataAdapter);

                //dataAdapter.Fill(dataSet, "Products");

                dataSet.Tables["Products"].Rows.Add(newDataRow);

                successRowNumber = dataAdapter.Update(dataSet, "Products");
            }

            return successRowNumber;
        }

        public static int DeleteArticle(string connectionString, string sqlQuarry, string articleNumber)
        {

            //if (rowIndex == null)
            //{
            //    throw new NullReferenceException("Article cant be Null");
            //}


            //dataSet.Clear();
            int successRowNumber;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuarry, connection);

                SqlCommandBuilder builder = new SqlCommandBuilder(dataAdapter);

                DataSet dataSet = new DataSet();

                dataAdapter.Fill(dataSet, "Products");


                int articleIndex = 0;
                foreach (DataRow dataRow in dataSet.Tables["Products"].Rows)
                {
                    if (dataRow["ArticleNumber"].ToString() == articleNumber)
                    {
                        articleIndex = dataSet.Tables["Products"].Rows.IndexOf(dataRow);
                    }
                }

                DataRow deleteDataRow = dataSet.Tables["Products"].Rows[articleIndex];

                deleteDataRow.Delete();

                successRowNumber = dataAdapter.Update(dataSet, "Products");
            }

            return successRowNumber;
        }

        public static DataSet ConnectionToSql(string connectionString, string sqlQuarry)
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuarry, connection);

                dataSet.Clear();

                dataAdapter.Fill(dataSet, "Products");
            }

            return dataSet;
        }


        public static void ProductsView(DataSet dataSet)
        {

            if (dataSet.Tables["Products"].Rows.Count > 0)
            {
                Console.WriteLine($"{"",1}{"Index",-7}{"ID",-4}{"Article Number",-16}{"Name",-10}{"Material",-10}{"Color",-10}{"Price",-10}{"Description",-10}");

                Console.WriteLine($"{"",3}{new string('=', 113)}");

                foreach (DataRow dataRow in dataSet.Tables["Products"].Rows)
                {
                    Console.WriteLine($"{"",3}{dataSet.Tables["Products"].Rows.IndexOf(dataRow),-7}{dataRow["ID"],-4}{dataRow["ArticleNumber"],-16}{dataRow["Name"],-10}{dataRow["Material"],-10}{dataRow["Color"],-10}{dataRow["Price"],-10}{dataRow["Description"],-10}");
                }
            }
            else
            {
                Console.WriteLine($"{"Article Number",-16}{"Name",-22}{"Price",-10}{"Description",-10}");

                Console.WriteLine(new string('=', 86));

                Console.WriteLine("The Database is Empty!");
            }

        }


        private static string AddName(int left, int top)
        {
            string name;
            Regex nameRegex = new Regex(@"^[A-Za-z][a-zA-Z0-9]*");
            do
            {
                Console.SetCursorPosition(left + 18, top + 4);
                Console.WriteLine("                        ");
                Console.SetCursorPosition(left + 18, top + 4);
                name = Console.ReadLine()?.ToUpper();
            } while (!nameRegex.IsMatch(name ?? string.Empty));

            return name;
        }


        private static int AddPrice(int left, int top)
        {
            int price;
            Regex priceRegex = new Regex(@"^[0-9]+$");
            string priceTemp;
            do
            {
                Console.SetCursorPosition(left + 18, top + 6);
                Console.WriteLine("                           ");
                Console.SetCursorPosition(left + 18, top + 6);
                priceTemp = Console.ReadLine();

            } while (!priceRegex.IsMatch(priceTemp ?? string.Empty));

            price = int.Parse(priceTemp);
            return price;
        }

        private static string AddMaterial(int left, int top)
        {
            string material;
            Regex nameRegex = new Regex(@"^[A-Za-z][a-zA-Z0-9]*");
            do
            {
                Console.SetCursorPosition(left + 58, top + 2);
                Console.WriteLine("                                               ");
                Console.SetCursorPosition(left + 58, top + 2);
                material = Console.ReadLine()?.ToUpper();
            } while (!nameRegex.IsMatch(material ?? string.Empty));

            return material;
        }

        private static string AddColor(int left, int top)
        {
            string color;
            Regex nameRegex = new Regex(@"^[A-Za-z][a-zA-Z0-9]*");
            do
            {
                Console.SetCursorPosition(left + 58, top + 4);
                Console.WriteLine("                                               ");
                Console.SetCursorPosition(left + 58, top + 4);
                color = Console.ReadLine()?.ToUpper();
            } while (!nameRegex.IsMatch(color ?? string.Empty));

            return color;
        }

        private static string AddDescription(int left, int top)
        {
            string description;
            Regex descriptionRegex = new Regex(@"^[A-Za-z][a-zA-Z0-9]*");
            do
            {
                Console.SetCursorPosition(left + 58, top + 6);
                Console.WriteLine("                               ");
                Console.SetCursorPosition(left + 58, top + 6);
                description = Console.ReadLine();
            } while (!descriptionRegex.IsMatch(description ?? string.Empty));

            return description;
        }

        public enum CentralMenu
        {
            AddArticle,
            SearchArticle,
            Exit
        }

    }
}
