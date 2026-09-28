namespace MyApp.Web.Client.Models;

public record Project(
    string Index,
    string Name,
    string Summary,
    string[] Tags,
    string[] Tech,
    string Live,
    string Github,
    string Year
);

public record HackathonRecord(
    string Year,
    string Event,
    string Award,
    string Badge
);

public record TimelineItem(
    string Year,
    string Role,
    string Org,
    string Desc
);

public record ContactFormModel
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public record SocialLink(
    string Label,
    string Handle,
    string Url
);

public record ReviewItem(
    string Id,
    string Author,
    string Role,
    int Rating,
    string Title,
    string Comment,
    string Date,
    string Category,
    int Upvotes = 0
);

public class ReviewSubmissionModel
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public string Title { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string Category { get; set; } = "Portfolio UX";
}