using System.Collections.Generic;

namespace KooliProjekt.PublicAPI
{
    public class Result
    {
        public Dictionary<string, List<string>> Errors { get; set; }

        public Result()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        // Parandatud nimi: HasErrors (varem HasError)
        public bool HasErrors
        {
            get
            {
                return Errors.Count > 0;
            }
        }

        public bool HasError { get; set; }
        public string Error { get; set; }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!Errors.ContainsKey(propertyName))
            {
                Errors.Add(propertyName, new List<string>());
            }

            Errors[propertyName].Add(errorMessage);
        }
    }
}