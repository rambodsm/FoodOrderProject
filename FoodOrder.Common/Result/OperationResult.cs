﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrder.Common.Result
{
    public class OperationResult<TResult>
    {
        public TResult Result { get; private set; }

        public bool Success { get; private set; }
        public string? ErrorMessage { get; private set; }
        public Exception? Exception { get; private set; }

        public static OperationResult<TResult> BuildSuccessResult(TResult result)
        {
            return new OperationResult<TResult> { Success = true, Result = result };

        }

        public static OperationResult<TResult> BuildFailure(string errorMessage)
        {
            return new OperationResult<TResult> { Success = false, ErrorMessage = errorMessage };

        }
        public static OperationResult<TResult> BuildFailure(Exception ex)
        {
            return new OperationResult<TResult> { Success = false, Exception = ex };
        }

        public static OperationResult<TResult> BuildFailure(Exception ex, string errorMessage)
        {
            return new OperationResult<TResult> { Success = false, Exception = ex, ErrorMessage = errorMessage };
        }

    }
    public class OperationResult
    {
        public bool Success { get; private set; }
        public string? ErrorMessage { get; private set; }
        public Exception? Exception { get; private set; }

        public static OperationResult BuildSuccessResult()
        {
            return new OperationResult { Success = true};

        }

        public static OperationResult BuildFailure(string errorMessage)
        {
            return new OperationResult { Success = false, ErrorMessage = errorMessage };

        }
        public static OperationResult BuildFailure(Exception ex)
        {
            return new OperationResult { Success = false, Exception = ex };
        }

        public static OperationResult BuildFailure(Exception ex, string errorMessage)
        {
            return new OperationResult { Success = false, Exception = ex, ErrorMessage = errorMessage };
        }

    }
}
