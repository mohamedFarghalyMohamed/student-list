using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentsList.UI;
using StudentsList.ProcessingTools;
using StudentsList.IO;
using StudentsList;

namespace dev275x.studentlist
{
    class Program
    {
        static void Main(string[] args)
        {
            App app = new App();
            app.Setup();
            app.Start(args);
        }
    }
}