using MediatR;

namespace UnicornTalentAgency.CQRS.Events;

public class ViewsRefresher :
    INotificationHandler<UnicornAppliedForRole>,
    INotificationHandler<UnicornSelectedForRole>
{
    public Task Handle(UnicornSelectedForRole notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Handle(UnicornAppliedForRole notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}