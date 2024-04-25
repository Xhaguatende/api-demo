// -------------------------------------------------------------------------------------
//  <copyright file="ProductsController.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Controllers;

using Application.Commands.DeleteProduct;
using Application.Commands.UpsertProduct;
using Application.Queries.GetProductById;
using Application.Queries.GetProducts;
using Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController : ApiDemoControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProductAsync(Guid id)
    {
        var response = await _mediator.Send(new DeleteProductCommand(id));

        if (!response.IsSuccess)
        {
            return BadRequest(response.Errors);
        }

        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductAsync(Guid id)
    {
        var response = await _mediator.Send(new GetProductByIdQuery(id));

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetProductsAsync()
    {
        var response = await _mediator.Send(new GetProductsQuery());

        return Ok(response);
    }

    [HttpPost]
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