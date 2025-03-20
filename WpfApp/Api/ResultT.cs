using WpfApp.Api;

namespace WpfApp.Api

{

    public class Result<T> : Result

    {

        public T Value { get; set; }

    }

}

