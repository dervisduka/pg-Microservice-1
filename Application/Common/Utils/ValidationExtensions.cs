using FluentValidation;
using FluentValidation.Results;

namespace Application.Common.Utils
{
    public static class ValidationExtensions
    {
        public static IEnumerable<ValidationFailure> FormatErrorCodes(this IEnumerable<ValidationFailure> failures)
        {
            var errorList = new Dictionary<string, ValidationFailure>();
            string errorCommand = "Unkown";
            var errors = failures.Where(f => f.Severity != Severity.Info);
            int elementAt = 0;
            foreach (var err in errors)
            {
                var errorType = err.ErrorCode.Replace("Validator", string.Empty);
                var errorInfos = failures.Where(f => f.Severity == Severity.Info && f.PropertyName == err.PropertyName).ToList();
                var errorInfo = errorInfos.FirstOrDefault();
                var useCustom = false;
                var errorCode = string.Empty;
                if (errorInfos != null && errorInfos.Any())
                {
                    var infoCount = errorInfos.Count();
                    if (infoCount > 1)
                    {
                        errorInfo = errorInfos.Skip(elementAt).FirstOrDefault();
                        elementAt += 1;
                    }

                    if (errorInfo == null)
                    {
                        continue;
                    }

                    errorCommand = errorInfo.ErrorCode.ValidationStripText();
                    if (errorInfo.CustomState != null && true.CompareTo(errorInfo.CustomState) == 0)
                    {
                        useCustom = true;
                    }

                    if (!string.IsNullOrEmpty(err.PropertyName))
                    {
                        errorCode = $"{errorCommand}_{err.PropertyName}_{errorType}".BeautifulVariable();
                    }
                    else
                    {
                        errorCode = $"{errorCommand}_{errorType}".BeautifulVariable();
                    }

                    if (useCustom)
                    {
                        errorCode = $"{errorCommand}_{errorType}";
                    }
                }

                if (!errorList.ContainsKey(errorCode))
                {
                    if (string.IsNullOrEmpty(err.PropertyName))
                    {
                        var errFields = errorCode.Split("_");
                        if (errFields.Length > 1)
                        {
                            if (errFields[1].IsStopWords())
                            {
                                err.PropertyName = errFields[0];
                            }
                            else
                            {
                                err.PropertyName = errFields[1];
                            }
                        }
                    }
                    errorList.Add(errorCode, new ValidationFailure(err.PropertyName, err.ErrorMessage)
                    {
                        ErrorCode = errorCode,
                        Severity = err.Severity,
                        AttemptedValue = err.AttemptedValue,
                        CustomState = err.CustomState,
                        FormattedMessagePlaceholderValues = err.FormattedMessagePlaceholderValues,
                    });
                }
            }

            return errorList.Values;
        }

        public static IRuleBuilderOptions<T, TProperty> WithErrorCodeAlreadyExists<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string? childCode = null)
        {
            var errCode = "Already_Exists";

            if (!string.IsNullOrEmpty(childCode))
            {
                errCode = $"{childCode}_{errCode}";
            }
            return rule.WithErrorCode(errCode);
        }

        public static IRuleBuilderOptions<T, TProperty> WithErrorCodeMustExists<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string? childCode = null)
        {
            var errCode = "Must_Exists";

            if (!string.IsNullOrEmpty(childCode))
            {
                errCode = $"{childCode}_{errCode}";
            }
            return rule.WithErrorCode(errCode);
        }

        public static IRuleBuilderOptions<T, TProperty> WithErrorCodeDoesNotExists<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string? childCode = null)
        {
            var errCode = "DoesNot_Exists";

            if (!string.IsNullOrEmpty(childCode))
            {
                errCode = $"{childCode}_{errCode}";
            }
            return rule.WithErrorCode(errCode);
        }



        public static IRuleBuilderOptions<T, TProperty> WithErrorGreaterThanNumber<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, int number = 0, string? fieldName = null)
        {
            var errCode = "GreaterThan";
            if (number == 0)
            {
                errCode = $"{errCode}_Zero";
            }
            else
            {
                errCode = $"{errCode}_{number}";
            }
            if (!string.IsNullOrEmpty(fieldName))
            {
                errCode = $"{fieldName}_{errCode}";
            }
            return rule.WithErrorCode(errCode);
        }

        public static IRuleBuilderOptions<T, TProperty> WithErrorGreaterThanOrEqualsNumber<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, int number = 0, string? fieldName = null)
        {
            var errCode = "GreaterThan_Or_Equals";
            if (number == 0)
            {
                errCode = $"{errCode}_Zero";
            }
            else
            {
                errCode = $"{errCode}_{number}";
            }
            if (!string.IsNullOrEmpty(fieldName))
            {
                errCode = $"{fieldName}_{errCode}";
            }
            return rule.WithErrorCode(errCode);
        }
        public static IRuleBuilderOptions<T, TProperty> WithErrorLessThanOrEqualsNumber<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, int number = 0, string? fieldName = null)
        {
            var errCode = "LessThan_Or_Equals";
            if (number == 0)
            {
                errCode = $"{errCode}_Zero";
            }
            else
            {
                errCode = $"{errCode}_{number}";
            }
            if (!string.IsNullOrEmpty(fieldName))
            {
                errCode = $"{fieldName}_{errCode}";
            }
            return rule.WithErrorCode(errCode);
        }

        public static IRuleBuilderOptions<T, TProperty> WithErrorMaximumLength<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, int number = 0, string? fieldName = null)
        {
            var errCode = "MaximumLength";
            if (number == 0)
            {
                errCode = $"{errCode}_Zero";
            }
            else
            {
                errCode = $"{errCode}_{number}";
            }
            if (!string.IsNullOrEmpty(fieldName))
            {
                errCode = $"{fieldName}_{errCode}";
            }
            return rule.WithErrorCode(errCode);
        }

        public static IRuleBuilderOptions<T, TProperty> WithErrorPendingChanges<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string? fieldName = null)
        {
            var errCode = "Pending_Changes";
            if (!string.IsNullOrEmpty(fieldName))
            {
                errCode = $"{fieldName}_{errCode}";
            }
            return rule.WithErrorCode(errCode);
        }

        public static IRuleBuilder<T, TProperty> WithCustomErrorCode<T, TProperty>(this IRuleBuilder<T, TProperty> rule, string? customVal = null, string? childCode = null)
        {
            string errorCode = string.Empty;
            var uValidators = new List<string>();

            return rule.Custom((vprop, context) =>
            {
                errorCode = $"{typeof(T).Name}";
                var failure = new ValidationFailure(context.PropertyName, string.Empty) { ErrorCode = errorCode, Severity = Severity.Info };

                if (!string.IsNullOrEmpty(childCode))
                {
                    failure.ErrorCode = $"{errorCode}_{childCode}";
                    failure.CustomState = true;
                }
                if (!string.IsNullOrEmpty(customVal))
                {
                    failure.ErrorCode = customVal;
                    if (!string.IsNullOrEmpty(childCode))
                    {
                        failure.ErrorCode = $"{customVal}_{childCode}";
                    }
                    failure.CustomState = true;
                }
                context.AddFailure(failure);
            });
        }
    }
}
