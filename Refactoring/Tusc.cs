using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        public static bool loggedIn = false;      //User is logged in?
        public static List<User> lUsers;
        public static List<Product> lProds;

        //public Tusc(List<User> users, List<Product> prods)
        //{
        //    lUsers = users;
        //    lProds = prods;
        //}

        private static void WelcomeMessage()
        {
            // Write welcome message
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");
        }

        private static void LogInSuccessfulMessage(string name)
        {
            // Show welcome message
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Login successful! Welcome " + name + "!");
            Console.ResetColor();
        }

        private static bool isCorrectPassword(string sName, string sPwd)
        {
            if(!isUserInputValid(sPwd))
            {
                Console.WriteLine();
                Console.WriteLine("You entered an invalid password");
            }
            foreach (var user in lUsers)
            {
                if (user.Name.Equals(sName) && user.Pwd.Equals(sPwd))
                {
                    LoggedInUser.sUser = sName;
                    LoggedInUser.dBalance = user.Bal;
                    loggedIn = true;
                    break;
                }
            }
            if (!loggedIn)
            {
                Console.WriteLine();
                Console.WriteLine("You entered an invalid password");

            }
            return loggedIn;
        }

        private static bool isUserNameValid(string sName)
        {
            bool isValid = isUserInputValid(sName);
            bool bFound = false;
            foreach (var user in lUsers)
            {
                if (user.Name.Equals(sName))
                {
                    bFound = true;
                }
            }

            if(!isValid || !bFound)
            {
                InvalidUser();
                return false;
            }
            return isValid;
        }


        public static bool LogIn()
        {
            // Prompt for user input
            Console.WriteLine();
            Console.WriteLine("Enter Username:");
            string sName = Console.ReadLine();

            Console.WriteLine("Enter Password:");
            string sPwd = Console.ReadLine();

            loggedIn = false; //Default
            if (isUserNameValid(sName) && isCorrectPassword(sName, sPwd))
            {
                User users = new User();
                foreach (var user in lUsers)
                {
                    if (user.Name.Equals(sName) && user.Pwd.Equals(sPwd))
                    {
                        LoggedInUser.sUser = sName;
                        LoggedInUser.dBalance = user.Bal;
                        loggedIn = true;
                        break;
                    }
                }
            }

            return loggedIn;
        }

        private static void InvalidUser()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You entered an invalid user");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void ShowUserRemainingBalance()
        {
            // Show remaining balance
            Console.WriteLine();
            Console.WriteLine("Your balance is " + LoggedInUser.dBalance.ToString("C"));
        }


        #region Validators
        public static bool isUserInputValid(string sInput)
        {
            //Validate if null.
            return !string.IsNullOrEmpty(sInput);
        }
        #endregion

        public static bool bCheckBalance(int iProd, int qty)
        {
            // Check if balance - quantity * price is less than 0
            if (LoggedInUser.dBalance - lProds[iProd].Price * qty < 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("You do not have enough money to buy that.");
                Console.ResetColor();
                return false;
            }
            return true;
        }

        public static bool bCheckQuantity(int iProd, int qty)
        {
            // Check if quantity is less than quantity
            if (lProds[iProd].Qty <= qty)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("Sorry, " + lProds[iProd].Name + " is out of stock");
                Console.ResetColor();
                return false;
            }
            return true;
        }

        public static void ProductSelection()
        {
            // Prompt for user input
            Console.WriteLine("Enter a number:");
            string sSelection = Console.ReadLine();
            int iSelection = Convert.ToInt32(sSelection);
            iSelection = iSelection - 1;    /* Subtract 1 from number
                                                num = num + 1 // Add 1 to number */

            if (ValidateProductSelection(iSelection))
            {
                //continue to transaction
                Console.WriteLine();
                Console.WriteLine("You want to buy: " + lProds[iSelection].Name);
                Console.WriteLine("Your balance is " + LoggedInUser.dBalance.ToString("C"));

                // Prompt for user input
                Console.WriteLine("Enter amount to purchase:");
                string sAmount = Console.ReadLine();
                int qty = Convert.ToInt32(sAmount);

                // Check if balance
                // Check if quantity is less than quantity
                // Check if quantity is greater than zero
                if (bCheckBalance(iSelection, qty) && bCheckQuantity(iSelection, qty) && qty > 0)
                {
                    // Balance = Balance - Price * Quantity
                    LoggedInUser.dBalance = LoggedInUser.dBalance - lProds[iSelection].Price * qty;

                    // Quanity = Quantity - Quantity
                    lProds[iSelection].Qty = lProds[iSelection].Qty - qty;

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You bought " + qty + " " + lProds[iSelection].Name);
                    Console.WriteLine("Your new balance is " + LoggedInUser.dBalance.ToString("C"));
                    Console.ResetColor();
                }

                else
                {
                    // Quantity is less than zero
                    Console.WriteLine(" Purchase cancelled");

                }
            }
            else
            {
                //Selection is out of range/ Or user wants to exit the console
                CancelOrder();
            }

        }

        public static void CancelOrder()
        {
            TransactionHolder();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Purchase cancelled");
            Console.ResetColor();
        }

        public static bool ValidateProductSelection(int iSelection)
        {
            if (iSelection < lProds.Count() && iSelection > 0)
            {
                //When user wants to exit;
                TransactionHolder();
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine("Purchase cancelled");
                Console.ResetColor();

                return false;
            }
            return ((lProds.Count() > iSelection) && (iSelection >= 0));
        }

        public static void TransactionHolder()
        {
            foreach (var user in lUsers)
            {
                if (user.Name == LoggedInUser.sUser)     //TODO: password is optional, We just need to find the correct user
                {
                    user.Bal = LoggedInUser.dBalance;
                    break;
                }
            }
            ExecuteTransaction();       //Save the transaction
        }

        public static void ExecuteTransaction()
        {
            // Write out new balance
            string json = JsonConvert.SerializeObject(lUsers, Formatting.Indented);
            File.WriteAllText(@"Data/Users.json", json);

            // Write out new quantities
            string json2 = JsonConvert.SerializeObject(lProds, Formatting.Indented);
            File.WriteAllText(@"Data/Products.json", json2);


            // Prevent console from closing
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
            return;
        }


        public static void Start(List<User> usrs, List<Product> prods)
        {
            lUsers = usrs;
            lProds = prods;

            WelcomeMessage();

            if (LogIn())
            {
                LogInSuccessfulMessage(LoggedInUser.sUser);
                ShowUserRemainingBalance();

                DisplayProducts();
                ProductSelection();
            }

            //// Prevent console from closing
            //Console.WriteLine();
            //Console.WriteLine("Press Enter key to exit");
            //Console.ReadLine();

        }

        private static void DisplayProducts()
        {
            // Prompt for user input
            Console.WriteLine();
            Console.WriteLine("What would you like to buy?");

            int iProdNumber = 1;
            foreach (var prod in lProds)
            {
                Console.WriteLine(iProdNumber + ": " + prod.Name + " (" + prod.Price.ToString("C") + ")");
                iProdNumber++;
            }

            Console.WriteLine(iProdNumber + ": Exit");
        }







    }
}
