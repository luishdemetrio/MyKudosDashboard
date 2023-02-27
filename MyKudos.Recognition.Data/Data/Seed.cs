

namespace MyKudos.Recognition.Data.Data;


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
