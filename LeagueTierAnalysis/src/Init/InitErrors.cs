using System;

namespace LeagueTierLevels
{
    public class InitErrors
    {
        public const string ERROR_FLAG = "ERROR";
        public const string MULTIPLE_FILES = ERROR_FLAG + "MULTIPLE FILES";
        public const string INI_NOT_FOUND = ERROR_FLAG + "NO INI";
        public const string CANT_OPEN_INI = ERROR_FLAG + "CANT OPEN";


        public InitErrors(Exception e)
        {

        }



        public static void Errors(string message)
        {
            string trimmedMessage = message.Remove(0, ERROR_FLAG.Length);
            switch (trimmedMessage)
            {
                case MULTIPLE_FILES:
                    MultipleFilesError();
                    break;
                case INI_NOT_FOUND:
                    IniNotFoundError();
                    break;
                case CANT_OPEN_INI:
                    CantOpenFile();
                    break;
                default:
                    break;
            }
        }
        public static void Errors(string message, string data)
        {
            string trimmedMessage = message.Remove(0, ERROR_FLAG.Length);
            switch (trimmedMessage)
            {
                case MULTIPLE_FILES:
                    MultipleFilesError(data);
                    break;
                case INI_NOT_FOUND:
                    IniNotFoundError(data);
                    break;
                case CANT_OPEN_INI:
                    CantOpenFile(data);
                    break;
                default:
                    break;
            }
        }


        private static void MultipleFilesError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FATAL ERROR: Multiple ini files detected.\n" +
                              "Remove duplicates and try again\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
            Environment.Exit(0);
        }
        private static void MultipleFilesError(string data)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FATAL ERROR: Multiple ini files detected.\n" +
                              "Remove duplicates and try again\n");
            if (!string.IsNullOrEmpty(data))
            {
                Console.WriteLine(data);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private static void IniNotFoundError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FATAL ERROR: No INI file detected.\n" +
                              "Is file in INI format and in the program directory?\n" +
                              "Path can also be specified using command line args -path ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
            Environment.Exit(0);
        }
        private static void IniNotFoundError(string data)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FATAL ERROR: No INI file detected.\n" +
                              "Is file in INI format and in the program directory?\n" +
                              "Path can also be specified using command line args -path ");
            if (!string.IsNullOrEmpty(data))
            {
                Console.WriteLine(data);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private static void CantOpenFile()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FATAL INTERNAL ERROR: Can't open file");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
            Environment.Exit(0);
        }
        private static void CantOpenFile(string data)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FATAL ERROR: Can't open file at: " + data + "\n" +
                              "File may be inaccessable");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
            Environment.Exit(0);
        }






    }
}
