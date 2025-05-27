namespace KooliProjekt.BlazorApp
{
    public class Result
    {
        public Dictionary<string, List<string>> Errors { get; set; } = new();

        public bool HasErrors => Errors.Any();

        public void AddError(string propertyName, string errorMessage)
        {
            if (!Errors.ContainsKey(propertyName))
            {
                Errors[propertyName] = new List<string>();
            }

            Errors[propertyName].Add(errorMessage);
        }
    }
}
