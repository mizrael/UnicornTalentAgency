namespace UnicornTalentAgency.CRUD.Routes;

public record CastingRoleDetailsDto(
    int Id,
    string Name,
    string Description,
    string Location,
    int Pay,
    DateTimeOffset When
);