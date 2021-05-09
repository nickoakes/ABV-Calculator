using System;

namespace ABV_Calculator
{
    public static class Process
    {
        public static void Information()
        {
            Console.WriteLine("Use this calculator to work out the approximate A.B.V. of home-brewed drinks.");
            Console.WriteLine();
            Console.WriteLine(@"Assumptions:
                - All sugars are fermented completely to ethanol
                - The default sugar content of red grape juice is 160g/1000ml
                - The default sugar content of clear honey is 81g/100g
                - The default sugar content of apple juice is 100g/1000ml
                - A standard 40-pint beer kit contains 517.7g of sugar");
            Console.WriteLine();
        }

        public static void Start()
        {
            Console.WriteLine();
            Console.WriteLine("What have you made (beer, wine, cider or mead)?");
            Console.WriteLine();
        }

        public static void HandleInput()
        {
            string drink = Console.ReadLine().ToLower();

            switch (drink)
            {
                case "beer":
                    Drink beer = new Drink("beer", Constants.BeerKit, false);
                    beer.DrinkPath();
                    break;
                case "wine":
                    Drink wine = new Drink("wine", Constants.GrapeJuicePerLitre, true);
                    wine.DrinkPath();
                    break;
                case "cider":
                    Drink cider = new Drink("cider", Constants.AppleJuicePerLitre, true);
                    cider.DrinkPath();
                    break;
                case "mead":
                    Drink mead = new Drink("mead", Constants.HoneyPer100g, false);
                    mead.DrinkPath();
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Drink not recognised");
                    Console.WriteLine();
                    break;
            }
        }
    }
}
