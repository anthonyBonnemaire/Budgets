using System;

namespace Budgets.Core
{
    public class Checker
    {
        public bool IsValid { get; init; }
        public string Message { get; init; }

        public static Checker CheckerValid = new Checker { IsValid = true, Message = "It's Ok" };
        
        public static Checker CreateCheckerError(string message) => new Checker { IsValid = false, Message = message };


    }

    public class Checker<TResult> : Checker
    {
        public TResult Result { get; init; }

        public static Checker<TResult> CreateCheckerValidWithValue(TResult result)
            => new Checker<TResult> { IsValid = true, Message = "It's Ok", Result = result };

        public static Checker<TResult> CreateCheckerError(string message) => new Checker<TResult> { IsValid = false, Message = message };

    }
}
