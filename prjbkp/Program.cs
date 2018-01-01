using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjbkp
{
    class PrjBkp
    {
        //static string PrjNum;

        static string PrjsPath = @"Z:\HomecareRND\Users\Roger\prjs";
//        static string PrjsPath = @"C:\prjs";

        static void Main(string[] args)
        {
            //Args
            if (args.Length == 0)
            {
                Console.WriteLine("Prj Num is missing");
                Console.ReadLine();
                return;
            }

            string PrjNum = args[0];

            //get prj dir name
            var prjDirInfo = getPrjDirName(PrjNum);
            if (prjDirInfo.Item2)
            {
                Console.WriteLine("Prj Dir error: found 0 or > 1");
                Console.ReadLine();
                return;
            }

            string PrjDirName = prjDirInfo.Item1;
            PrjDirName = Path.Combine(PrjsPath, PrjDirName + @"\bkps");

            //make bkp dir
            string BkpDirName = getBkpDirName();
            BkpDirName = Path.Combine(PrjDirName, BkpDirName);
            Directory.CreateDirectory(BkpDirName);

            //get file list
            var files = File.ReadAllLines(PrjDirName + @"\FileList.txt");
            int copied = 0, errs = 0;
            foreach (var f in files)
            {
                Console.WriteLine(f);
                string dest = Path.Combine(BkpDirName, Path.GetFileName(f));
                try
                {
                    File.Copy(f, dest);
                    copied++;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Error copying: {f} to {dest}");
                    errs++;
                }
            }

            if (errs == 0)
            {
                Console.WriteLine($"success, copied {copied} files");
            }
            else
            {
                Console.WriteLine($"ERRORS, num of errors: {errs},  copied {copied} files");
            }
            Console.ReadLine();
        }

        static Tuple<string, bool> getPrjDirName(string PrjNum)
        {
            int found = 0;
            string PrjDirName = "";
            var dirs = Directory.GetDirectories(PrjsPath);
            foreach (var dir in dirs) {
                var dirName = new DirectoryInfo(dir).Name;
                if (dirName.StartsWith(PrjNum))
                {
                    found++;
                    PrjDirName = dirName;
                }
            }

            if (found == 1)
            {
                return Tuple.Create(PrjDirName, false);
            }

            return Tuple.Create("", true);
        }

        static string getBkpDirName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
