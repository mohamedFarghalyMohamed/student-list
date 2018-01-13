using System.Collections.Generic;
using System;

namespace StudentsList.UI
{
    public class UIRouter
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
            else 
                throw new ArgumentException("the Arugemnt is not supported and is not prefixed with a supported operator");
        }
    }

}