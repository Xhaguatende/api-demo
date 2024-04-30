// -------------------------------------------------------------------------------------
//  <copyright file="ProductsController.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Controllers;

using System.Net.Mime;
using Application.Commands.DeleteProduct;
using Application.Commands.UpsertProduct;
using Application.Queries.GetProductById;
using Application.Queries.GetProducts;
using Application.Responses.Product;
using Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// The product's controller.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController : ApiDemoControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsController"/> class.
    /// </summary>
    /// <param name="mediator"> An instance of <see cref="IMediator"/>.</param>
    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Delete a product by its id.
    /// </summary>
    /// <param name="id">The product ID.</param>
    /// <returns>The product delete response.</returns>
    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The product was deleted successfully")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Unable to delete the product.", typeof(ProblemDetails))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> DeleteProductAsync(Guid id)
    {
        var response = await _mediator.Send(new DeleteProductCommand(id));

        if (!response.IsSuccess)
        {
            return BadRequest(response.Errors);
        }

        return NoContent();
    }

    /// <summary>
    /// Get a product by its id.
    /// </summary>
    /// <param name="id">The product ID.</param>
    /// <returns>The product response.</returns>
    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, "The product.", typeof(ProductResponse))]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The product was not found.", typeof(ProblemDetails))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetProductAsync(Guid id)
    {
        var response = await _mediator.Send(new GetProductByIdQuery(id));

        return Ok(response);
    }

    /// <summary>
    /// Get all the products.
    /// </summary>
    /// <returns>The list of products</returns>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, "The list of products.", typeof(List<ProductResponse>))]
    [ProducesResponseType(typeof(List<ProductResponse>), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetProductsAsync()
    {
        var response = await _mediator.Send(new GetProductsQuery());

        return Ok(response);
    }

    /// <summary>
    /// Upsert a product.
    /// </summary>
    /// <param name="command">The upsert command.</param>
    /// <returns>The upserted product.</returns>
    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, "The product was upserted successfully.", typeof(ProductResponse))]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Unable to upsert the product.", typeof(ProblemDetails))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> UpsertProductAsync([FromBody] UpsertProductCommand command)
    {
        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Value);
    }
}