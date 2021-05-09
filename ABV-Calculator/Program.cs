using System;

namespace ABV_Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool start = true;

            Process.Information();

            while (start)
            {
                Process.Start();
                Process.HandleInput();

                Console.WriteLine("Would you like to perform another calculation (enter y or n)?");

                start = Console.ReadLine().ToLower() == "y";
            }
        }
    }
}
