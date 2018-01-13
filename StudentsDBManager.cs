using StudentsList.IO;
using StudentsList.ProcessingTools;
using System.Collections.Generic;
using System;

namespace StudentsList
{
    public class StudentsDBManager
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

}