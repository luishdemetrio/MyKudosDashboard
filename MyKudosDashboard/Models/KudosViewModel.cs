namespace MyKudosDashboard.Models
{
    public class KudosViewModel
    {
        public string Id { get; set; }

        public string FromPersonId { get; set; }

        public string ToPersonId { get; set; }

        public string TitleId { get; set; }

        public string? Message { get; set; }

        public DateTime SendOn { get; set; }
    }

    public class Person
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }
    }

    public record KudosRequest (
        Person From,
        Person To,
        Reward Title,
        string Message
    );

    public record Reward(string Id, string Description);


    public class KudosResponse
    {
        public string Id { get; set; }

        public Person From { get; set; }

        public Person To { get; set; }

        public string Title { get; set; }

        public string? Message { get; set; }

        public DateTime SendOn { get; set; }
    }

}
