namespace MyKudosDashboard.Models;

public record GraphUserPhotoDTO(string id, string body);

public class GraphUserPhotos
{
    public GraphUserPhotoDTO[] responses { get; set; }
}





