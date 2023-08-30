//using Grpc.Net.Client;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static MyKudos.Recognition.gRPC.RecognitionService;

//namespace SuperKudos.KudosCatalog.Domain.Services;

//public class RecognitionService
//{

//    private readonly string _recognitionServiceUrl;
    

//    public RecognitionService(IConfiguration config)
//    {
//        _recognitionServiceUrl = config["recognitionServiceUrl"];
        
//    }

   
//    public void GetRecognitions()
//    {
//        var recognitionClient = new RecognitionServiceClient(
//                         GrpcChannel.ForAddress(_recognitionServiceUrl)
//                      );

//        recognitionClient.GetRecognitions()
//    }
//}


//public class Recognition
//{

//    public Guid Id { get; set; } = Guid.NewGuid();
//    public string Emoji { get; set; }
//    public string Description { get; set; }
//    public bool IsSelected { get; set; }
//}