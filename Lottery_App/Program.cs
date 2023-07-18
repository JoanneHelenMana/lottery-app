using System;
using System.Threading;


namespace Lottery_App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                int totalValues = 5;        // Total number of values to be drawn by lotto/picked by user - values allowed => 2
                int lowestValue = 1;        // Lowest value for predefinied range of lotto numbers - values not allowed: '0', '-1'
                int highestValue = 100;     // Highest value for predefinied range of lotto numbers - values not allowed: '0', '-1'

                int[] userNumbersArray = new int[totalValues];
                int[] randomNumbersArray = new int[totalValues];
                int[] matchedNumbersArray = new int[totalValues];


                // gameplay interaction
                WelcomeLines(totalValues, lowestValue, highestValue);
                Console.WriteLine();
                GetUserNumbers(lowestValue, highestValue, userNumbersArray);
                PrintUserNumbers(userNumbersArray);
                GenerateRandomNumbers(randomNumbersArray, lowestValue, highestValue);
                Console.WriteLine();
                PrintRandomNumbers(randomNumbersArray);
                // LinearSearchResult(randomNumbersArray, userNumbersArray, matchedNumbersArray);
                BinarySearchResult(randomNumbersArray, userNumbersArray, matchedNumbersArray);
                Console.WriteLine();
                OutputResult(matchedNumbersArray, totalValues);
                Console.WriteLine();
                Console.WriteLine("Would you like to play again? (YES or NO):");
                string playAgain = Console.ReadLine();
                playAgain = playAgain.ToLower();

                if (playAgain == "y" | playAgain == "yes")
                {
                    Console.WriteLine();
                    continue;
                }

                else
                {
                    Console.WriteLine("OK, this window will close in 3 seconds...");
                    Thread.Sleep(3000);
                    break;
                }
            }


            void WelcomeLines(int total, int lowest, int highest)
            {
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("Welcome to the lottery app!");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                if (total == 1)
                    Console.WriteLine($"Choose {total} number from {lowest} to {highest}. " + $"You win the jackpot if your number is drawn. ");
                else
                    Console.WriteLine($"Choose {total} numbers from {lowest} to {highest}. " + $"You win the jackpot if your {total} numbers are drawn. ");
                
                Console.WriteLine();
                Console.WriteLine($"Good luck!");
            }


            void GetUserNumbers(int lowest, int highest, int[] userArray)
            {
                int value;
                string[] ordinalNumbers = { "first", "second", "third", "fourth", "fifth" };
                string outOfRange = "The value you entered is out of range.";
                string repeated = "You already chose that number.";

                for (int i = 0; i < userArray.Length; i++)
                {
                    while (true)
                    {
                        Console.Write($"Enter your {ordinalNumbers[i]} lucky number from {lowest} to {highest}: ");
                        string inputNumber = Console.ReadLine();
                        bool intConversionSuccessful = int.TryParse(inputNumber, out value);

                        if (intConversionSuccessful == true)
                        {
                            bool isValueInArray = Array.Exists(userArray, element => element == value);
                            if (isValueInArray == true)
                            {
                                if (value == 0)     // '0' is excluded as it's a default value in the array initialisation
                                    Console.WriteLine(outOfRange);
                                else
                                {
                                    Console.WriteLine(repeated);
                                    continue;
                                }
                            }
                            else if (isValueInArray == false)
                            {
                                if (value <= highest & value >= lowest)
                                {
                                    userArray[i] = value;
                                    break;
                                }
                                Console.WriteLine(outOfRange);
                            }
                        }
                        else if (intConversionSuccessful == false)
                            Console.WriteLine(outOfRange);
                    }
                }
            }


            void PrintUserNumbers(int[] userArray)
            {
                Console.WriteLine();
                Console.WriteLine("Your chosen numbers are:");
                foreach (int number in userArray)
                    Console.WriteLine(number);
            }


            void GenerateRandomNumbers(int[] lottoArray, int lowest, int highest)
            {
                Random rnd = new Random(); 

                for (int i = 0; i < lottoArray.Length; i++)
                {
                    while (true)
                    {
                        int randomNumber = rnd.Next(lowest, highest + 1);
                        bool isNumberInArray = Array.Exists(lottoArray, ele => ele == randomNumber);

                        if (isNumberInArray == true)
                            continue;

                        else if (isNumberInArray == false)
                        {
                            lottoArray[i] = randomNumber;
                            break;
                        }
                    }
                }
            }


            void PrintRandomNumbers(int[] lottoArray)
            {
                Console.WriteLine("The lotto numbers drawn are:");
                foreach (int number in lottoArray)
                    Console.WriteLine(number);
            }


            /*
             * OPTIONAL TO BINARY SEARCH
             * 
            void LinearSearchResult(int[] lottoArray, int[] userArray, int[] matchesArray)
            {
                for (int i = 0; i < lottoArray.Length; i++)
                {
                    foreach (int number in userArray)
                    {
                        if (number == lottoArray[i])
                        {
                            matchesArray[i] = number;
                            break;
                        }
                        else
                            matchesArray[i] = -1;   // '-1' indicates a non-match
                    }
                }
            }
            *
            */


            int BinarySearch(int[] lottoArray, int value, int low, int high)
            {
                if (high >= low)
                {
                    int mid = low + (high - low) / 2;
                    if (lottoArray[mid] == value)
                        return mid;

                    if (lottoArray[mid] > value)
                        return BinarySearch(lottoArray, value, low, mid - 1);  

                    return BinarySearch(lottoArray, value, mid + 1, high);
                }
                return -1;
            }


            void BinarySearchResult(int[] lottoArray, int[] userArray, int[] matchesArray)
            {
                Array.Sort(lottoArray);
                Array.Sort(userArray);

                for (int i = 0; i < userArray.Length; i++)
                {
                    int result = BinarySearch(lottoArray, userArray[i], 0, lottoArray.Length - 1);
                    if (result != -1)
                        matchesArray[i] = userArray[i];
                    else
                        matchesArray[i] = -1;   // '-1' indicates a non-match
                }
            }


            void OutputResult(int[] matchesArray, int total)
            {
                int matches = 0;

                foreach (int number in matchesArray)
                {
                    if (number != -1)
                        matches++;
                }

                if (matches > 0)
                {
                    string matched = "Your matched number";

                    if (matches == 1)
                        Console.Write($"{matched}: ");
                    else
                        Console.Write($"{matched}s: ");

                    foreach (int number in matchesArray)
                    {
                        if (number != -1)
                            Console.Write($"{number} ");
                    }
                    Console.WriteLine();
                }

                switch (matches)
                {
                    case 0:
                        Console.WriteLine("None of your numbers were drawn :(");
                        Console.WriteLine("Better luck next time.");
                        break;
                    case 1:
                        Console.WriteLine("One of your numbers was drawn.");
                        Console.WriteLine("You won $50!");
                        break;
                    case 2:
                        Console.WriteLine("Two of your numbers were drawn.");
                        Console.WriteLine("You won $100!");
                        break;
                    case 3:
                        Console.WriteLine("Three of your numbers were drawn.");
                        Console.WriteLine("You won $200!");
                        break;
                    case 4:
                        Console.WriteLine("Four of your numbers were drawn.");
                        Console.WriteLine("You won $500!");
                        break;
                    case 5:
                        Console.WriteLine("JACKPOT! All your numbers were drawn.");
                        Console.WriteLine("You won $2000!");
                        break;
                }
            }
        }
    }
}
