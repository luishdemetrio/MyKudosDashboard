namespace MyKudos.Gateway.Models;

public class Recognition
{
    public string Id { get; set; }
    public string Emoji { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; }
}
