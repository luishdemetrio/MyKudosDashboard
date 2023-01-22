
using MyKudos.Domain.Core.Bus;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Services;

public class KudosService : IKudosService
{
    private readonly IEventBus _bus;

    private readonly IKudosRepository _kudosRepository;


    public KudosService(IEventBus bus, IKudosRepository kudosRepository)
    {
        _bus = bus;
        _kudosRepository = kudosRepository;        
    }

    public IEnumerable<KudosLog> GetKudos()
    {
        return _kudosRepository.GetKudos();
    }

    public bool Send(KudosLog kudos)
    {   
        //TODO: need to change here to send a message instead of saving in the repository
        
        return _kudosRepository.Add(kudos);
    }
}
