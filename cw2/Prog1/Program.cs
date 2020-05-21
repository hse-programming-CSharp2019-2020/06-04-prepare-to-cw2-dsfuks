using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.OleDb;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using Lib;
using System.Runtime.Serialization.Formatters;
using System.Xml.Serialization;

namespace Prog1
{
    internal class Program
    {
        private static Random gen = new Random();

        private static string genStr()
        {

            string name = "";
            name += (char) gen.Next('A', 'Z' + 1);
            for (int i = 0; i < gen.Next(1, 8); i++)
            {
                name += (char) gen.Next('a', 'z' + 1);
            }

            return name;
        }

        private static int[] genHouses()
        {
            int[] houses = new int[gen.Next(2,10)];
            for (int i = 0; i < houses.Length; i++)
            {
                houses[i] = gen.Next(1, 101);
            }

            return houses;
        }
        private static void Main(string[] args)
        {
            // Отвечает за корректность файла data.txt
            bool wrong = false;
            FileStream a = null;
            StreamReader reader = null;
            try
            {
                a = File.Open("data.txt", FileMode.Open);
                reader = new StreamReader(a);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
                wrong = true;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                wrong = true;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
                wrong = true;
            }
           
            
            int n;
            do
            {
                Console.Write("Введите количество улиц: ");
            } while (!int.TryParse(Console.ReadLine(), out n) && n<1);
            List<List<string>> st = new List<List<string>>();

            // Обработка файла data.txt
            while (!reader.EndOfStream && !wrong)
            {
                string str = reader.ReadLine();
                string[] strs;
                strs = str.Split(' ');
                // Требований к названию улицы нет
                st.Add(new List<string>());
                if (strs.Length <= 1)
                {
                    wrong = true;
                    break;
                }
                st[st.Count - 1].Add(strs[0]);
                for (int i = 1; i < strs.Length; i++)
                {
                    bool nozero = false;
                    int temp;
                    int.TryParse(strs[i], out temp);
                    if (temp > 100 || temp < 1)
                    {
                        wrong = true;
                        break;
                    }
                    foreach (var p in strs[i])
                    {
                        if (p == '0' && !nozero || p < '0' || p > '9')
                        {
                            wrong = true;
                            break;
                        }

                        if (p > '0' && p <= '9') nozero = true;
                        

                    }

                    if (wrong) break;
                    st[st.Count - 1].Add(strs[i]);
                }

                if (wrong) break;
            }
            
            

            Street[] streetsArray;
            if (wrong)
            {
                Console.WriteLine("Некорректные данные в файле");
                streetsArray = new Street[n];
                for (int i = 0; i < n; i++)
                {
                    streetsArray[i] = new Street() {Name = genStr(), Houses = genHouses()};
                }
            }
            else
            {
                // Заносим все данные из списка в соответствующие массивы
                string[] names = new string[Math.Min(n, st.Count)];
                int[][] houses = new int[names.Length][];

                for (int i = 0; i < names.Length; i++)
                {
                    names[i] = st[i][0];
                    houses[i] = new int[st[i].Count - 1];
                    for (int j = 1; j < st[i].Count; j++)
                    {
                        int.TryParse(st[i][j], out houses[i][j - 1]);
                    }
                }
                // заполняем массив streetsArray
                streetsArray = new Street[names.Length];
                for (int i = 0; i < names.Length; i++)
                {
                    streetsArray[i] = new Street() {Name = names[i], Houses = houses[i]};
                }

                
            }

            // Выводим информацию об улицах
            for (int i = 0; i < streetsArray.Length; i++)
            {
                Console.WriteLine(streetsArray[i].ToString());
            }
            reader.Close();

            // Начинаем сериализацию
            XmlSerializer formater = new XmlSerializer(typeof(Street[]));
            try
            {
                FileStream fs = new FileStream("out.ser", FileMode.Create);
                formater.Serialize(fs, streetsArray);
                fs.Close();
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}