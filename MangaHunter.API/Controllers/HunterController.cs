using MangaDexSharp.Parameters.Manga;

using MangaHunter.Application.Hunter.Commands.Create;
using MangaHunter.Application.Hunter.Commands.Delete;
using MangaHunter.Application.Hunter.Commands.Update;
using MangaHunter.Application.Hunter.Queries.GetById;
using MangaHunter.Application.Hunter.Queries.GetByUsername;
using MangaHunter.Application.Hunter.Queries.GetByUsernameWithPaginationQuery;
using MangaHunter.Application.Hunter.Queries.GetMangaUpdatesId;
using MangaHunter.Application.Hunter.Queries.GetTodayRss;
using MangaHunter.Application.Hunter.Queries.Search;
using MangaHunter.Contracts.Common;
using MangaHunter.Contracts.Hunter;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Serilog;

using Hunter = MangaHunter.Domain.Entities.Hunter;

namespace MangaHunter.API.Controllers;

[Route("user/{username}/[controller]")]
public class HunterController : ApiController
{
    public HunterController(ISender mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetByUsername(string username, [FromQuery] QueryParameters parameters)
    {
        Log.Debug($"User:{username} want all hunter. \n" +
                  $" Mangadex : {parameters.HasMangadex} MangaUpdate : {parameters.HasMangaUpdates}.");

        var query = new GetByUsernameQuery(Username: username, HasMangadex: parameters.HasMangadex,
            HasMangaUpdates: parameters.HasMangaUpdates, HasMangaUpdatesRss: parameters.HasMangaUpdatesRss);
        var result = await Mediator.Send(query);

        return result.Match(value => Ok(Mapper.Map<List<HunterResponse>>(value)), Problem);
    }

    [HttpGet]
    public async Task<IActionResult> GetByUserUsernameWithPagination(string username,
        [FromQuery] QueryParameters parameters)
    {
        Log.Debug($"User:{username} want page {parameters.Page} by {parameters.Size}. \n" +
                  $"Mangadex : {parameters.HasMangadex} MangaUpdate : {parameters.HasMangaUpdates}.");

        var paginationParameters = Mapper.Map<Domain.Common.PaginationParameters>(parameters);

        var query = new GetByUsernameWithPaginationQuery(Username: username, PaginationParameters: paginationParameters,
            HasMangadex: parameters.HasMangadex,
            HasMangaUpdates: parameters.HasMangaUpdates,
            HasMangaUpdatesRss: parameters.HasMangaUpdatesRss);
        var result = await Mediator.Send(query);

        return result.Match(value => Ok(Mapper.Map<HunterResponseWithPagination>(value)), Problem);
    }

    [HttpGet("{mangadexId}")]
    public async Task<IActionResult> GetById(string mangadexId, string username, [FromQuery] QueryParameters parameters)
    {
        Log.Debug(
            $"User:{username} want Id : {mangadexId}. \n" +
            $" Mangadex : {parameters.HasMangadex} MangaUpdate : {parameters.HasMangaUpdates}.");

        var query = new GetByIdQuery(MangadexId: mangadexId, Username: username, HasMangadex: parameters.HasMangadex,
            HasMangaUpdates: parameters.HasMangaUpdates, HasMangaUpdatesRss: parameters.HasMangaUpdatesRss);
        var result = await Mediator.Send(query);

        return result.Match(value => Ok(Mapper.Map<HunterResponse>(value)), Problem);
    }

    [HttpGet("/[controller]/mangaupdatesid/{mangadexId}")]
    public async Task<IActionResult> GetMangaUpdatesId(string mangadexId)
    {
        Log.Debug($"Some user want a new mangaUpdatesId : {mangadexId}.");

        var query = new GetNewMangaUpdatesIdQuery(mangadexId);
        var result = await Mediator.Send(query);

        return result.Match(value => Ok(value), Problem);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Create(HunterDto hunterDto, string username)
    {
        Log.Debug($"User:{username} want to create a hunter.");

        var command = new CreateCommand(Mapper.Map<Hunter>(hunterDto), username);
        var result = await Mediator.Send(command);

        return result.Match(value => Ok(Mapper.Map<HunterResponse>(value)), Problem);
    }

    [HttpPut("edit/{mangadexId}")]
    public async Task<IActionResult> Update(HunterDto hunterDto, string username, string mangadexId)
    {
        Log.Debug($"User:{username} want to update a hunter.");

        var command = new UpdateCommand(Mapper.Map<Hunter>(hunterDto), username, mangadexId);
        var result = await Mediator.Send(command);

        return result.Match(value => Ok(Mapper.Map<HunterResponse>(value)), Problem);
    }

    [HttpDelete("delete/{mangadexId}")]
    public async Task<IActionResult> Delete(string username, string mangadexId)
    {
        Log.Debug($"User:{username} want to delete a hunter.");

        var command = new DeleteCommand(username, mangadexId);
        var result = await Mediator.Send(command);

        return result.Match(_ => NoContent(), Problem);
    }

    [AllowAnonymous]
    [HttpGet("/rss/today")]
    public async Task<IActionResult> GetTodayRss()
    {
        var query = new GetTodayRssQuery();
        var result = await Mediator.Send(query);
        return result.Match(rss => Ok(Mapper.Map<TodayRssDto>(rss)), Problem);
    }

    [AllowAnonymous]
    [HttpPost("/search")]
    public async Task<IActionResult> Search(SearchQueryParameters parameters)
    {
        Log.Debug($"Search : {parameters.Title}.");
        var query = new SearchQuery(Mapper.Map<GetMangaListParameters>(parameters));
        var result = await Mediator.Send(query);
        return result.Match(value => Ok(Mapper.Map<List<HunterResponse>>(value)), Problem);
    }
}