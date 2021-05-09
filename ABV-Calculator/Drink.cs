using System;
using System.IO;
using System.Text;

namespace ABV_Calculator
{
    public class Drink
    {
        public string Name { get; set; }
        public double BaseSugar { get; set; }
        public bool BaseSugarPerLitre { get; set; }
        public bool DefaultSugar { get; set; }
        public double TotalBaseSugar { get; set; }
        public double AdditionalSugar { get; set; }
        public double TotalVolume { get; set; }
        public double ABV { get; set; }

        public Drink(string name, double baseSugar, bool baseSugarPerLitre)
        {
            Name = name;
            BaseSugar = baseSugar;
            BaseSugarPerLitre = baseSugarPerLitre;
        }

        public double GetNewBaseSugarMass()
        {
            double baseSugarMass = 0;

            bool success = false;

            Console.WriteLine();
            Console.WriteLine("Please enter a new base sugar mass, in grams (no units necessary):");
            Console.WriteLine();

            while (!success)
            {
                bool validMass = double.TryParse(Console.ReadLine(), out baseSugarMass);

                success = validMass;
            }

            return baseSugarMass;
        }

        public void GetHoneySugarMass()
        {
            double honeyMass = 0;

            bool validMass = false;

            Console.WriteLine();
            Console.WriteLine("How much honey did you add, in grams (no units necessary)");
            Console.WriteLine();

            while (!validMass)
            {
                validMass = double.TryParse(Console.ReadLine(), out honeyMass);
            }

            BaseSugar = 0.81 * honeyMass;
        }

        public void GetBaseSugarMass()
        {
            Console.WriteLine();
            Console.WriteLine($"Would you like to use the default amount of sugar for {Name} (enter y or n)? **RECOMMENDED**");
            Console.WriteLine();

            bool notDefault = Console.ReadLine().ToLower() == "n" ? true : false;

            if (notDefault)
            {
                BaseSugar = GetNewBaseSugarMass();
                BaseSugarPerLitre = false;
                DefaultSugar = false;
            } 
            else if(string.Equals(Name, "mead"))
            {
                GetHoneySugarMass();
                DefaultSugar = true;
            }
            else
            {
                DefaultSugar = true;
            }
        }

        public void GetAdditionalSugarMass()
        {
            bool success = false;

            int sugarMass = 0;

            while (!success)
            {
                Console.WriteLine();
                Console.WriteLine("How much additional sugar did you add, in grams (no units necessary)?");
                Console.WriteLine();

                bool validSugarMass = int.TryParse(Console.ReadLine(), out sugarMass);

                success = validSugarMass;
            }

            AdditionalSugar = sugarMass;
        }
        public void GetTotalVolume()
        {
            bool success = false;

            double totalVolume = 0;

            while (!success)
            {
                Console.WriteLine();
                Console.WriteLine("What is the total volume, in litres (no units necessary)?");
                Console.WriteLine();

                bool validVolume = double.TryParse(Console.ReadLine(), out totalVolume);

                if (validVolume)
                {
                    success = true;
                }
            }

            TotalVolume = totalVolume;
        }

        public void CalculateTotalBaseSugar()
        {
            TotalBaseSugar = BaseSugarPerLitre ? BaseSugar * TotalVolume : BaseSugar;
        }

        public double CalculateABV()
        {
            double totalSugarMass = TotalBaseSugar + AdditionalSugar;

            double sucroseMoles = totalSugarMass / Constants.SucroseMolarMass;

            TotalVolume *= 1000;

            //C12H22O11 + H2O => 2C6H12O6
            double glucoseMoles = sucroseMoles * 2;

            //C6H12O6 => 2C2H6O + 2CO2
            double ethanolMoles = glucoseMoles * 2;

            double ethanolMass = ethanolMoles * Constants.EthanolMolarMass;

            double ethanolVolume = ethanolMass * Constants.EthanolDensity;

            return Math.Round((ethanolVolume / TotalVolume) * 100, 1);
        }

        public void SaveResults()
        {
            Console.WriteLine();
            Console.WriteLine("Would you like to save this result (enter y or n)?");
            Console.WriteLine();

            bool save = Console.ReadLine().ToLower() == "y";

            if (save)
            {
                bool pathExists = false;

                string path = string.Empty;

                while (!pathExists)
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter a destination path (e.g. C:/Users/Jed/Desktop):");
                    Console.WriteLine();

                    path = Console.ReadLine();

                    if (Directory.Exists(path))
                    {
                        pathExists = true;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Path not found. Please try again...");
                        Console.WriteLine();
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Enter a name for the file:");
                Console.WriteLine();

                string fileName = Console.ReadLine();

                var builder = new StringBuilder();

                string result = builder.Append($"Date: {DateTime.Now} ")
                                       .AppendLine()
                                       .Append($"Drink: {Name} ")
                                       .AppendLine()
                                       .Append($"Default sugar mass used: {DefaultSugar} ")
                                       .AppendLine()
                                       .Append($"Total base sugar: {TotalBaseSugar}g ")
                                       .AppendLine()
                                       .Append($"Additional sugar: {AdditionalSugar}g ")
                                       .AppendLine()
                                       .Append($"Total volume: {TotalVolume / 1000} litres ")
                                       .AppendLine()
                                       .Append($"A.B.V.: approximately {ABV}%")
                                       .ToString();

                File.WriteAllTextAsync($"{path}/{fileName}.txt", result);

                Console.WriteLine();
                Console.WriteLine($"File {fileName}.txt saved to {path}");
                Console.WriteLine();
            }
        }
        public void DrinkPath()
        {
            GetBaseSugarMass();
            GetAdditionalSugarMass();
            GetTotalVolume();
            CalculateTotalBaseSugar();

            double aBV = CalculateABV();

            ABV = aBV;

            Console.WriteLine();
            Console.WriteLine($"Your {Name} is approximately {aBV}% A.B.V.");
            Console.WriteLine();

            SaveResults();
        }
    }
}
