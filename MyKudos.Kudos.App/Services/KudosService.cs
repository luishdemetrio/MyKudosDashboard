
using MyKudos.Kudos.Domain.Commands;
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


        //var createTransferCommand = new CreateSendKudosCommand
        //    (
        //       fromPersonId: kudos.FromPersonId,
        //       toPersonId: kudos.ToPersonId,
        //       titleId: kudos.TitleId,
        //       message: kudos.Message,
        //       date: kudos.Date
        //    );

        //_bus.SendCommand(createTransferCommand);

        //return true;

        return _kudosRepository.Add(kudos);
    }

    public bool SendLike(string kudosId, string personId)
    {
        return _kudosRepository.SendLike(kudosId, personId);


    }

  
}
