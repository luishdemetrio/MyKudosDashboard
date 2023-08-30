using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client;
using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SuperKudos.Aggregator.Domain.Models;
using SuperKudos.Aggregator.Helpers;
using SuperKudos.Aggregator.Interfaces;
using SuperKudos.Aggregator.Protos;
using GrpcClasses = SuperKudos.Aggregator.Protos;

namespace SuperKudos.Aggregator.Grpc;

public class KudosGrpcService : AppCallback.AppCallbackBase
{
    private readonly ILogger<KudosGrpcService> _logger;

    private readonly DaprClient _daprClient;

    private readonly IKudosService _kudosService;
    private string _defaultProfilePicture;

    public KudosGrpcService(DaprClient daprClient, IConfiguration configuration, ILogger<KudosGrpcService> logger, IKudosService kudosService )
    {
        _daprClient = daprClient;
        _logger = logger;
        _defaultProfilePicture = configuration["DefaultProfilePicture"];
        _kudosService = kudosService;
    }

    public override async Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
    {

        var response = new InvokeResponse();

        switch (request.Method)
        {
            case "getKudos":

                var input = request.Data.Unpack<GetKudosRequest>();

                //get kudos
                var kudosRaw = await _kudosService.GetKudosAsync(input.PageNumber);

                var kudos =  KudosHelper.GetKudos(kudosRaw, _defaultProfilePicture);

                var kudosGrpcResponse = new PaginatedGetKudosResponse() ;

                foreach ( var item in kudos )
                {
                    var kudosResponse = new GetKudosResponse()
                    {
                        Id = item.Id,
                        From = new GrpcClasses.Person()
                        {
                            Id = item.From.Id.ToString(),
                            GivenName = item.From.GivenName,
                            Name = item.From.Name,
                            Photo = item.From.Photo

                        },
                        Comments = { item.Comments }
                    };

                    foreach (var like in item.Likes)
                    {
                        kudosResponse.Likes.Add(new GrpcClasses.Person()
                        {
                            Id = like.Id.ToString(),
                            GivenName = like.GivenName,
                            Name = like.Name,
                            Photo = like.Photo
                        });
                    }

                    kudosGrpcResponse.Data.Add(kudosResponse);

                }

                var output = await Task.FromResult<PaginatedGetKudosResponse>(kudosGrpcResponse);

                response.Data = Any.Pack(output);

                break;
        }


        return response;
    }

    public override Task<ListInputBindingsResponse> ListInputBindings(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new ListInputBindingsResponse());
    }


    public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new ListTopicSubscriptionsResponse());
    }

}
