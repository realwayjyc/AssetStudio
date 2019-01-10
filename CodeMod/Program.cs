using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeMod
{
    class Program
    {
        static string path = @"C:\";
        static void Main(string[] args)
        {
            ParseDir(path);


        }

        static Dictionary<string, string> dictSharpBracket = new Dictionary<string, string>();
        static Dictionary<string, string> dictDollar = new Dictionary<string, string>();

        private static void ParseDir(string dir)
        {
            foreach (string d in Directory.GetDirectories(dir))
            {
                ParseDir(d);
            }
            foreach (string s in Directory.GetFiles(dir))
            {
                ParseFile(s);
            }
        }

        static void ParseFile(string file)
        {
            dictSharpBracket.Clear();
            dictDollar.Clear();

            Encoding encodingN = ASCIIEncoding.UTF8;
            if (file.EndsWith("cs") == false) return;
            Console.WriteLine("修改文件:" + file);
            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] content = new byte[fs.Length];
            int readed = fs.Read(content, 0, content.Length);
            fs.Close();

            //////////////////////////////第一遍检查<>符号
            MemoryStream ms=new MemoryStream(content);
            string lineString = encodingN.GetString(content);
            StreamReader sw = new StreamReader(ms, encodingN);
            string line = sw.ReadLine();
            while (line != null)
            {
                string[] tokens = GetAllTokens(line);
                AnalyseTokens(tokens,0);
                line = sw.ReadLine();
            }
            foreach (string key in dictSharpBracket.Keys)
            {
                lineString = lineString.Replace(key, dictSharpBracket[key]);
            }
            sw.Close();
            ms.Close();


            ////////////////////////////////第二遍检查<>符号/////////////////////////////////////////////////
            content = encodingN.GetBytes(lineString);
            ms = new MemoryStream(content);
            sw = new StreamReader(ms, encodingN);
            line = sw.ReadLine();
            while (line != null)
            {
                string[] tokens = GetAllTokens(line);
                AnalyseTokens(tokens, 1);
                line = sw.ReadLine();
            }
            foreach (string key in dictSharpBracket.Keys)
            {
                lineString = lineString.Replace(key, dictSharpBracket[key]);
            }

            sw.Close();
            ms.Close();
            ////////////////////////////////第三遍检查$符号/////////////////////////////////

            content = encodingN.GetBytes(lineString);
            ms = new MemoryStream(content);
            sw = new StreamReader(ms, encodingN);
            line = sw.ReadLine();
            while (line != null)
            {
                string[] tokens = GetAllTokens(line);
                AnalyseTokens(tokens, 2);
                line = sw.ReadLine();
            }
            foreach (string key in dictSharpBracket.Keys)
            {
                lineString = lineString.Replace(key, dictSharpBracket[key]);
            }

            sw.Close();
            ms.Close();


            //sw.Close();
            //fs.Close();

            //if (toReplaceDict.Keys.Count == 0)
            //{
            //    return;
            //}

            byte[] contentW = encodingN.GetBytes(lineString);
            fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            fs.Write(contentW, 0, contentW.Length);
            fs.Close();
        }

        /// <summary>
        /// mode=0 去除尖括号
        /// mode=1 去除$
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="mode"></param>
        private static void AnalyseTokens(string[] tokens, int mode)
        {
            foreach (string token in tokens)
            {
                string originalToken = token;
                string replacedToken = token;
                if (mode == 0)
                {
                    //去除尖括号，判断一个token中<和>的个数
                    if (replacedToken.StartsWith("<") && replacedToken.Contains('>'))
                    {
                        if (GetCharCount(replacedToken, '<') == 1 && GetCharCount(replacedToken, '>') == 1)
                        {
                            replacedToken = replacedToken.Replace("<", "B_");
                            replacedToken = replacedToken.Replace(">", "_B");
                            if (dictSharpBracket.ContainsKey(originalToken) == false)
                            {
                                dictSharpBracket.Add(originalToken, replacedToken);
                            }
                        }
                    }
                }
                else if (mode == 1)
                {
                    //去除尖括号，如果还是以<开头，则替换第一个<和>
                    if (replacedToken.StartsWith("<") && replacedToken.Contains('>'))
                    {
                        string temp = "";
                        int leftBracketCount = 0;
                        int rightBracketCount = 0;
                        foreach (char c in replacedToken.ToCharArray())
                        {
                            if (c == '<' && leftBracketCount==0)
                            {
                                leftBracketCount++;
                                temp+="B_";
                            }
                            else if (c == '>' && rightBracketCount == 0)
                            {
                                rightBracketCount++;
                                temp += "_B";
                            }
                            else
                            {
                                temp += c;
                            }
                        }
                        replacedToken = temp;

                        if (dictSharpBracket.ContainsKey(originalToken) == false)
                        {
                            dictSharpBracket.Add(originalToken, replacedToken);
                        }

                    }
                }
                else if (mode == 2)
                {
                    //去除$符号
                    if (replacedToken.Contains('$') && replacedToken.Contains("$\"")==false)
                    {
                        replacedToken = replacedToken.Replace("$", "_D_");
                        if (dictSharpBracket.ContainsKey(originalToken) == false)
                        {
                            dictSharpBracket.Add(originalToken, replacedToken);
                        }
                    }
                }
            }
        }

        private static int GetCharCount(string content, char specifyChar)
        {
            int count = 0;
            foreach (char c in content.ToCharArray())
            {
                if (c == specifyChar)
                {
                    count++;
                }
            }
            return count;
        }



        private static string[] GetAllTokens(string line)
        {
            if (line == null) return new string[0];
            line = line.Replace(";", " ");
            line = line.Replace(".", " ");
            line = line.Replace("(", " ");
            line = line.Replace(")", " ");
            string[] ret = line.Split(' ');
            List<string> fields = new List<string>();
            foreach (string r in ret)
            {
                if (r.Trim() != "" && r != null)
                {
                    fields.Add(r);
                }
            }
            return fields.ToArray();
        }

        private static string searchNextToken(string line, int index)
        {
            char c = ' ';
            string ret = "";
            while ((c = line.ToCharArray()[index]) != ' ' || ret != "")
            {
                ret += c;
                index++;
            }
            return ret;
        }
    }
}
