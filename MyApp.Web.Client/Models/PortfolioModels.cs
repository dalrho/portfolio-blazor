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