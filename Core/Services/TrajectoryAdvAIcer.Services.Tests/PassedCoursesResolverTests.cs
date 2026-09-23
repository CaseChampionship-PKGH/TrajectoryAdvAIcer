using FluentAssertions;
using TrajectoryAdvAIcer.Entities.Enums;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Contracts.Helpers;
using Xunit;

namespace TrajectoryAdvAIcer.Services.Tests;

/// <summary>
/// Тесты резолвинга пройденных курсов сотрудника.
/// </summary>
public class PassedCoursesResolverTests
{
    [Fact]
    public void Resolve_PassedRecordsFromCatalogAndUnknown_ShouldReturnBothTitlesInOrder()
    {
        const string employeeId = "emp-1";
        const string catalogTitle = "Каталог-курс";
        const string unknownTitle = "2024_06_05-2024_06_25_Курс вне справочника (ГЗ)(ОДО)";
        var catalog = new CourseCatalog
        {
            Courses = [new Course { Id = StableIdProvider.GenerateStableId(catalogTitle), Title = catalogTitle }]
        };
        var records = new List<LearningRecord>
        {
            NewRecord(employeeId, catalogTitle, CompletionStatus.Passed),
            NewRecord(employeeId, unknownTitle, CompletionStatus.Passed)
        };

        var result = PassedCoursesResolver.ResolvePassedCourses(employeeId, records, catalog);

        result.Should().Equal(catalogTitle, unknownTitle);
    }

    [Fact]
    public void Resolve_OnlyNotPassedRecords_ShouldReturnEmpty()
    {
        const string employeeId = "emp-2";
        const string title = "Курс";
        var catalog = new CourseCatalog
        {
            Courses = [new Course { Id = StableIdProvider.GenerateStableId(title), Title = title }]
        };
        var records = new List<LearningRecord>
        {
            NewRecord(employeeId, title, CompletionStatus.NotPassed),
            NewRecord(employeeId, title, CompletionStatus.InProgress)
        };

        var result = PassedCoursesResolver.ResolvePassedCourses(employeeId, records, catalog);

        result.Should().BeEmpty();
    }

    [Fact]
    public void Resolve_DuplicatePassedRecords_ShouldReturnUniqueTitles()
    {
        const string employeeId = "emp-3";
        const string title = "Курс";
        var catalog = new CourseCatalog
        {
            Courses = [new Course { Id = StableIdProvider.GenerateStableId(title), Title = title }]
        };
        var records = new List<LearningRecord>
        {
            NewRecord(employeeId, title, CompletionStatus.Passed),
            NewRecord(employeeId, title, CompletionStatus.Passed)
        };

        var result = PassedCoursesResolver.ResolvePassedCourses(employeeId, records, catalog);

        result.Should().ContainSingle().Which.Should().Be(title);
    }

    [Fact]
    public void Resolve_RecordsOfOtherEmployees_ShouldBeIgnored()
    {
        const string title = "Курс";
        var catalog = new CourseCatalog
        {
            Courses = [new Course { Id = StableIdProvider.GenerateStableId(title), Title = title }]
        };
        var records = new List<LearningRecord>
        {
            NewRecord("emp-4", title, CompletionStatus.Passed)
        };

        var result = PassedCoursesResolver.ResolvePassedCourses("emp-5", records, catalog);

        result.Should().BeEmpty();
    }

    private static LearningRecord NewRecord(string employeeId, string courseTitle, CompletionStatus status) => new()
    {
        EmployeeId = employeeId,
        CourseId = StableIdProvider.GenerateStableId(courseTitle),
        CourseTitle = courseTitle,
        Type = CourseType.EK,
        Status = status
    };
}