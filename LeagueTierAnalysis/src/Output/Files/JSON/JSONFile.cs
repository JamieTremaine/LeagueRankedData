using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace LeagueTierLevels.Files
{
    class JSONFile : File
    {
        private string m_Path;

        public JSONFile(string name)
        {
            this.m_Path = CreateNewFile(name);
        }

        public void AddToFile(Division division)
        {
            string json = JsonConvert.SerializeObject(division);

            StreamWriter file = default;

            try
            {
                using (file = new StreamWriter(m_Path, true))
                {
                    file.WriteLine(json);
                    file.Close();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                file.Close();
            }
        }

        public void AddToFile(Tier tier)
        {
            string json = JsonConvert.SerializeObject(tier, Formatting.Indented);
            StreamWriter file = default;
            try
            {
                using (file = new StreamWriter(m_Path, true))
                {
                    file.WriteLine(json);
                }
            }
            catch
            {
                file.Close();
                AddToFile(tier);
            }
            finally
            {
                file.Close();
            }
        }

        private string CreateNewFile(string name)
        {
            string workingDir = Directory.GetCurrentDirectory();

            string fileName = DateTime.UtcNow.ToString();
            fileName = fileName.Replace('/', '_');
            fileName = fileName.Replace(' ', '_');
            fileName = fileName.Replace(':', '_');
            fileName = string.Format("{0}_{1}", name, fileName);          

            string fullPath = Path.Combine(workingDir, fileName);
            fullPath = fullPath + ".json";
            return fullPath;                             
        } 

    } 
}
