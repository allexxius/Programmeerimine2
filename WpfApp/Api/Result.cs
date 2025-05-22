namespace WpfApp.Api

{

    public class Result

    {

        public string Error { get; set; }

        public bool IsSuccess => !HasError;

        public bool HasError => !string.IsNullOrEmpty(Error);

        public static Result Success() => new Result();

        public static Result Fail(string error) => new Result { Error = error };

    }

}
