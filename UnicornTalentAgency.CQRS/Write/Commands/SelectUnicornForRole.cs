
using MediatR;
using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CQRS.Events;

namespace UnicornTalentAgency.CQRS.Write.Commands;

public record SelectUnicornForRole(int UnicornId, int RoleId) : MediatR.IRequest;

public class SelectUnicornForRoleHandler : MediatR.IRequestHandler<SelectUnicornForRole>
{
    private readonly WriteDbContext _dbContext;
    private readonly IMediator _mediator;

    public SelectUnicornForRoleHandler(WriteDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task Handle(SelectUnicornForRole request, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles
            .Include(r => r.Auditions)
            .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);
        if (role is null)
            throw new ArgumentOutOfRangeException($"Role {request.RoleId} does not exist.");

        var unicorn = await _dbContext.Unicorns
            .FirstOrDefaultAsync(u => u.Id == request.UnicornId, cancellationToken);
        if (unicorn is null)
            throw new ArgumentOutOfRangeException($"Unicorn {request.UnicornId} does not exist.");

        if (role.Auditions.Any(a => a.UnicornId != request.UnicornId && a.IsSuccessful))
            throw new ArgumentException("Another unicorn has already been selected for this role.");

        var audition = role.Auditions.FirstOrDefault(a => a.UnicornId == request.UnicornId);
        if (audition is null)
            throw new ArgumentException($"Unicorn {request.UnicornId} has not auditioned for role {request.RoleId}.");

        audition.IsSuccessful = true;
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new UnicornSelectedForRole(request.UnicornId, request.RoleId), cancellationToken);
    }
}