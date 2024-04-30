// -------------------------------------------------------------------------------------
//  <copyright file="CategoriesController.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Controllers;

using System.Net.Mime;
using Application.Queries.GetCategories;
using Application.Queries.GetCategoryById;
using Application.Responses.Category;
using Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// The categories' controller.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : ApiDemoControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoriesController"/> class.
    /// </summary>
    /// <param name="mediator">An instance of <see cref="IMediator"/>.</param>

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all the categories.
    /// </summary>
    /// <returns>The list of categories.</returns>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, "The list of categories.", typeof(List<CategoryResponse>))]
    [ProducesResponseType(typeof(List<CategoryResponse>), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _mediator.Send(new GetCategoriesQuery());

        return Ok(categories);
    }

    /// <summary>
    /// Get a category by its id.
    /// </summary>
    /// <param name="id">The category ID.</param>
    /// <returns>The category.</returns>
    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, "The category.", typeof(CategoryResponse))]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The category was not found.", typeof(ProblemDetails))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var response = await _mediator.Send(new GetCategoryByIdQuery(id));

        return Ok(response);
    }
}