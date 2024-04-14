// -------------------------------------------------------------------------------------
//  <copyright file="ICategoryRepository.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Repositories;

using Domain.Categories.Entity;

public interface ICategoryRepository : IRepository<Category, Guid, Category>;