using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.Domain.Interfaces;

public interface IRecognitionGroupRepository
{

    IEnumerable<RecognitionGroup> GetRecognitionGroups();

    bool SetRecognitionGroups(RecognitionGroup group);

}
