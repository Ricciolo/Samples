using System;

namespace Industria4.DomainModel
{
    /// <summary>
    ///     Represents the result of a validation
    /// </summary>
    public class ValidationResult
    {
        public ValidationResult(int errorCode, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage)) throw new ArgumentException("message", nameof(errorMessage));

            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        ///     Gets the code for the validation result
        /// </summary>
        public int ErrorCode { get; }

        /// <summary>
        ///     Gets the message for the validation result
        /// </summary>
        public string ErrorMessage { get; }
    }
}