namespace UnicornTalentAgency.CRUD.Persistence.Entities;

public record Audition
{
    public required int UnicornId { get; init; }
    public required Unicorn Unicorn { get; init; }

    public required int RoleId { get; init; }
    public required CastingRole Role { get; init; }
    
    public bool IsSuccessful { get; init; }
}
