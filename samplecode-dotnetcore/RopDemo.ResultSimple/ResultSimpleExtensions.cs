using System;

namespace RopDemo.ResultSimple
{
    public static class ResultSimpleExtensions
    {
        public static ResultSimple<K> OnSuccess<T, K>(
            this ResultSimple<T> result, Func<T, ResultSimple<K>> func)
        {
            if (result.IsFailure)
            {
                return ResultSimple.Fail<K>(result.Error);
            }

            return func(result.Value);
        }

        public static ResultSimple<T> OnFailure<T>(
            this ResultSimple<T> result, Action<string> action)
        {
            if (result.IsFailure)
            {
                action(result.Error);
            }
                
            return result;
        }

        public static K OnBoth<T, K>(
            this ResultSimple<T> result, Func<ResultSimple<T>, K> func)
        {
            return func(result);
        }
    }
}