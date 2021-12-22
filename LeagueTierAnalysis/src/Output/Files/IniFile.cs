using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System;

namespace LeagueTierLevels.Files
{
    class IniFile : File
    {
        private string m_Path;
        private string m_Exe;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);


        public IniFile(string path)
        {
            this.m_Exe = Assembly.GetExecutingAssembly().GetName().Name;
            this.m_Path = path;
        }

        public string[] ReadFile()
        {
            string[] output = new string[2];

            output[0] = Read("key");
            output[1] = Read("Key_Type");
            return output;
        }

        public static bool TryOpen(string path)
        {
            try
            {
                System.IO.File.OpenRead(path);
                return true;
            }
            catch
            {
                Console.WriteLine("Could not open file at {0}. The file may be open already.", path);
                Console.WriteLine("Press any key to try again");
                Console.ReadKey();

                return TryOpen(path);
            }
        }

        public static string FindIniFile()
        {
            try
            {
                return FindFile("ini");
            }
            catch
            {
                throw;
            }
 
              
        }

        private string Read(string Key)
        {
            var retVal = new StringBuilder(255);
            string path = m_Path.Normalize();
            int i = GetPrivateProfileString(m_Exe, Key, "", retVal, retVal.Capacity, m_Path);
            return retVal.ToString();
        }

    }
}
