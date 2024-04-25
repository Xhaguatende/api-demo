// -------------------------------------------------------------------------------------
//  <copyright file="Result.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Shared;

using System.Collections.Generic;

public class Result<T> where T : class
{
    private Result(T? value)
    {
        Value = value;
        Errors = [];
    }

    private Result(List<Error> errors)
    {
        Errors = errors;
    }

    public List<Error> Errors { get; }

    public bool IsSuccess => Errors.Count == 0;

    public T? Value { get; private set; }

    public static Result<T> Failure(List<Error> errors)
    {
        return new Result<T>(errors);
    }

    public static implicit operator Result<T>(T? value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public void AddError(Error error)
    {
        Errors.Add(error);
    }
}