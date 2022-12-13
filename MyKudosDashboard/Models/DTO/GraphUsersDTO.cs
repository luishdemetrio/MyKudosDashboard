namespace MyKudosDashboard.Models;

public record GraphUserDTO(string id, string displayName, string userPrincipalName);

public class GraphUsersDTO
{
    public GraphUserDTO[] value { get; set; }
}
