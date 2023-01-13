using ErrorOr;

using MangaHunter.Application.Hunter;
using MangaHunter.Domain.Common;

namespace MangaHunter.Application.Common.Interfaces.Persistence;

public interface IHunterRepository
{
    Task<List<Domain.Entities.Hunter>> GetByUsername(string username);
    Task<HunterResultWithPagination> GetByUsernameWithPagination(string username, PaginationParameters parameters);
    Task<Domain.Entities.Hunter?> GetByMangaId(Guid mangadexId, string username);
    Task<Domain.Entities.Hunter?> GetByMangaIdNoCaching(Guid mangadexId, string username);
    Task<long?> GetMangaUpdatesId(Guid mangadexId);
    Task<Domain.Entities.Hunter> Add(Domain.Entities.Hunter hunter);
    Task<Domain.Entities.Hunter> Update(Domain.Entities.Hunter hunter);
    Task<Deleted> Delete(Domain.Entities.Hunter hunter);
}