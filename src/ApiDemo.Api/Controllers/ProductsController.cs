// -------------------------------------------------------------------------------------
//  <copyright file="ProductsController.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Controllers;

using Application.Commands.DeleteProduct;
using Application.Commands.UpsertProduct;
using Application.Queries.GetProductById;
using Application.Queries.GetProductsAggregate;
using AutoMapper;
using Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProductAsync(Guid id)
    {
        await _mediator.Send(new DeleteProductCommand(id));

        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductAsync(Guid id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));

        var response = _mapper.Map<GetProductDto>(product);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetProductsAsync()
    {
        var products = await _mediator.Send(new GetProductsAggregateQuery());

        var response = _mapper.Map<List<GetProductDto>>(products);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> UpsertProductAsync([FromBody] UpsertProductDto upsertProduct)
    {
        var command = _mapper.Map<UpsertProductCommand>(upsertProduct);

        var response = _mapper.Map<UpsertProductDto>(await _mediator.Send(command));

        return Ok(response);
    }
}