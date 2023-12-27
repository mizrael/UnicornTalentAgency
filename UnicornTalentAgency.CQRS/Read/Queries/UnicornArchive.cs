using MediatR;
using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CQRS.Write.Entities;

namespace UnicornTalentAgency.CQRS.Read.Queries;

public record GetUnicornArchiveItem : IRequest<IEnumerable<UnicornArchiveItem>>;

public record UnicornArchiveItem(int Id, string Name, int TotalPay)
{
    public static UnicornArchiveItem FromWriteModel(Unicorn unicorn)
        => new UnicornArchiveItem(
            unicorn.Id,
            unicorn.Name,
            unicorn.Auditions.Where(a => a.IsSuccessful).Sum(r => r.Role.Pay)
        );
}

public class GetUnicornArchiveItemHandler : IRequestHandler<GetUnicornArchiveItem, IEnumerable<UnicornArchiveItem>>
{
    private readonly ReadDbContext _dbContext;

    public GetUnicornArchiveItemHandler(ReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<UnicornArchiveItem>> Handle(GetUnicornArchiveItem request, CancellationToken cancellationToken)
    {
        return await _dbContext.UnicornArchive
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}