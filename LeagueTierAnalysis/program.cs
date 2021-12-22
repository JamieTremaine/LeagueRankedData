using System;
using System.Threading.Tasks;
using LeagueTierLevels.Files;

using System.IO;

namespace LeagueTierLevels
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Init.Start();
            }
            catch (Exception)
            {
                Console.WriteLine("Could not init. Closing...");

                Console.ReadKey();
                return;
            }

            Api Api = new Api(ApiInit.API_TYPE);

            (Tier tier, bool Success) = await Api.GetTierOfPlayers(Regions.EUW, TierNames.IR0N);

            if (!Success)
            {
                tier = null;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("FATAL ERROR");
                Console.WriteLine("CLOSING");
                Console.ResetColor();

                End();
            }

            double averageLevel = tier.GetAverageLevel();


            JSONFile file = new JSONFile(tier.Name);

            file.AddToFile(tier);
            Console.WriteLine("JSON file created");

            Console.WriteLine("The average level of {0} is: {1}", tier.Name, averageLevel.ToString());


            End();
        }

        private static void End()
        {
            while (true)
            {
                Console.ReadKey();
            }
        }

    }
}