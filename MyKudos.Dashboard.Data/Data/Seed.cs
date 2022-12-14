using MyKudos.Dashboard.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKudos.Dashboard.Data.Data
{
    //context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "🏆", "Awesome", false));
    //context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "✨", "Thank you", false));
    //context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "🎉", "Congratulations", false));
    //context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "🏅", "Achiever", false));
    //context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "💡", "Problem Solver", false));
    //context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "🦁", "Courage", false));
    //context.Recognitions?.Add(new Recognition(Guid.NewGuid().ToString(), "🏀", "Team Player", false));

    public static class Seed
    {
        public static ICollection<Recognition> Data =>
            new[]
            {
                new Recognition
                {
                    Emoji="🏆",
                    Description = "Awesome"

                },
                new Recognition
                {
                    Emoji="✨",
                    Description = "Thank you"
                },
                new Recognition
                {
                    Emoji="🎉",
                    Description = "Congratulations"
                },
                new Recognition
                {
                    Emoji="🏅",
                    Description = "Achiever"
                },
                new Recognition
                {
                    Emoji="💡" ,
                    Description = "Problem Solver"
                },
                new Recognition
                {
                    Emoji="🦁" ,
                    Description = "Courage"
                },
                new Recognition
                {
                    Emoji="🏀" ,
                    Description = "Team Player"
                }
            };
    }
}
