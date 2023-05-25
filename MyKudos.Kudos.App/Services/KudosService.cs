
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.App.Services;

public sealed class KudosService : IKudosService
{
    private readonly IKudosRepository _kudosRepository;

    private readonly ICommentsRepository _commentsRepository;

    private readonly IRecognitionRepository _recognitionRepository;

    private readonly IKudosLikeRepository _kudosLikeRepository;

    private readonly object _lock = new object();

    public KudosService(IKudosRepository kudosRepository, ICommentsRepository commentsRepository,
                        IRecognitionRepository recognitionRepository, IKudosLikeRepository kudosLikeRepository)
    {
        _kudosRepository = kudosRepository;
        _commentsRepository = commentsRepository;
        _recognitionRepository = recognitionRepository;
        _kudosLikeRepository = kudosLikeRepository;
    }

    public Task<IEnumerable<Domain.Models.Kudos>> GetKudos(int pageNumber, int pageSize)
    {
        return _kudosRepository.GetKudosAsync(pageNumber, pageSize);
    }

    public IEnumerable<Domain.Models.Kudos> GetUserKudos(string pUserId)
    {
        return _kudosRepository.GetUserKudos(pUserId);
    }


    public Task<IEnumerable<KudosGroupedByValue>> GetUserKudosByCategory(string pUserId)
    {
        lock (_lock)
        {


            var recognitions = _recognitionRepository.GetRecognitions();

            var kudos = _kudosRepository.GetUserKudos(pUserId).Select(k => new MyKudos.Kudos.Domain.Models.Kudos
            {
                KudosId = k.KudosId,
                TitleId = k.TitleId,
                ToPersonId = k.ToPersonId
            }
            ).ToList();


            var result = from kudo in kudos
                         join recognition in recognitions
                            on kudo.TitleId equals recognition.Id.ToString()
                         group recognition by recognition.ValuesCodeGroup into recognitionGroup
                         select new KudosGroupedByValue()
                         {
                             ValueCodeGroup = recognitionGroup.Key,
                             Count = recognitionGroup.Select(r => r.Id).Distinct().Count()
                         };


            return Task.FromResult(result);

        }
    }

    public int Send(Domain.Models.Kudos kudos)
    {
        return (_kudosRepository.Add(kudos));

    }
    public bool Like(int kudosId, string personId)
    {
        return _kudosLikeRepository.Like(kudosId, personId);
    }

    public bool UndoLike(int kudosId, string personId)
    {
        return _kudosLikeRepository.UndoLike(kudosId, personId);
    }

   
}
