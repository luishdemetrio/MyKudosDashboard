using SuperKudos.KudosCatalog.Domain.Models;


namespace SuperKudos.KudosCatalog.Domain.Interfaces;

public interface IRecognitionGroupRepository
{

    IEnumerable<RecognitionGroup> GetRecognitionGroups();

    bool SetRecognitionGroups(RecognitionGroup group);

}
