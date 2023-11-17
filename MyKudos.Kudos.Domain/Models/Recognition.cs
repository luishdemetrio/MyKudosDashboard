
using System.ComponentModel.DataAnnotations.Schema;

namespace MyKudos.Kudos.Domain.Models;

public class Recognition
{

    public int RecognitionId { get; set; } 
    
    public string Title { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; }
    public int RecognitionGroupId { get; set; }


}


public class RecognitionGroup
{
    public int RecognitionGroupId { get; set; }
    public string Description { get; set; }
    public string BadgeName { get; set; }
    public string Emoji { get; set; }
}