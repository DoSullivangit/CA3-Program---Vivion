using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;         
using System.ComponentModel.Design;

namespace CSVReader
{//Program to read csv file   *****************Author-Declan O' Sullivan - s00238857 - CA3 - Vivion\\
    
    //1. menu.
    //2.ship.
    //3.age.
    //4.validate data.
   
    class Program
    {
        static void Main(string[] args)
        {
            Menu();//display a menu that repeats afer user input
        }

        static void Menu()
        {
            do
            {
                int menuChoice;

                Console.WriteLine("Menu Options: ");
                Console.WriteLine("enter menu choice");
                Console.WriteLine("1. Ship Reports");
                Console.WriteLine("2. Occupation Report");
                Console.WriteLine("3. Age Report");
                Console.WriteLine("4. Exit");

                if (int.TryParse(Console.ReadLine(), out menuChoice))
                {
                    switch (menuChoice)
                    {
                        case 1:
                            Ship();  /////////SHIP REPORT//////////
                            break;
                        case 2:
                            OccupationReport();/////////OCCUPATIONREPORT/////////////
                            break;
                        case 3:
                            AgeReport();/////////AGEREPORT///////////////
                            break;
                        case 4:
                            Console.WriteLine("Press any key to exit...");
                            Console.ReadKey();
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice, please try again."); //loop to redo the input
                }

                Console.WriteLine();

                Console.Write("Do you want to continue? (Y/N) "); //allows user to exit after input without needing to press 4 and also allows program to continue
            } while (Console.ReadLine().ToUpper() == "Y");
        }
        static void Ship()
        {
            // Read contents of the CSV file applies to a string array
            string[] csvLines = System.IO.File.ReadAllLines("C:\\Users\\decla\\Downloads\\faminefile.csv");

            // Create lists with the CSV data using var
            var firstNames = new List<string>();
            var lastNames = new List<string>();
            var ship = new List<string>();
            var ageRanges= new List<string>();


            // Split each row into column data and collect each column data and place in a array 
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] rowData = csvLines[i].Split(',');
                ship.Add(rowData[8]);//ship array
                lastNames.Add(rowData[0]);
                firstNames.Add(rowData[1]);
                ageRanges.Add(rowData[2]);//age array
            }

            // Prompt user to select a ship from the list
            Console.WriteLine("Select a ship from the list below:");
            var uniqueShips = ship.Distinct().ToList(); //unique ships var allows the program to auto give the ship its own name for line 102 to
                                                        //check if a row has that ship and take its data
            for (int i = 0; i < uniqueShips.Count; i++) //unique ships takes the data from row 8 into its list in the index
            {
                Console.WriteLine($"{i + 1}. {uniqueShips[i]}");
            }
            int shipIndex = int.Parse(Console.ReadLine()) - 1; //ship index allows user to select ship and acts as a menu to collect data aswell
            
            // Write out formatted first and last names of each row with the selected ship
            Console.WriteLine($"Rows with '{uniqueShips[shipIndex]}' in Ship column:");
            for (int i = 0; i < ship.Count; i++)
            {
                if (ship[i] == uniqueShips[shipIndex]) //this puts the ship into list index
                {
                    Console.WriteLine($"First Name {firstNames[i].ToUpper()} : Last Name {lastNames[i].ToUpper()}");//formating to display the data output
                }
                
                
            }
            

            Console.ReadKey();
        }
        static void OccupationReport()
        {
            // Read the contents of the CSV file (as plain text)
            string[] csvLines = File.ReadAllLines("C:\\Users\\decla\\Downloads\\faminefile.csv");

            // Create a dictionary to store the number of passengers for each ship
            var occupations = new Dictionary<string, int>();

            // Split each row into column data and count the number of passengers for each ship
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] rowData = csvLines[i].Split(',');
                string occupation = rowData[4].Trim();

                if (!string.IsNullOrEmpty(occupation) && !occupations.ContainsKey(occupation)) //.contain so it finds each ocupatation
                {
                    occupations.Add(occupation, 1);
                }
                else if (!string.IsNullOrEmpty(occupation))
                {
                    occupations[occupation]++;  
                }
            }
            // Write out the occupation report for each ship
            Console.WriteLine("Occupation Report:");
            foreach (KeyValuePair<string, int> occupation in occupations)
            {
                Console.WriteLine($"{occupation.Key}: {occupation.Value}");
            }

            Console.ReadKey();
        }

        static void AgeReport()
        {
            // Attempt to read the contents of the CSV file (as plain text)
            string filePath = "C:\\Users\\decla\\Downloads\\faminefile.csv";
            string[] csvLines;
            try
            {
                csvLines = File.ReadAllLines(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the CSV file: {ex.Message}");
                return;
            }

            // Create a dictionary to store the number of passengers in each age category
            var ageCategories = new Dictionary<string, int>()
    {
        {"Infants (<1 year)", 0},
        {"Children (1-12)", 0},
        {"Teenage (12-19)", 0},
        {"Young Adult (20-29)", 0},
        {"Adult (30+)", 0},
        {"Older Adult (50+)", 0},
        {"Unknown", 0}
    };

            // Split each row into column data and count the number of passengers in each age category
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] rowData = csvLines[i].Split(',');

                string ageStr = rowData[2].Trim().Substring(3); // Remove "Age: " from the beginning of the string
                
                
                    if (ageStr.ToLower().Contains("months"))
                    {
                        ageCategories["Infants (<1 year)"]++; //split up the infants cat so its easy to fix
                    }
                
                else
                {
                    int age;
                    bool isValidAge = int.TryParse(ageStr, out age);
                    if (isValidAge)      //individual categories
                    {
                        if (age >= 1 && age <= 12)
                        {
                            ageCategories["Children (1-12)"]++;
                        }
                        else if (age >= 13 && age <= 19)
                        {
                            ageCategories["Teenage (12-19)"]++;
                        }
                        else if (age >= 20 && age <= 29)
                        {
                            ageCategories["Young Adult (20-29)"]++;
                        }
                        else if (age >= 30 && age <= 49)
                        {
                            ageCategories["Adult (30+)"]++;
                        }
                        else if (age >= 50)
                        {
                            ageCategories["Older Adult (50+)"]++;
                        }
                    }
                    else
                    {
                        ageCategories["Unknown"]++;
                    }
                }
            }

            // Write out the age report for each age category
            Console.WriteLine("Age Report:");
            foreach (KeyValuePair<string, int> ageCategory in ageCategories)
            {
                Console.WriteLine($"{ageCategory.Key}: {ageCategory.Value}"); //display the data using key value pair A key-value pair is a set of data that represents two associated groups through a key and a value.
            }

            Console.ReadKey();
        }
    }
}
