namespace UnicornTalentAgency.CRUD.Persistence.Entities;

public record Unicorn
{
    public int Id { get; set; }
    public required string Name { get; init; }
    public required string MagicalAbilities { get; init; }
    public List<Audition> Auditions { get; init; } = new ();
}
