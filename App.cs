using StudentsList;
using StudentsList.IO;
using StudentsList.UI;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentsList.ProcessingTools;

namespace StudentsList
{
    public class App
    {
        private readonly string studentsFilePath = "students.txt";
        private StudentsDBManager manager;
        private UIRouter router;

        Printer consolePrinter;

        public void Setup()
        {
            consolePrinter = new Printer(Console.Out);
            FileHandler fileHandler = new FileHandler(studentsFilePath);
            NamesSerializer serializer = new NamesSerializer();
            
            manager = new StudentsDBManager(fileHandler, serializer);
            router = new UIRouter();

            router.On(AppArguemnts.ListAllStudents, arg =>
            {
                ListAllStudents();
            });

            router.On(AppArguemnts.GetRandomStudent, arg =>
            {
                ListRandomStudent();
            });

            router.On(AppArguemnts.AddStudent, studentName =>
            {
                AddStudent(studentName);
            });

            router.On(AppArguemnts.StudentExists, studentName =>
            {
                CheckStudentExistance(studentName);
            });

            router.On(AppArguemnts.GetStudentsCount, arg =>
            {
                PrintStudentsCount();
            });
        }

        public void Start(string[] args)
        {
            string arguemnt = args[0];
            this.router.Route(arguemnt);
        }

        private void PrintStudentsCount()
        {
            consolePrinter.PrintLine("Working on it...");
            int count = manager.GetStudntsCount();
            consolePrinter.PrintLine(count.ToString());
        }

        private void CheckStudentExistance(string arg)
        {
            string studentName = arg.Substring(1);
            bool isFound = manager.Contains(studentName);
            consolePrinter.PrintLine(isFound ? "We found it!" : "No such student exists");
        }

        private void AddStudent(string arg)
        {
            string studentName = arg.Substring(1);
            manager.AddStudentName(studentName);
            consolePrinter.PrintLine("Done");
        }

        private void ListRandomStudent()
        {
            consolePrinter.PrintLine("Loading...");
            string randomName = manager.GetRandomStudent();
            consolePrinter.PrintLine(randomName);
            consolePrinter.PrintLine("Done");
        }

        private void ListAllStudents()
        {
            consolePrinter.PrintLine("Loading");
            List<string> names = manager.GetStudentsList();
            consolePrinter.PrintLine(names);
            consolePrinter.PrintLine("Data loaded");
        }
    }
}