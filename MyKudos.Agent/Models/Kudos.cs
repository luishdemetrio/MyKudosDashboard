﻿namespace MyKudos.Agent.Models;

public record Kudos(
    Person From,
    Person To,
    Reward Title,
    string Message,
    DateTime SendOn,
    string? ManagerId
);

public record Reward(string Id, string Description);

public record Person(
     string Id ,
     string Name ,
     string Photo 
);

