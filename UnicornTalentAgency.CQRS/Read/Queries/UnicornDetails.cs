using MediatR;
using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CQRS.Write.Entities;

namespace UnicornTalentAgency.CQRS.Read.Queries;

public record GetUnicornDetails(int Id) : IRequest<UnicornDetails?>;

public record UnicornDetails(
    int Id, 
    string Name, 
    string MagicalAbilities,
    int AuditionsCount,
    int TotalPay,
    string[] SuccessfulRoles){
        public static UnicornDetails FromWriteModel(Unicorn unicorn)
        => new UnicornDetails(
                unicorn.Id,
                unicorn.Name,
                unicorn.MagicalAbilities,
                unicorn.Auditions.Count(),
                unicorn.Auditions.Where(a => a.IsSuccessful).Sum(r => r.Role.Pay),
                unicorn.Auditions.Where(a => a.IsSuccessful).Select(x => x.Role.Name).ToArray()
           );
    }

public class GetUnicornDetailsHandler : IRequestHandler<GetUnicornDetails, UnicornDetails?>{
    private readonly ReadDbContext _dbContext;

    public GetUnicornDetailsHandler(ReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UnicornDetails?> Handle(GetUnicornDetails request, CancellationToken cancellationToken)
    {
        return await _dbContext.UnicornDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }
}