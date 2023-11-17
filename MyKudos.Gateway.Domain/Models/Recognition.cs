using System.ComponentModel.DataAnnotations;

namespace MyKudos.Gateway.Domain.Models;

public class Recognition
{
    public int RecognitionId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }
    
    public int DisplayOrder { get; set; }
    
    [Required(ErrorMessage = "Group is required")]
    public int RecognitionGroupId { get; set; }

    public string RecognitionGroupName { get; set; }
}

public class RecognitionGroup
{
    public int RecognitionGroupId { get; set; }
    public string Description { get; set; }
    public string BadgeName { get; set; }
    public string Emoji { get; set; }
}

public class RecognitionViewModel
{
    public int RecognitionId { get; set; }
    public string Emoji { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; }
    public string GroupName { get; set; }
}
