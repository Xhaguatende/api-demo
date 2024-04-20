// -------------------------------------------------------------------------------------
//  <copyright file="ProductMapping.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Mapping;

using Application.Commands.UpsertProduct;
using AutoMapper;
using Domain.Products.Aggregate;
using Domain.Products.Entity;
using Dtos.Product;

public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<ProductAggregate, GetProductDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(
                dest => dest.Category,
                opt => opt.MapFrom(
                    src => new ProductCategoryDto
                    {
                        Id = src.Category.Id,
                        Name = src.Category.Name
                    }));

        CreateMap<UpsertProductDto, UpsertProductCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock));

        CreateMap<Product, UpsertProductDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock));
    }
}