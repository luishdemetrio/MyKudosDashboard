﻿using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Common;

public static class KudosCommonVariables
{
    public static bool VisualizeJustMyTeam { get; set; }

    public static UserProfile User { get; set; }

    public static KudosTypeTab KudosTypeTab { get; set; }

    public static bool UsesAzureOpenAI { get; set; }    

    public static Guid? GetManagerId()
    {
        return VisualizeJustMyTeam ? User.UserProfileId : null;
    }
}
