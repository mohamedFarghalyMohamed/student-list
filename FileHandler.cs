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
}