﻿using MyKudos.Gamification.Domain.Models;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

internal interface IScoreMessageSender
{
    Task NotifyProfileScoreUpdated(UserScore score);
}
