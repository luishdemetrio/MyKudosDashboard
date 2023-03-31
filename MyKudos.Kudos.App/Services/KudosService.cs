﻿
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.App.Services;

public sealed class KudosService : IKudosService
{
    private readonly IKudosRepository _kudosRepository;

    private readonly ICommentsRepository _commentsRepository;

    public KudosService(IKudosRepository kudosRepository, ICommentsRepository commentsRepository)
    {
        _kudosRepository = kudosRepository;
        _commentsRepository = commentsRepository;
    }

    public IEnumerable<KudosLog> GetKudos()
    {
        return _kudosRepository.GetKudos();
    }

    public Guid Send(KudosLog kudos)
    {
        return (_kudosRepository.Add(kudos));

    }
    public int SendLike(string kudosId, string personId)
    {
        return _kudosRepository.SendLike(kudosId, personId);
    }

    public string SendComments(Comments comment)
    {
        var commentsId = _commentsRepository.Add(new Comments()
        {
            KudosId = comment.KudosId,
            FromPersonId = comment.FromPersonId,
            Message = comment.Message,
            Date = comment.Date
        });

        _kudosRepository.SendComments(comment.KudosId, commentsId.ToString());

        return commentsId.ToString();
    }

    public IEnumerable<Comments> GetComments(string kudosId)
    {
        return _commentsRepository.GetComments(kudosId);
    }

    public bool UpdateComments(Comments comments)
    {
        return _commentsRepository.Update(comments);
    }

    public bool DeleteComments(string kudosId, Guid commentId)
    {
        

        if (_commentsRepository.Delete(commentId))
        {
            _kudosRepository.DeleteComments(kudosId, commentId);
            return true;
        }

        return false;
    }
}
