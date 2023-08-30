namespace MyKudos.Gateway.Models;

public class GraphUser
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string UserPrincipalName { get; set; }
}

public class GraphUsers
{
    public List<GraphUser> value { get; set; } = new();
}