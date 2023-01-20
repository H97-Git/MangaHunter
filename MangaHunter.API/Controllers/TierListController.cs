using MangaHunter.Application.TierList.Commands.Create;
using MangaHunter.Application.TierList.Commands.Delete;
using MangaHunter.Application.TierList.Commands.Update;
using MangaHunter.Application.TierList.Queries.GetById;
using MangaHunter.Application.TierList.Queries.GetByShareCode;
using MangaHunter.Application.TierList.Queries.GetByUsername;
using MangaHunter.Contracts.TierList;
using MangaHunter.Domain.Entities;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Serilog;

namespace MangaHunter.API.Controllers;
[AllowAnonymous]
[Route("user/{username}/[controller]")]
public class TierListController : ApiController
{
    public TierListController(ISender mediator, IMapper mapper) : base(mediator, mapper)
    {
    }


    [HttpGet]
    [Route("/[controller]/{shareCode}")]
    public async Task<IActionResult> GetByShareCode(string shareCode)
    {
        Log.Debug($"Anonymous user want Tierlist : {shareCode}.");
        var query = new GetByShareCodeQuery(shareCode);
        var result = await Mediator.Send(query);
        return result.Match(value => Ok(Mapper.Map<TierListResponse>(value)), Problem);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByUserName(string username)
    {
        Log.Debug($"User:{username} want all Tierlist.");

        var query = new GetByUsernameQuery(username);
        var result = await Mediator.Send(query);
        return result.Match(value => Ok(Mapper.Map<List<TierListDto>>(value)), Problem);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(string username, int id)
    {
        Log.Debug($"User:{username} want Tierlist : {id}.");
        
        var query = new GetByIdQuery(id, username);
        var result = await Mediator.Send(query);
        return result.Match(value => Ok(Mapper.Map<TierListResponse>(value)), Problem);
    }


    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create(string username, [FromBody] TierListDto tierListModel)
    {
        Log.Debug($"User:{username} want to create a Tierlist.");
        
        var command = new CreateCommand(TierList: Mapper.Map<TierList>(tierListModel), Username: username);
        var result = await Mediator.Send(command);
        return result.Match(value => Ok(Mapper.Map<TierListDto>(value)), Problem);
    }


    [HttpPut("edit/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(string username, int id, [FromBody] TierListDto tierListModel)
    {
        Log.Debug($"User:{username} want to update a Tierlist.");
        
        var command = new UpdateCommand(TierList: Mapper.Map<TierList>(tierListModel), Id: id, Username: username);
        var result = await Mediator.Send(command);
        return result.Match(value => Ok(Mapper.Map<TierListDto>(value)), Problem);
    }


    [HttpDelete("delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(string username, int id)
    {
        Log.Debug($"User:{username} want to delete a Tierlist.");

        var command = new DeleteCommand(username, id);
        var result = await Mediator.Send(command);
        return result.Match(_ => NoContent(), Problem);
    }
}