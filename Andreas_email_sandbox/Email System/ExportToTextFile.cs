using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExportToTextFile
{

    static class Program
    {
        public static void myMain()
        {
            IList<string> data = new List<string>();
            data.Add(";test;");
            data.Add("test2");

            File.WriteAllLines("SavedLists.txt", data);

            //data.ExportToTextFile("person.txt", ';');
        }


        static void ExportToTextFile<T>(this IEnumerable<T> data, string FileName, char ColumnSeperator)
        {
            
        }
    }
}