using MediatR;
using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CQRS.Write.Entities;

namespace UnicornTalentAgency.CQRS.Read.Queries;

public record GetCastingRoleArchive : IRequest<IEnumerable<CastingRoleArchive>>;

public record CastingRoleArchive(
    int Id,
    string Name,
    DateTimeOffset When
)
{
    public static CastingRoleArchive FromWriteModel(CastingRole role)
    => new CastingRoleArchive(
        role.Id,
        role.Name,
        role.When
    );
}


public class GetCastingRoleArchiveHandler : IRequestHandler<GetCastingRoleArchive, IEnumerable<CastingRoleArchive>>
{
    private readonly ReadDbContext _dbContext;

    public GetCastingRoleArchiveHandler(ReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CastingRoleArchive>> Handle(GetCastingRoleArchive request, CancellationToken cancellationToken)
    {
        return await _dbContext.CastingRoleArchive
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}