namespace UnicornTalentAgency.CQRS.Events;

public record UnicornAppliedForRole(int UnicornId, int RoleId) : MediatR.INotification;