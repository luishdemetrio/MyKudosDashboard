using Grpc.Core;
using SuperKudos.Aggregator.Helpers;
using SuperKudos.Aggregator.Interfaces;
using SuperKudos.Aggregator.Protos;
using GrpcClasses = SuperKudos.Aggregator.Protos;

namespace SuperKudos.Aggregator.Grpc;

public class KudosAggregadorService : KudosService.KudosServiceBase
{

    private readonly ILogger<KudosGrpcService> _logger;

    
    private readonly IKudosService _kudosService;
    private string _defaultProfilePicture;

    public KudosAggregadorService(IConfiguration configuration, ILogger<KudosGrpcService> logger, IKudosService kudosService)
    {
        _logger = logger;
        _defaultProfilePicture = configuration["DefaultProfilePicture"];
        _kudosService = kudosService;
    }

    public override async Task<PaginatedGetKudosResponse> GetKudos(GetKudosRequest request, ServerCallContext context)
    {
        //get kudos
        var kudosRaw = await _kudosService.GetKudosAsync(request.PageNumber);

        var kudos = KudosHelper.GetKudos(kudosRaw, _defaultProfilePicture);

        var kudosGrpcResponse = new PaginatedGetKudosResponse();

        foreach (var item in kudos)
        {
            var kudosResponse = new GetKudosResponse()
            {
                Id = item.Id,
                From = new GrpcClasses.Person()
                {
                    Id = item.From.Id.ToString(),
                    GivenName = string.IsNullOrEmpty(item.From.GivenName) ? string.Empty : item.From.GivenName,
                    Name = item.From.Name,
                    Photo = item.From.Photo

                },
                Comments = { item.Comments },
                Title = item.Title,
                SendOn = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(item.SendOn.ToUniversalTime()),
                Message = item.Message
            };

            foreach (var like in item.Likes)
            {
                kudosResponse.Likes.Add(new GrpcClasses.Person()
                {
                    Id = like.Id.ToString(),
                    GivenName = string.IsNullOrEmpty(like.GivenName) ? string.Empty : like.GivenName,
                    Name = like.Name,
                    Photo = like.Photo
                });
            }

            foreach (var receiver in item.Receivers)
            {
                kudosResponse.Receivers.Add(new GrpcClasses.Person()
                {
                    Id = receiver.Id.ToString(),
                    GivenName = string.IsNullOrEmpty(receiver.GivenName) ? string.Empty : receiver.GivenName,
                    Name = receiver.Name,
                    Photo = receiver.Photo
                });
            }


            kudosGrpcResponse.Data.Add(kudosResponse);


            
        }
        return kudosGrpcResponse;
    }
}
