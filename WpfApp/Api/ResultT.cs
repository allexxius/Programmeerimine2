namespace WpfApp.Api
{
    public class Result<T> : Result
    {
        public T Value { get; set; }

        public static Result<T> Success(T value) => new Result<T> { Value = value };
        public static new Result<T> Fail(string error) => new Result<T> { Error = error };
    }
}