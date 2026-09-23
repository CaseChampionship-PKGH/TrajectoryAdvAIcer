using FluentAssertions;
using TrajectoryAdvAIcer.Entities.Enums;
using TrajectoryAdvAIcer.Entities.Models;
using TrajectoryAdvAIcer.Parsing.Excel;
using Xunit;

namespace TrajectoryAdvAIcer.Parsing.Tests;

/// <summary>
/// Регрессионные тесты Excel-парсера истории обучения.
/// </summary>
public class LearningHistoryExcelParserTests
{
    private readonly LearningHistoryExcelParser parser = new();

    [Fact]
    public async Task Parse_FileWithAuxiliaryFirstSheet_ShouldParseDataFromDataSheet()
    {
        using var stream = OpenDataFile("Массив данных к кейсу 3.xlsx");

        var result = await parser.ParseAsync<LearningHistory>(stream);

        result.Data.Records.Should().HaveCount(1314);
        result.Data.Employees.Should().HaveCount(323);
        result.Data.Records.Should().OnlyContain(r => !string.IsNullOrWhiteSpace(r.CourseTitle));
    }

    [Fact]
    public async Task Parse_FileWithoutTypeColumn_ShouldUseEkTypeAndEmitWarning()
    {
        using var stream = OpenDataFile("Массив данных к кейсу 3.xlsx");

        var result = await parser.ParseAsync<LearningHistory>(stream);

        result.Warnings.Should().Contain(w => w.Contains("Тип", StringComparison.OrdinalIgnoreCase));
        result.Data.Records.Should().OnlyContain(r => r.Type == CourseType.EK);
    }

    [Fact]
    public async Task Parse_OriginalExcelFile_ShouldKeepWorking()
    {
        using var stream = OpenDataFile("выгрузка с историей обучения.xlsx");

        var result = await parser.ParseAsync<LearningHistory>(stream);

        result.Data.Records.Should().HaveCount(1314);
        result.Data.Employees.Should().HaveCount(323);
        result.Data.Records.Should().Contain(r => r.Type == CourseType.PPK);
        result.Data.Records.Should().Contain(r => r.Type == CourseType.EK);
        result.Warnings.Should().BeEmpty();
    }

    private static Stream OpenDataFile(string name) =>
        File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", name));
}