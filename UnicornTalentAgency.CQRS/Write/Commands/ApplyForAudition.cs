using MediatR;
using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CQRS.Events;

namespace UnicornTalentAgency.CQRS.Write.Commands;

public record ApplyForAudition(int UnicornId, int RoleId) : MediatR.IRequest;

public class ApplyForAuditionHandler : MediatR.IRequestHandler<ApplyForAudition>
{
    private readonly WriteDbContext _dbContext;
    private readonly IMediator _mediator;

    public ApplyForAuditionHandler(WriteDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task Handle(ApplyForAudition request, CancellationToken cancellationToken)
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

        if (!role.Auditions.Any(a => a.UnicornId == request.UnicornId))
        {
            role.Auditions.Add(new Entities.Audition()
            {
                UnicornId = request.UnicornId,
                Unicorn = unicorn,
                RoleId = request.RoleId,
                Role = role
            });
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new UnicornAppliedForRole(request.UnicornId, request.RoleId), cancellationToken);
        }
    }
}