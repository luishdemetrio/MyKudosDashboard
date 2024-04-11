using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Common;

public class KudosCommonVariables 
{
    public  bool VisualizeJustMyTeam { get; set; }

    public UserProfile User { get; set; }

    public KudosTypeTab KudosTypeTab { get; set; }

    public bool UsesAzureOpenAI { get; set; }
    
    public bool ShowTopContributors { get; set; }
    
    public bool ShowAllKudosTab { get; set; }

    public bool HasDirectReports { get; set; }
    
    public bool IsMobile { get; set; }


    public Guid? GetManagerId()
    {
        return VisualizeJustMyTeam ? User.UserProfileId : null;
    }
}
