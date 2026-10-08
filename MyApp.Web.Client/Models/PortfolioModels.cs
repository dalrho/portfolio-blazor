namespace MyApp.Web.Client.Models;

#region Enums

public enum SkillCategory
{
    Language,
    Framework,
    Database,
    CloudAndDevOps,
    Tool,
    Other
}

public enum AchievementBadge
{
    WINNER,
    FINALIST,
    HONORABLE_MENTION,
    PARTICIPANT
}

public enum MessageStatus
{
    New,
    Read,
    Archived,
    Spam
}

#endregion

#region Base Abstractions (Inheritance Hierarchy)

/// <summary>
/// Base class for all persisted entities with an identifier.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
}

/// <summary>
/// Base entity with creation and modification tracking timestamps.
/// </summary>
public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Base entity for items that can be explicitly sorted in UI lists.
/// </summary>
public abstract class SortableEntity : AuditableEntity
{
    public int SortOrder { get; set; }
}

#endregion

#region Core Domain Entities

/// <summary>
/// Represents a portfolio project or case study.
/// </summary>
public class Project : SortableEntity
{
    public string Index { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[] Tags { get; set; } = [];
    public string[] Tech { get; set; } = [];
    public string Live { get; set; } = string.Empty;
    public string Github { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public bool IsFeatured { get; set; } = true;
    public int ViewCount { get; set; }

    // Composition: Project owns its gallery images (lifecycle-bound)
    public List<ProjectImage> Images { get; set; } = [];

    // Aggregation / Many-to-Many associations
    public List<ProjectTag> ProjectTags { get; set; } = [];
    public List<ProjectTechnology> ProjectTechnologies { get; set; } = [];

    // Methods
    public void IncrementViews() => ViewCount++;
    public bool HasTag(string tagName) => Tags.Contains(tagName, StringComparer.OrdinalIgnoreCase);

    public Project() { }

    public Project(
        string index,
        string name,
        string summary,
        string[] tags,
        string[] tech,
        string live,
        string github,
        string year)
    {
        Index = index;
        Name = name;
        Slug = name.ToLowerInvariant().Replace(" ", "-");
        Summary = summary;
        Tags = tags;
        Tech = tech;
        Live = live;
        Github = github;
        Year = year;
    }
}

/// <summary>
/// Image belonging strictly to a Project (Composition).
/// </summary>
public class ProjectImage : BaseEntity
{
    public int ProjectId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public bool IsHero { get; set; }
    public int SortOrder { get; set; }
}

/// <summary>
/// Domain category / tag for filtering projects.
/// </summary>
public class Tag : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public List<ProjectTag> ProjectTags { get; set; } = [];
}

/// <summary>
/// Join entity for Project and Tag (Association class).
/// </summary>
public class ProjectTag
{
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}

/// <summary>
/// Technology or skill entry (Aggregation with Project).
/// </summary>
public class Technology : SortableEntity
{
    public string Name { get; set; } = string.Empty;
    public SkillCategory Category { get; set; } = SkillCategory.Tool;
    public string? IconSvg { get; set; }
    public int ProficiencyPercent { get; set; } = 90;
    public bool IsPrimary { get; set; } = false;
    public List<ProjectTechnology> ProjectTechnologies { get; set; } = [];
}

/// <summary>
/// Join entity for Project and Technology (Association class).
/// </summary>
public class ProjectTechnology
{
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public int TechnologyId { get; set; }
    public Technology Technology { get; set; } = null!;
}

/// <summary>
/// Career work experience / timeline item.
/// </summary>
public class TimelineItem : SortableEntity
{
    public string Year { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Org { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public string? Location { get; set; }
    public bool IsCurrent { get; set; }

    public TimelineItem() { }

    public TimelineItem(string year, string role, string org, string desc)
    {
        Year = year;
        Role = role;
        Org = org;
        Desc = desc;
    }
}

/// <summary>
/// Hackathon or open source achievement.
/// </summary>
public class HackathonRecord : SortableEntity
{
    public string Year { get; set; } = string.Empty;
    public string Event { get; set; } = string.Empty;
    public string Award { get; set; } = string.Empty;
    public string Badge { get; set; } = "WINNER";
    public string? ProjectUrl { get; set; }

    public HackathonRecord() { }

    public HackathonRecord(string year, string @event, string award, string badge)
    {
        Year = year;
        Event = @event;
        Award = award;
        Badge = badge;
    }
}

/// <summary>
/// External profile link.
/// </summary>
public class SocialLink : SortableEntity
{
    public string Label { get; set; } = string.Empty;
    public string Handle { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public SocialLink() { }

    public SocialLink(string label, string handle, string url)
    {
        Label = label;
        Handle = handle;
        Url = url;
    }
}

#endregion

#region User Interaction & Analytics Models

/// <summary>
/// Form model bound to the contact form on UI.
/// </summary>
public class ContactFormModel
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Persisted contact message sent through the website terminal.
/// </summary>
public class ContactMessage : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public MessageStatus Status { get; set; } = MessageStatus.New;
    public string? SenderIp { get; set; }
    public string? UserAgent { get; set; }

    public void MarkAsRead() => Status = MessageStatus.Read;
    public void MarkAsArchived() => Status = MessageStatus.Archived;
}

/// <summary>
/// Lightweight visit / pageview analytics event (Association with optional Project).
/// </summary>
public class AnalyticsEvent : BaseEntity
{
    public string PagePath { get; set; } = string.Empty;
    public string? Referrer { get; set; }
    public string? CountryCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
}

#endregion