using System;

namespace Lib
{
    [Serializable]
    public class Street
    {
        public string Name { set; get; }
        public int[] Houses { set; get; }

        public static int operator ~(Street a)
        {
            return a.Houses.Length;
        }

        public static bool operator +(Street a)
        {
            foreach (var p in a.Houses)
            {
                var temp = p;
                while (temp != 0)
                {
                    if (temp % 10 == 7) return true;
                    temp /= 10;
                }
            }
            return false;
        }

        public override string ToString()
        {
            string ans = $"Название улицы: {Name} Дома: ";
            for (int i = 0; i < Houses.Length - 1; i++)
            {
                ans += $"{Houses[i]}, ";
            }

            ans += $"{Houses[Houses.Length - 1]}";
            return ans;
        }

        public Street()
        {

        }
    }
}