using TrajectoryAdvAIcer.Analysis.Contracts.Interfaces;
using TrajectoryAdvAIcer.Analysis.Contracts.Models.Aggregation;
using TrajectoryAdvAIcer.Entities.Enums;
using TrajectoryAdvAIcer.Entities.Models;

namespace TrajectoryAdvAIcer.Analysis;

/// <inheritdoc cref="ICourseCandidateSelector"/>
public class CourseCandidateSelector : ICourseCandidateSelector
{
    private readonly ICollaborativeFilteringService collaborativeFilteringService;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CourseCandidateSelector"/>
    /// </summary>
    public CourseCandidateSelector(ICollaborativeFilteringService collaborativeFilteringService)
    {
        this.collaborativeFilteringService = collaborativeFilteringService;
    }

    List<CourseCandidate> ICourseCandidateSelector.SelectCandidates(LearningHistory history, string employeeId)
    {
        var popularityMap = collaborativeFilteringService.BuildPopularityMapForEmployee(history, employeeId);

        var passedCourseIds = history.Records
            .Where(r => r.EmployeeId == employeeId && r.Status == CompletionStatus.Passed)
            .Select(r => r.CourseId)
            .ToHashSet();

        return popularityMap
            .Where(kv => !passedCourseIds.Contains(kv.Key))
            .Select(kv => new CourseCandidate
            {
                CourseId = kv.Key,
                Popularity = kv.Value
            })
            .OrderByDescending(c => c.Popularity)
            .ToList();
    }
}
