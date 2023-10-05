using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.App.Interfaces;

public interface IRecognitionGroupService
{

    public IEnumerable<RecognitionGroup> GetRecognitionGroups();

    public bool AddNewRecognitionGroup(RecognitionGroup group);

    public bool UpdateRecognitionGroup(RecognitionGroup group);

    public bool RemoveRecognitionGroup(int idRecognitionGroup); 

}
