using TrajectoryAdvAIcer.Analysis.Contracts.Interfaces;
using TrajectoryAdvAIcer.Analysis.Contracts.Models;

namespace TrajectoryAdvAIcer.Analysis;

/// <inheritdoc cref="ITopCourseSelector"/>
public class TopCourseSelector : ITopCourseSelector
{
    List<string> ITopCourseSelector.SelectTopCourses(
        List<CourseCandidate> candidates,
        int similarEmployeesCount,
        double popularityThreshold,
        int maxCourses)
        => similarEmployeesCount <= 0 ? []
            : candidates
            .Where(c => (double)c.Popularity / similarEmployeesCount >= popularityThreshold)
            .Take(maxCourses)
            .Select(c => c.CourseId)
            .ToList();
}
