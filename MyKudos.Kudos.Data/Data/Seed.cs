

namespace MyKudos.Kudos.Data.Data;


public static class Seed
{
    public static ICollection<Domain.Models.Recognition> Data =>
        new[]
        {
            new Domain.Models.Recognition
            {
                
                Title = "Awesome",
                Description = "Recognizes outstanding performance or behavior that goes above and beyond expectations.",
                RecognitionGroupId=0


            },
            new Domain.Models.Recognition
            {
                Title = "Thank you",
                Description = "Expresses gratitude for specific actions or efforts made by the employee that had a positive impact.",
                RecognitionGroupId=1
            },
            new Domain.Models.Recognition
            {
                Title = "Congratulations",
                Description = "Celebrates significant accomplishments or milestones achieved by the employee.",
                RecognitionGroupId=0
            },
            new Domain.Models.Recognition
            {
                Title = "Achiever",
                Description = "Acknowledges employees who consistently achieve their goals and demonstrate a commitment to excellence.",
                RecognitionGroupId=2
            },
            new Domain.Models.Recognition
            {
                Title = "Problem Solver",
                Description = "Recognizes employees who have demonstrated exceptional problem-solving skills and contributed to the success of a project or initiative.",
                RecognitionGroupId=2
            },
            new Domain.Models.Recognition
            {
                Title = "Courage",
                Description = "Acknowledges employees who have shown courage, resilience, or perseverance in the face of challenges or adversity.",
                RecognitionGroupId=2
            },
            new Domain.Models.Recognition
            {
                Title = "Team Player",
                Description = "Recognizes employees who have demonstrated strong teamwork skills, contributed to a positive team dynamic, and supported their colleagues.",
                RecognitionGroupId=1
            }
        };
}
