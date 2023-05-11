﻿

namespace MyKudos.Kudos.App.Interfaces;

public interface IRecognitionService
{
    IEnumerable<Domain.Models.Recognition> GetRecognitions();

    Task SeedDatabaseAsync();
}