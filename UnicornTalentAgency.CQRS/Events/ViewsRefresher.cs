using MediatR;
using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CQRS.Read;
using UnicornTalentAgency.CQRS.Read.Queries;
using UnicornTalentAgency.CQRS.Write;

namespace UnicornTalentAgency.CQRS.Events;

public class ViewsRefresher :
    INotificationHandler<UnicornAppliedForRole>,
    INotificationHandler<UnicornSelectedForRole>
{
    private readonly ReadDbContext _readDbContext;
    private readonly WriteDbContext _writeDbContext;

    public ViewsRefresher(ReadDbContext readDbContext, WriteDbContext writeDbContext)
    {
        _readDbContext = readDbContext;
        _writeDbContext = writeDbContext;
    }

    public async Task Handle(UnicornSelectedForRole notification, CancellationToken cancellationToken)
    {
        var role = await _writeDbContext.Roles
            .Include(r => r.Auditions)
            .FirstAsync(x => x.Id == notification.RoleId, cancellationToken);

        var unicorn = await _writeDbContext.Unicorns
            .Include(u => u.Auditions)
            .ThenInclude(a => a.Role)
            .FirstAsync(x => x.Id == notification.UnicornId, cancellationToken);

        var oldDetails = await _readDbContext.UnicornDetails
            .FirstOrDefaultAsync(x => x.Id == notification.UnicornId, cancellationToken);
        if (oldDetails is not null)
            _readDbContext.UnicornDetails.Remove(oldDetails);

        _readDbContext.UnicornDetails.Add(
            UnicornDetails.FromWriteModel(unicorn)
        );

        var oldArchiveItem = await _readDbContext.UnicornArchive
            .FirstOrDefaultAsync(x => x.Id == notification.UnicornId, cancellationToken);
        if (oldArchiveItem is not null)
            _readDbContext.UnicornArchive.Remove(oldArchiveItem);

        _readDbContext.UnicornArchive.Add(
            UnicornArchiveItem.FromWriteModel(unicorn)
        );

        var oldRoleDetails = await _readDbContext.CastingRoleDetails
            .FirstOrDefaultAsync(x => x.Id == notification.RoleId, cancellationToken);
        if (oldRoleDetails is not null)
            _readDbContext.CastingRoleDetails.Remove(oldRoleDetails);

        _readDbContext.CastingRoleDetails.Add(
            CastingRoleDetails.FromWriteModel(role, unicorn)
        );

        await _readDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Handle(UnicornAppliedForRole notification, CancellationToken cancellationToken)
    {
        var unicorn = await _writeDbContext.Unicorns
            .Include(u => u.Auditions)
            .ThenInclude(a => a.Role)
            .FirstAsync(x => x.Id == notification.UnicornId, cancellationToken);

        var oldDetails = await _readDbContext.UnicornDetails
            .FirstOrDefaultAsync(x => x.Id == notification.UnicornId, cancellationToken);
        if (oldDetails is not null)
            _readDbContext.UnicornDetails.Remove(oldDetails);

        _readDbContext.UnicornDetails.Add(
             UnicornDetails.FromWriteModel(unicorn)
         );

        await _readDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RefreshAll(CancellationToken cancellationToken = default)
    {
        var unicorns = await _writeDbContext.Unicorns
            .Include(u => u.Auditions)
            .ThenInclude(a => a.Role)
            .ToListAsync(cancellationToken);

        foreach (var unicorn in unicorns)
        {
            _readDbContext.UnicornDetails.Add(
                UnicornDetails.FromWriteModel(unicorn)
            );

            _readDbContext.UnicornArchive.Add(
                UnicornArchiveItem.FromWriteModel(unicorn)
            );
        }

        var roles = await _writeDbContext.Roles
            .Include(r => r.Auditions)
            .ThenInclude(a => a.Unicorn)
            .ToListAsync(cancellationToken);

        foreach (var role in roles)
        {
            var selectedUnicorn = role.Auditions.FirstOrDefault(a => a.IsSuccessful)?.Unicorn;
            _readDbContext.CastingRoleDetails.Add(
                CastingRoleDetails.FromWriteModel(role, selectedUnicorn)
            );

            _readDbContext.CastingRoleArchive.Add(
                CastingRoleArchive.FromWriteModel(role)
            );
        }

        await _readDbContext.SaveChangesAsync(cancellationToken);
    }
}