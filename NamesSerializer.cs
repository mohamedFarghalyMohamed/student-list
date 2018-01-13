using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

namespace StudentsList.ProcessingTools
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

}