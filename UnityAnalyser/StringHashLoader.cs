using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UnityAnalyzer
{
    public class StringHashLoader
    {
        static Dictionary<uint,string> stringHash=new Dictionary<uint, string>();
        public static void LoadHash()
        {
            string dir=AppDomain.CurrentDomain.BaseDirectory;
            string[] files=System.IO.Directory.GetFiles(dir);
            foreach(string file in files)
            {
                if(file.EndsWith(".hash"))
                {
                    string hash=File.ReadAllText(file);
                    string[] lines = hash.Split('\n');
                    foreach(string line in lines)
                    {
                        string linecontent = line.Trim();
                        if (string.IsNullOrEmpty(linecontent)) continue;
                        string[] fields = linecontent.Split(':');
                        uint key = uint.Parse(fields[0].Substring(2), 
                            System.Globalization.NumberStyles.HexNumber);
                        if(!stringHash.ContainsKey(key))
                        {
                            stringHash.Add(key, fields[1]);
                        }
                    }
                }
            }
        }

        public static string GetStringByHash(uint hash)
        {
            if(stringHash.ContainsKey(hash))
            {
                return stringHash[hash];
            }
            return "[PATH:NULL]";
        }
    }
}
