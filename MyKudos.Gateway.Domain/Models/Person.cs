﻿namespace MyKudos.Gateway.Domain.Models;

public class Person
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Photo { get; set; }

    public string? GivenName { get; set; }

    public string? EMail { get; set; }
}


