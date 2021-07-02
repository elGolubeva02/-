using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Джонсон
{
    public class Jonson
    {
        public int[,] mas;
        public int maxProst;
        List<MassDan> vhod = new List<MassDan>(); //считывание из файла с лист

        struct MassDan
        {
            public int pervSt;
            public int vtorST;
            public override string ToString()
            {
                return pervSt + " " + vtorST;
            }
        }
        public void textshit()
        {
            try
            {
                int[] ms1 = { };
                using (StreamReader sr = new StreamReader("Ввод.csv"))
                    while (sr.EndOfStream != true)
                    {
                        string[] str = sr.ReadLine().Split(';');
                        vhod.Add(new MassDan() { pervSt = Convert.ToInt32(str[0]), vtorST = Convert.ToInt32(str[1]) });
                    }
            }
            catch
            {
                Console.WriteLine("Измените данные в файле");
            }
        }
        public void Reshenie()
        {
            textshit();
            int p = 0;
            Stack S = new Stack();
            Queue Q = new Queue();
            int c = 0;
            int up = vhod.Count;
            int c1 = vhod.Count - 1;
            int[,] mas = new int[10, 2];
            while (vhod.Count > 0)
            {
                int min = int.MaxValue;
                int max = int.MinValue;
                int n = 0;
                bool flag = true;
                foreach (MassDan u in vhod) //нахождение минимального значение
                {
                    if (u.pervSt < min)
                    {
                        min = u.pervSt;
                        max = u.vtorST;
                        p = n;
                        flag = true;
                    }
                    if (u.vtorST < min)
                    {
                        min = u.pervSt;
                        max = u.vtorST;
                        p = n;
                        flag = false;
                    }
                    n++;
                }
                if (flag == false) //добавление строки в массив и стек, если мин во втором столбце
                {
                    mas[c1, 0] = min;
                    mas[c1, 1] = max;
                    S.Push(vhod[p]);
                    c1--;
                }
                else //добавление строки в массив  очередь, если мин в первом столбце
                {
                    mas[c, 0] = min;
                    mas[c, 1] = max;
                    Q.Enqueue(vhod[p]);
                    c++;
                }
                vhod.RemoveAt(p);
            }
            for (int i = 0; i < up; i++)
            {
                Debug.WriteLine(mas[i, 0] + " " + mas[i, 1]);
            }

            int[] vremia = new int[c1 + 1];
            int forVRE = 0;
            vremia[forVRE] = mas[0, 0];
            int sum = 0;
            for (int o = 1; o < c1 + 1; o++) //подсчет времени простоя
            {
                for (int j = 0; j < o; j++)
                {
                    sum += mas[j, 0];
                }
                for (int j = 0; j < o - 1; j++)
                {
                    sum -= mas[j, 1];
                }
                forVRE++;
                vremia[forVRE] = sum;
                sum = 0;
            }
            maxProst = vremia.Max();
            Console.WriteLine(maxProst);
            using (StreamWriter sw = new StreamWriter("Вывод.csv")) //запись в файл
            {
                while (Q.Count > 0)
                {
                    sw.WriteLine(Q.Dequeue());
                }
                while (S.Count > 0)
                {
                    sw.WriteLine(S.Pop());
                }
                sw.Write("Время простоя:" + maxProst);
                Debug.WriteLine(maxProst);
            }

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.Listeners.Add(new TextWriterTraceListener(File.CreateText("Промежуточные.txt")));
            Debug.AutoFlush = true;
            Jonson d = new Jonson();
            d.Reshenie();
            Console.ReadKey();
        }
    }
}
