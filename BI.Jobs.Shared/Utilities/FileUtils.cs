using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Utilities
{
    public static class FileUtilities
    {
        public static string GetTempModifier()
        {
            return "_temp_";
        }

        public static string GetOverwrittenModifier()
        {
            return "_overwritten_";
        }

        public static string GetStaticFileDirectory(string hostingPath)
        {
            //string result = $"{hostingPath}\\wwwroot\\staticfiles\\";
            string result = $"{hostingPath}\\";
            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);
            return result;
            //return string.Empty;
        }

        public static string GetStaticFilePath(string fileName, string fileExtension, string hostingPath)
        { 
            string result = GetStaticFileDirectory(hostingPath) + $"{fileName}.{fileExtension}";
            return result;
        }

        public static IEnumerable<string> ToCsv<T>(this IEnumerable<T> objectlist, string separator = ",", bool header = true)
        {
            FieldInfo[] fields = typeof(T).GetFields();
            PropertyInfo[] properties = typeof(T).GetProperties();
            if (header)
            {
                yield return String.Join(separator, fields.Select(f => f.Name).Concat(properties.Select(p => p.Name)).ToArray());
            }
            foreach (var o in objectlist)
            {
                yield return string.Join(separator, fields.Select(f => (f.GetValue(o) ?? "").ToString())
                    .Concat(properties.Select(p => ((p.GetValue(o, null) != null) ? "\"" + (p.GetValue(o, null)) + "\"" : "").ToString())).ToArray());
            }
        }

        public static void WriteToFile(string fileName, string extension, string hostingPath, string content)
        {
            //var hostingPath = AppDomain.CurrentDomain.BaseDirectory;

            string file = GetStaticFilePath(fileName, extension, hostingPath);
            using (var sw = new StreamWriter(file, true))
            {
                sw.WriteLine(content);
            }
        }

        public static void WriteToFileWithPath(string filePath, string content)
        {
            //var hostingPath = AppDomain.CurrentDomain.BaseDirectory;
            //string file = GetStaticFilePath(fileName, extension);
            using (var sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(content);
            }
        }

       public static void RenameFile(string oriFile, string newFile)
        {
           if (File.Exists(oriFile))
            {
                File.Move(oriFile, newFile);
                
            }
        }

        public static string GetOverwrittenFile(string fileToReplace, int counter)
        {
            string result = fileToReplace + GetOverwrittenModifier() + counter.ToString();
            return result;
        }

        public static string CreateTempFile(string fileToReplace)
        {
            string tempFile = fileToReplace + GetTempModifier();
            int i = 0;
            string newFileName = tempFile;
            while (true)
            {
                i++;
                newFileName = tempFile + i.ToString();
                if (!File.Exists(newFileName))
                {
                    File.Create(newFileName).Close();
                    break;
                }
            }
            return newFileName;
        }
    }
}
