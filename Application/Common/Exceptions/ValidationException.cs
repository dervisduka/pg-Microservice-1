using FluentValidation.Results;

namespace Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string message)
           : base("One or more validation failures have occurred." + Environment.NewLine + message)
        {
            List<string> mesazhet = new List<string>();
            Errors = new Dictionary<string, string[]>();
            mesazhet.Add(message);
            Errors.Add("", mesazhet.ToArray());
        }
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorCode)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
