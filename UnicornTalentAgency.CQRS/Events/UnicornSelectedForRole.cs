namespace UnicornTalentAgency.CQRS.Events;

public record UnicornSelectedForRole(int UnicornId, int RoleId) : MediatR.INotification;