using MediatR;
using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CQRS.Write.Entities;

namespace UnicornTalentAgency.CQRS.Read.Queries;

public record GetCastingRoleDetails(int Id) : IRequest<CastingRoleDetails?>;

public record CastingRoleDetails(
    int Id,
    string Name,
    string Description,
    string Location,
    int Pay,
    DateTimeOffset When,
    string SelectedUnicornName
){
    public static CastingRoleDetails FromWriteModel(CastingRole role, Unicorn? selectedUnicorn)
        => new CastingRoleDetails(
            role.Id,
            role.Name,
            role.Description,
            role.Location,
            role.Pay,
            role.When,
            selectedUnicorn?.Name ?? "No unicorn selected"
        );

}

public class GetCastingRoleDetailsHandler : IRequestHandler<GetCastingRoleDetails, CastingRoleDetails>
{
    private readonly ReadDbContext _dbContext;

    public GetCastingRoleDetailsHandler(ReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CastingRoleDetails?> Handle(GetCastingRoleDetails request, CancellationToken cancellationToken)
    {
        return await _dbContext.CastingRoleDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }
}