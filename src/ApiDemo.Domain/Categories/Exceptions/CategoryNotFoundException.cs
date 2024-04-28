// -------------------------------------------------------------------------------------
//  <copyright file="CategoryNotFoundException.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Categories.Exceptions;

using ApiDemo.Domain.Exceptions;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(Guid id)
        : base($"Category with id '{id}' was not found.")
    {
    }
}