namespace MyKudos.MSGraph.Api.Models;

public record GraphUserPhoto(string id, string photo);

public record GraphUserPhotoDTO(string id, string body);

public class GraphUserPhotos
{
    public GraphUserPhotoDTO[] responses { get; set; }
}
