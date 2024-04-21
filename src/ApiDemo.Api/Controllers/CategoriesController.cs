// -------------------------------------------------------------------------------------
//  <copyright file="CategoriesController.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Controllers;

using Application.Queries.GetCategories;
using Application.Queries.GetCategoryById;
using AutoMapper;
using Dtos.Category;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _mediator.Send(new GetCategoriesQuery());

        var response = _mapper.Map<List<GetCategoryDto>>(categories);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));

        var response = _mapper.Map<GetCategoryDto>(category);

        return Ok(response);
    }
}