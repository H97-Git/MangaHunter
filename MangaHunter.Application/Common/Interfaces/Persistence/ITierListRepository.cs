using ErrorOr;

using MangaHunter.Domain.Entities;

namespace MangaHunter.Application.Common.Interfaces.Persistence;

public interface ITierListRepository
{
    Task<List<Domain.Entities.TierList>> GetByUsername(string username);
    Task<Domain.Entities.TierList?> GetById(int id);
    Task<Domain.Entities.TierList?> GetByIdNoCaching(int id);
    Task<Domain.Entities.TierList?> GetByShareCode(string shareCode);
    bool IsShareCodeNew(string shareCode);
    Task<Domain.Entities.TierList> Add(Domain.Entities.TierList tierList);
    Task<Updated> Update(Domain.Entities.TierList tierList);
    Task<Deleted> Delete(Domain.Entities.TierList tierList);
    void DeleteRow(TierRow row);
}