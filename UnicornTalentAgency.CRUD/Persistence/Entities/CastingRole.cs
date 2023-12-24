namespace UnicornTalentAgency.CRUD.Persistence.Entities;

public record CastingRole
{
    public int Id { get; set; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Location { get; init; }
    public required int Pay { get; init; }
    public required DateTimeOffset When { get; init; }
    public List<Audition> Auditions { get; init; } = new();

}
