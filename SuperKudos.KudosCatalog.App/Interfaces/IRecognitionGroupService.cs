using SuperKudos.KudosCatalog.Domain.Models;


namespace SuperKudos.KudosCatalog.App.Interfaces;

public interface IRecognitionGroupService
{

    public IEnumerable<RecognitionGroup> GetRecognitionGroups();

    public bool SetRecognitionGroups(RecognitionGroup group);
    
}
