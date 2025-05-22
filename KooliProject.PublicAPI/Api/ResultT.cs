namespace KooliProjekt.PublicAPI
{
    public class Result<T> : Result
    {
        public T Value { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
    }
}