namespace MyKudos.Gateway.Models;

public record GraphUserPhoto(string id, string body);

public class GraphUserPhotos
{
    public GraphUserPhoto[] responses { get; set; }
}
