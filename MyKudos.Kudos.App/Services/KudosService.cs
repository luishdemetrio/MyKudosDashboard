
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Services;

public class KudosService : IKudosService
{
    private readonly IKudosRepository _kudosRepository;


    public KudosService(IKudosRepository kudosRepository)
    {
        _kudosRepository = kudosRepository;        
    }

    public IEnumerable<KudosLog> GetKudos()
    {
        return _kudosRepository.GetKudos();
    }

    public Guid Send(KudosLog kudos)
    {
        return ( _kudosRepository.Add(kudos) );
        
    }
    public bool SendLike(string kudosId, string personId)
    {
        return _kudosRepository.SendLike(kudosId, personId);


    }

  
}
