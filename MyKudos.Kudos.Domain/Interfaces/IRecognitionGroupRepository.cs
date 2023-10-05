using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.Domain.Interfaces;

public interface IRecognitionGroupRepository
{

    IEnumerable<RecognitionGroup> GetRecognitionsGroup();

    bool AddNewRecognitionGroup(RecognitionGroup group);

    bool DeleteRecognitionGroup(int recognitionGroupId);

    bool UpdateRecognitionGroup(RecognitionGroup group);

}
