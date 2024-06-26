﻿
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

    public Task<IEnumerable<Domain.Models.Kudos>> GetKudos(int pageNumber, int pageSize, 
                                                           Guid? managerId = null, int? year = null)
    {
        return _kudosRepository.GetKudosAsync(pageNumber, pageSize, managerId, year);
    }

    public Task<IEnumerable<Domain.Models.Kudos>> GetKudosFromMeAsync(Guid pUserId, int pageNumber = 1, 
                                                                      int pageSize = 5, 
                                                                      Guid? managerId = null,
                                                                      int? year = null)
    {
        return _kudosRepository.GetKudosFromMeAsync(pUserId, pageNumber, pageSize, managerId, year);
    }

    public Task<IEnumerable<Domain.Models.Kudos>> GetKudosToMeAsync(Guid pUserId, int pageNumber = 1, 
                                                                    int pageSize = 5, 
                                                                    Guid? managerId = null, 
                                                                    int? year = null)
    {
        return _kudosRepository.GetKudosToMeAsync(pUserId, pageNumber, pageSize, managerId, year);
    }


    public IEnumerable<Domain.Models.Kudos> GetUserKudos(Guid pUserId, int? year = null)
    {
        return _kudosRepository.GetUserKudos(pUserId, year);
    }


    public Task<IEnumerable<KudosGroupedByValue>> GetUserKudosByCategory(Guid pUserId)
    {
        lock (_lock)
        {


            var recognitions = _recognitionRepository.GetRecognitions();

            var kudos = _kudosRepository.GetUserKudos(pUserId).Select(k => new MyKudos.Kudos.Domain.Models.Kudos
            {
                KudosId = k.KudosId,
                RecognitionId = k.RecognitionId,
                Recognized = k.Recognized
            }
            ).ToList();


            var result = from kudo in kudos
                         join recognition in recognitions
                            on kudo.RecognitionId equals recognition.RecognitionId
                         group recognition by recognition.RecognitionGroupId into recognitionGroup
                         select new KudosGroupedByValue()
                         {
                             ValueCodeGroup = recognitionGroup.Key,
                             Count = recognitionGroup.Select(r => r.RecognitionId).Distinct().Count()
                         };


            return Task.FromResult(result);

        }
    }

    public int Send(Domain.Models.Kudos kudos)
    {
        return (_kudosRepository.Add(kudos));

    }
    public bool Like(int kudosId, Guid personId)
    {
        return _kudosLikeRepository.Like(kudosId, personId);
    }

    public bool UndoLike(int kudosId, Guid personId)
    {
        return _kudosLikeRepository.UndoLike(kudosId, personId);
    }

    public Domain.Models.Kudos GetKudos(int kudosId)
    {
        return _kudosRepository.GetKudos(kudosId);
    }

    public async Task<IEnumerable<Domain.Models.Kudos>> GetKudosByName(string name, int pageSize, int fromNumberOfDays)
    {
        return await _kudosRepository.GetKudosByName(name, pageSize, fromNumberOfDays);
    }

    public bool UpdateKudos(int kudosId, string? message)
    {
        return _kudosRepository.UpdateMessage(kudosId, message);
    }

    public bool DeleteKudos(int kudosId)
    {
        return _kudosRepository.Delete(kudosId);
    }
}
