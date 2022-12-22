using MyKudos.Recognition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKudos.Recognition.Data.Data;

//context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "🏆", "Awesome", false));
//context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "✨", "Thank you", false));
//context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "🎉", "Congratulations", false));
//context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "🏅", "Achiever", false));
//context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "💡", "Problem Solver", false));
//context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "🦁", "Courage", false));
//context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "🏀", "Team Player", false));

public static class Seed
{
    public static ICollection<Domain.Models.Recognition> Data =>
        new[]
        {
            new Domain.Models.Recognition
            {
                Emoji="🏆",
                Description = "Awesome"

            },
            new Domain.Models.Recognition
            {
                Emoji="✨",
                Description = "Thank you"
            },
            new Domain.Models.Recognition
            {
                Emoji="🎉",
                Description = "Congratulations"
            },
            new Domain.Models.Recognition
            {
                Emoji="🏅",
                Description = "Achiever"
            },
            new Domain.Models.Recognition
            {
                Emoji="💡" ,
                Description = "Problem Solver"
            },
            new Domain.Models.Recognition
            {
                Emoji="🦁" ,
                Description = "Courage"
            },
            new Domain.Models.Recognition
            {
                Emoji="🏀" ,
                Description = "Team Player"
            }
        };
}
