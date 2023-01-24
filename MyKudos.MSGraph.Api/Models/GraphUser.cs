namespace MyKudos.MSGraph.Api.Models;

public class GraphUser
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string UserPrincipalName { get; set; }
}

public class GraphUsers
{
    public GraphUser[] value { get; set; }
}