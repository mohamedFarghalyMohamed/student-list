using System.IO;

namespace StudentsList.IO
{
      public class FileHandler
    {
        private string filePath;
        
        public FileHandler(string filePath)
        {
            this.filePath = filePath;
        }

        public string Read()
        {
            using(var fileStream = new FileStream(filePath, FileMode.Open))
                using (var streamReader = new StreamReader(fileStream))
                {
                    return streamReader.ReadToEnd();
                }
        }

        public void Overwrite(string content)
        {
            using(var fileStream = new FileStream(filePath, FileMode.Open))
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    streamWriter.WriteLine(content);
                }
        }
    }
}