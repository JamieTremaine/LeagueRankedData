using LeagueTierLevels.Files;
using System;

namespace LeagueTierLevels
{
    static class Init
    {
        public static void Start()
        {
            string iniFilePath = default;
            try
            {
                iniFilePath = IniFile.FindIniFile();
            }
            catch(Exception e)
            {
                if (e is FileNotFoundException)
                {
                    Console.WriteLine("File could not be found. Make sure it is present.");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadKey();
                    Start();
                }
                else if (e is MultipleFilesException)
                {
                    Console.WriteLine("Multiple files found. Make sure only one is present");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadKey();
                    Start();
                }
                else
                {
                    throw;
                }
            }

            IniFile.TryOpen(iniFilePath); //checks if file is present but not accessable

            IniFile file = new IniFile(iniFilePath);
            string[] fileParams = file.ReadFile();
            string apiKey = fileParams[0];
            string keyType = fileParams[1].ToLower();

            ApiInit.InitApiKey(apiKey, keyType);
        }

    }
}
