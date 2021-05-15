using System;

namespace Checkers
{
    public class Checker<TResult>
    {
        public string Message { get; init; }

        public bool IsValid { get; init; }

        public TResult Result { get; init; }
    }
}
