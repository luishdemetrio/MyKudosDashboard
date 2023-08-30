
namespace SuperKudos.KudosCatalog.Domain.Models;

public record Like(string KudosId, Person Person);


public record SendLike(int KudosId, Guid FromPersonId);

