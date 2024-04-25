// -------------------------------------------------------------------------------------
//  <copyright file="Error.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Shared;

using System.Text;

public class Error
{
    public Error(string? code = default, string? message = default, string? property = default)
    {
        Code = code;
        Message = message;
        Property = property;
    }

    public string? Code { get; }

    public string? Message { get; }

    public string? Property { get; }

    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append($"Code: '{Code}' | ");
        builder.Append($"Message: '{Message}'");
        builder.Append($"Property: '{Property}' | ");

        return builder.ToString();
    }
}