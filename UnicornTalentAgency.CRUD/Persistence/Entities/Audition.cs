namespace UnicornTalentAgency.CRUD.Persistence.Entities;

public record Audition
{
    public required int UnicornId { get; init; }
    public required Unicorn Unicorn { get; init; }

    public required int CastingCallId { get; init; }
    public required CastingRole CastingCall { get; init; }
    
    public bool IsSuccessful { get; init; }
}
