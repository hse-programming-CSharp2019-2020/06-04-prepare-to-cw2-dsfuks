using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Lib;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Prog2
{
    class Program
    {
        static void Main(string[] args)
        {
            Street[] streetsArray = null;
            try
            {
                FileStream fs = new FileStream("../../../Prog1/bin/debug/out.ser", FileMode.Open);
                XmlSerializer formater = new XmlSerializer(typeof(Street[]));
                streetsArray = (Street[]) formater.Deserialize(fs);
                fs.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                var magicStreets = from p in streetsArray
                    where +p && ~p % 2 == 1
                    select p;
                foreach (var p in magicStreets)
                {
                    Console.WriteLine(p.ToString());
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
