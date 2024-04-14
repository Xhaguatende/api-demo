// -------------------------------------------------------------------------------------
//  <copyright file="IProductRepository.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Repositories;

using Domain.Products.Aggregate;
using Domain.Products.Entity;

public interface IProductRepository : IRepository<Product, Guid, ProductAggregate>;