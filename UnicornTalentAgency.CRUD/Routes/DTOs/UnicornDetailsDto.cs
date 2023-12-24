namespace UnicornTalentAgency.CRUD.Routes;

public record UnicornDetailsDto(
    int Id, 
    string Name, 
    string MagicalAbilities,
    int TotalPay,
    string[] SuccessfulRoles);