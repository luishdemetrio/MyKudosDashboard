﻿
namespace MyKudos.Kudos.Domain.Models;

public class Recognition
{

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Emoji { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsSelected { get; set; }
    public int DisplayOrder { get; set; }
    public int ValuesCodeGroup { get; set; }


}