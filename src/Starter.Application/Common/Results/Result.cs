
using System.Diagnostics.CodeAnalysis;

namespace Starter.Application.Common.Results;

public abstract class Result(bool isSuccess, Error? error)
{
    private readonly Error? _error = error;

    public bool IsSuccess { get; } = isSuccess;

    public Error Error
    {
        get
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException("Cannot access the error of a successful result.");
            }

            return _error!;
        }
    }

    public bool IsFailure => !IsSuccess;
}

[SuppressMessage(
    "Design",
    "CA1000:Do not declare static members on generic types",
    Justification = "Static factory and conversion methods are intentionally exposed by Result<T>.")]
public sealed class Result<T> : Result
{
    private readonly T? _value;
    private Result(bool isSuccess, T? value, Error? error) : base(isSuccess, error)
    {
        _value = value;
    }
    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException("Cannot access the value of a failed result.");
            }
            return _value!;
        }
    }

    public static Result<T> Success(T value) => new(isSuccess: true, value, error: null);

    public static Result<T> Failure(Error error) => new(isSuccess: false, value: default, error);

    public static implicit operator Result<T>(Error error)
    {
        return Result<T>.Failure(error);
    }

    public static Result<T> ToResult(Error error) => Result<T>.Failure(error);
}
