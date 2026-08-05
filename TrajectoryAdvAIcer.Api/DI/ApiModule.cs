using AutoMapper;
using TrajectoryAdvAIcer.Agent;
using TrajectoryAdvAIcer.Agent.Contracts.Interfaces;
using TrajectoryAdvAIcer.Agent.GigaChat;
using TrajectoryAdvAIcer.Agent.OpenAI;
using TrajectoryAdvAIcer.Analysis;
using TrajectoryAdvAIcer.Api.AutoMappers;
using TrajectoryAdvAIcer.Common.Mvc.Extensions;
using TrajectoryAdvAIcer.Parsing;
using TrajectoryAdvAIcer.Parsing.Contracts.Interfaces;
using TrajectoryAdvAIcer.Parsing.Csv;
using TrajectoryAdvAIcer.Parsing.Excel;
using TrajectoryAdvAIcer.Parsing.Json;
using TrajectoryAdvAIcer.Reporting;
using TrajectoryAdvAIcer.Reporting.Contracts.Interfaces;
using TrajectoryAdvAIcer.Services;
using TrajectoryAdvAIcer.Validation;
using Microsoft.Extensions.Logging.Abstractions;
using Module = TrajectoryAdvAIcer.Common.Mvc.Module;

namespace TrajectoryAdvAIcer.Api.DI;

/// <inheritdoc />
public class ApiModule : Module
{
    /// <inheritdoc />
    protected override void Load(IServiceCollection services)
    {
        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.RegisterMultipleInterfacesAssignableTo<IDataParser, SurveyCsvParser>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, SurveyExcelParser>(ServiceLifetime.Singleton);
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

        services.RegisterMultipleInterfacesAssignableTo<ILlmClient, GigaChatLlmClient>(ServiceLifetime.Singleton);

        services.AddHttpClient("ForeignLLM", client =>
        {
            client.BaseAddress = new Uri(config["ForeignLLM:BaseUrl"]!);
        });
        services.RegisterMultipleInterfacesAssignableTo<ILlmClient, OpenAiCompatibleLlmClient>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<ParserFactory>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<FormatDetector>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<StatisticsCalculator>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<SurveyAnalysisPipeline>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<DataValidator>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<SurveyAggregator>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<DefaultPromptProvider>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<LlmFactory>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<LlmSurveyAnalysisAgent>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IReportExporter, ExcelReportExporter>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IReportExporter, WordReportExporter>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<ReportExporterFactory>(ServiceLifetime.Singleton);
        services.RegisterAutoMapperProfile<SurveyAnalysisApiProfile>();
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
