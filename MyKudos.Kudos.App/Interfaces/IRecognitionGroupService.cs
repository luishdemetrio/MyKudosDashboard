using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.App.Interfaces;

public interface IRecognitionGroupService
{

    public IEnumerable<RecognitionGroup> GetRecognitionGroups();

    public bool SetRecognitionGroups(RecognitionGroup group);
    
}
