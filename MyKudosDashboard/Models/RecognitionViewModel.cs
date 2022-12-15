﻿namespace MyKudosDashboard.Models;

public class RecognitionViewModel
{
    public string Id { get; set; }
    public string Emoji { get; set; }
    public string Description { get; set; }
    public bool IsSelected { get; set; }

    public RecognitionViewModel(string emoji, string description, bool isSelected)
    {
        Emoji = emoji;
        Description = description;
        IsSelected = isSelected;

    }
}