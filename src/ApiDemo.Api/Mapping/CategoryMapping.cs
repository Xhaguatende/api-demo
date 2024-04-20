// -------------------------------------------------------------------------------------
//  <copyright file="CategoryMapping.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Mapping;

using AutoMapper;
using Domain.Categories.Entity;
using Dtos.Category;

public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<Category, GetCategoryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}