using MyKudos.Kudos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

public  interface IScoreMessageSender
{

    Task NotifyProfileScoreUpdated(UserPointScore score);
}
