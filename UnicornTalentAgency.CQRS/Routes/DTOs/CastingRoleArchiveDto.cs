namespace UnicornTalentAgency.CQRS.Routes;

public record CastingRoleArchiveDto(
    int Id,
    string Name,
    DateTimeOffset When
);
