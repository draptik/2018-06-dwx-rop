namespace RopDemo.ResultSimple
{
    public class ResultSimple
    {
        public bool Success { get; }
        public string Error { get; }

        protected ResultSimple(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public static ResultSimple<T> Fail<T>(string message)
        {
            return new ResultSimple<T>(false, message, default (T));
        }

        public static ResultSimple<T> Ok<T>(T value)
        {
            return new ResultSimple<T>(true, null, value);
        }
    }

    public class ResultSimple<T> : ResultSimple
    {
        public T Value { get; }
        public bool IsFailure => !Success;

        protected internal ResultSimple(bool success, string error, T value)
            : base(success, error)
        {
            Value = value;
        }
    }
}