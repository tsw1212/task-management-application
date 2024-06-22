

namespace DO;

// decleration of the entity task
public record Task
    (
    int Id,
    string Alias,
    string Description,
    DateTime CreatedAtDate,
    TimeSpan? RequierdEffortTime = null,
    int? EngineerId = null,
    bool IsMilestone = false,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadLineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    EngineerExperience‏? ComplexityLevel = EngineerExperience.Novice

    )


{


    public Task() : this(0, "", "", DateTime.Now,null, null, false, null, null, null, null, null, null, EngineerExperience.Novice) { }

}
