using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dev275x.studentlist
{
    public class NamesSerializer
    {
        public string Serialize(List<string> names)
        {
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < names.Count; i++)
            {
                builder.Append(names[i]);
                if(i != names.Count - 1)
                {
                    builder.Append(',');
                    continue;
                }
              
                builder.Append('.');
                builder.Append("\r\n");
                builder.Append($"Last update: {DateTime.Now}");
            }

            return builder.ToString();
        }
        public List<string> Deserialize(string serializedContent)
        {
            int endOfNames = serializedContent.IndexOf('.');
            var namesString = serializedContent.Substring(0, endOfNames);
            return namesString.Split(',').ToList();
        }
    }

    public class FileHandler
    {
        private string filePath;
        
        public FileHandler(string filePath)
        {
            this.filePath = filePath;
        }

        public string Read()
        {
            string content;
            using(var fileStream = new FileStream(filePath, FileMode.Open))
                content = ReadFileContent(fileStream);
            return content;
        }

        public void Overwrite(string content)
        {
            using(var fileStream = new FileStream(filePath, FileMode.Open))
                OverwriteFile(fileStream, content);
        }

        private string ReadFileContent(FileStream stream)
        {
            string fileContent = string.Empty;
            using (var streamReader = new StreamReader(stream))
            {
                fileContent = streamReader.ReadToEnd();
            }
            return fileContent;
        }

        private void OverwriteFile(FileStream stream, string content)
        {
            using (var streamWriter = new StreamWriter(stream))
            {
                stream.Seek(0, SeekOrigin.Begin);
                streamWriter.WriteLine(content);
            }
        }
    }

    class StudentsDBManager
    {
        private FileHandler fileHandler;
        private NamesSerializer serializer;
        
        public StudentsDBManager(FileHandler fileHandler, NamesSerializer serializer)
        {
            this.fileHandler = fileHandler;
            this.serializer = serializer;
        }

        public List<string> GetStudentsList()
        {
            return GetNamesListFromFile();
        }

        public bool Contains(string studentName)
        {
            return GetStudentsList().Contains(studentName);
        }

        public int GetStudntsCount()
        {
            return GetStudentsList().Count;
        }

        public string GetRandomStudent()
        {
            var names = GetStudentsList();
            var random = new Random();
            var randomIndex = random.Next(0, names.Count);
            return names[randomIndex];
        }

        public void AddStudentName(string studentName)
        {
            var namesList = GetNamesListFromFile();
            if(namesList.Contains(studentName))
                throw new ArgumentException("A student with the same name already exists");
            
            namesList.Add(studentName);
            string fileContent = this.serializer.Serialize(namesList);
            this.fileHandler.Overwrite(fileContent);
        }

        private List<string> GetNamesListFromFile()
        {
            string fileContent = this.fileHandler.Read();
            return this.serializer.Deserialize(fileContent);
        }
    }

    class UIRouter
    {
        Dictionary<string, Action<string>> dictionary;

        public UIRouter()
        {
            dictionary = new Dictionary<string, Action<string>>();
        }
        public void On(string arguemnt, Action<string> action)
        {
            dictionary.Add(arguemnt, action);
        }

        public void Route(string arguemnt)
        {
            string potenialPrefix = arguemnt[0].ToString();

            if(dictionary.ContainsKey(arguemnt))
                dictionary[arguemnt].Invoke(arguemnt);
            else if(dictionary.ContainsKey(potenialPrefix))
                dictionary[potenialPrefix].Invoke(arguemnt);
            else throw new ArgumentException("the Arugemnt is not supported and is not prefixed with a supported operator");
        }
    }

    class Printer
    {
        private TextWriter writer;
        public Printer(TextWriter writer)
        {
            this.writer = writer;
        }
        public void Print(string text)
        {
            this.writer.Write(text);
        }

        public void PrintLine()
        {
            PrintLine(string.Empty);
        }

        public void PrintLine(string text)
        {
            this.writer.WriteLine(text);
        }

        public void Print(List<string> list)
        {
            foreach(var item in list)
                Print(item);
        }

        public void PrintLine(List<string> list)
        {
            foreach(var item in list)
                PrintLine(item);
        }
    }

    abstract class AppArguemnts
    {
        public static readonly string ListAllStudents = "a";
        public static readonly string AddStudent = "+";

        public static readonly string GetRandomStudent = "r";

        public static readonly string GetStudentsCount = "c";

        public static readonly string StudentExists = "?";
    }

    class Program
    {
        private static readonly string studentsFilePath = "students.txt";
        static void Main(string[] args)
        {
            Printer consolePrinter = new Printer(Console.Out);
            UIRouter router = new UIRouter();
            FileHandler fileHandler = new FileHandler(studentsFilePath);
            NamesSerializer serializer = new NamesSerializer();
            StudentsDBManager manager = new StudentsDBManager(fileHandler, serializer);

            router.On(AppArguemnts.ListAllStudents, arg =>{
                consolePrinter.PrintLine("Loading");
                List<string> names = manager.GetStudentsList();
                consolePrinter.PrintLine(names);
                consolePrinter.PrintLine("Data loaded");
            });

            router.On(AppArguemnts.GetRandomStudent, arg =>{
                consolePrinter.PrintLine("Loading...");
                string randomName = manager.GetRandomStudent();
                consolePrinter.PrintLine(randomName);
                consolePrinter.PrintLine("Done");
            });

            router.On(AppArguemnts.AddStudent, arg =>{
                string studentName = arg.Substring(1);
                manager.AddStudentName(studentName);
                consolePrinter.PrintLine("Done");
            });

            router.On(AppArguemnts.StudentExists, arg =>{
                string studentName = arg.Substring(1);
                bool isFound = manager.Contains(studentName);
                consolePrinter.PrintLine(isFound ? "We found it!" : "No such student exists");
            });

            router.On(AppArguemnts.StudentExists, arg =>{
                consolePrinter.PrintLine("Working on it...");
                int count = manager.GetStudntsCount();
                consolePrinter.PrintLine(count.ToString());
            });

            string argument = args[0];
    
            router.Route(argument);
            consolePrinter.PrintLine();
        }
    }
}