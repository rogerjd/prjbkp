﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjbkp
{
    class PrjBkp
    {
        static string PrjNum;

        static string PrjsPath = @"Z:\HomecareRND\Users\Roger\prjs";
//        static string PrjsPath = @"C:\prjs";

        static void Main(string[] args)
        {
            //Args
            if (args.Length == 0)
            {
                Console.WriteLine("Prj Name is missing");
                Console.ReadLine();
                return;
            }

            PrjNum = args[0];

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
            foreach (var f in files)
            {
                Console.WriteLine(f);
            }
            //copy, foreach file
        }

        static Tuple<string, bool> getPrjDirName(string tktNum)
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
            return DateTime.Now.ToString("yyyymmddhhmm");
        }
    }
}