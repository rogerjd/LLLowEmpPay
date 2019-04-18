using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LLLow_EmpPay
{
    static class Program
    {
        class EmpRec
        {
            public string Name;
            public double Rate;
            public double Hours;
            public decimal Amount;
        }

        static List<EmpRec> Emps = new List<EmpRec>();


        static string FileName;
        static int Main(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine(args[0]);
                FileName = args[0];
                //FileName = Path.Join(Directory.GetCurrentDirectory(),args[0]);
            }
            else
            {
                return -1;
            }

            if (File.Exists(FileName))
            {
                var res = ProcessInputFile();
                if (res == 0)
                {
                    res = ProcessResults();
                }
                return res;

            }
            else
            {
                return -1;
            }
        }

        static int ProcessInputFile()
        {
            string rec;
            string[] flds;
            using (StreamReader sr = new StreamReader(FileName))
            {
                while ((rec = sr.ReadLine()) != null)
                {
                    Console.WriteLine(rec);
                    if (string.IsNullOrWhiteSpace(rec))
                    {
                        continue;
                    }
                    flds = rec.Split(' ');
                    if (flds.Length == 0)
                    {
                        return -1;
                    }

                    var emp = Emps.FirstOrDefault(e => e.Name == flds[1]);
                    if (emp == null)
                    {
                        emp = new EmpRec();
                        Emps.Add(emp);
                    }

                    switch (flds[0])
                    {
                        case "Employee":
                            emp.Name = flds[1];
                            break;
                        case "Pay":
                            if (double.TryParse(flds[2], out double res))
                            {
                                emp.Rate = res;
                            }
                            else
                            {
                                return -1;
                            }
                            break;
                        case "Time":
                            DateTime dt1 = DateTime.Parse(flds[2]);
                            DateTime dt2 = DateTime.Parse(flds[3]);
                            TimeSpan ts = dt2 - dt1;
                            emp.Hours += ts.TotalHours;
                            emp.Amount = (decimal)(emp.Hours * emp.Rate);
                            break;
                        default:
                            return -1;
                    }
                }
                return 0;
            }
        }

        static private int ProcessResults()
        {
            var l = Emps.OrderByDescending(e => e.Amount);
            foreach (var emp in l)
            {
                Console.WriteLine($"{emp.Name} {emp.Rate} {emp.Hours} {emp.Amount}");
            }
            return 0;
        }
    }
}
