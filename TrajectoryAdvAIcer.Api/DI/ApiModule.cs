using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using TrajectoryAdvAIcer.Agent;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Analysis;
using TrajectoryAdvAIcer.Api.AutoMappers;
using TrajectoryAdvAIcer.Common.Mvc.Extensions;
using TrajectoryAdvAIcer.Parsing;
using TrajectoryAdvAIcer.Parsing.Archive;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;
using TrajectoryAdvAIcer.Parsing.Csv;
using TrajectoryAdvAIcer.Parsing.Excel;
using TrajectoryAdvAIcer.Parsing.Json;
using TrajectoryAdvAIcer.Reporting;
using TrajectoryAdvAIcer.Reporting.Contracts.Interfaces;
using TrajectoryAdvAIcer.Services;
using TrajectoryAdvAIcer.Validation;
using Module = TrajectoryAdvAIcer.Common.Mvc.Module;

namespace TrajectoryAdvAIcer.Api.DI;

/// <inheritdoc />
public class ApiModule : Module
{
    /// <inheritdoc />
    protected override void Load(IServiceCollection services)
    {
        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddSingleton<LearningHistoryCsvParser>();
        services.AddSingleton<LearningHistoryExcelParser>();
        services.AddSingleton<LearningHistoryJsonParser>();
        services.AddSingleton<LearningHistoryArchiveParser>();
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, LearningHistoryCsvParser>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, LearningHistoryExcelParser>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, LearningHistoryJsonParser>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, LearningHistoryArchiveParser>(ServiceLifetime.Singleton);

        services.AddSingleton<CourseCatalogCsvParser>();
        services.AddSingleton<CourseCatalogExcelParser>();
        services.AddSingleton<CourseCatalogJsonParser>();
        services.AddSingleton<CourseCatalogArchiveParser>();
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, CourseCatalogCsvParser>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, CourseCatalogExcelParser>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, CourseCatalogJsonParser>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, CourseCatalogArchiveParser>(ServiceLifetime.Singleton);

        services.RegisterMultipleInterfacesAssignableTo<IDataParser, AgentResponseJsonParser>(ServiceLifetime.Singleton);

        services.AddHttpClient("RussianLLMAccessToken", client =>
        {
            client.BaseAddress = new Uri(config["RussianLLM:TokenUrl"]!);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
            {
                if (config.GetSection("RussianLLM").GetValue("BypassSsl", false)! == true)
                {
                    return new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                    };
                }
                else
                {
                    return new HttpClientHandler();
                }
            });

        services.AddHttpClient("RussianLLM", client =>
        {
            client.BaseAddress = new Uri(config["RussianLLM:BaseUrl"]!);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            if (config.GetSection("RussianLLM").GetValue("BypassSsl", false)! == true)
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
            }
            else
            {
                return new HttpClientHandler();
            }
        });

        services.RegisterMultipleInterfacesAssignableTo<ILlmClient, MockLlmClient>(ServiceLifetime.Singleton);

        services.AddHttpClient("ForeignLLM", client =>
        {
            client.BaseAddress = new Uri(config["ForeignLLM:BaseUrl"]!);
        });
        services.RegisterMultipleInterfacesAssignableTo<ILlmClient, MockLlmClient>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<ParserFactory>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<FormatDetector>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<DataValidator>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<CollaborativeFilteringService>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<CourseCandidateSelector>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<TopCourseSelector>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<DefaultPromptProvider>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<LlmFactory>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<LlmTrajectoryAdvicerAgent>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IReportExporter, ExcelReportExporter>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<TrajectoryAnalysisPipeline>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<ReportExporterFactory>(ServiceLifetime.Singleton);
        services.RegisterAutoMapperProfile<TrajectoryAnalysisApiProfile>();
        RegisterAutoMapper(services);
        services.AddHttpContextAccessor();
    }

    private static void RegisterAutoMapper(IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            var profiles = provider.GetServices<Profile>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                foreach (var profile in profiles)
                {
                    mc.AddProfile(profile);
                }
            }, NullLoggerFactory.Instance);
            var mapper = mapperConfig.CreateMapper();
            return mapper;
        });
    }
}
