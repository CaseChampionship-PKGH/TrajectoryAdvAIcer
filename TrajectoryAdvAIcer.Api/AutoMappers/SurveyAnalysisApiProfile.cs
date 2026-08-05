using AutoMapper;
using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Api.Models;

namespace TrajectoryAdvAIcer.Api.AutoMappers;

/// <inheritdoc />
public class SurveyAnalysisApiProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SurveyAnalysisApiProfile"/>
    /// </summary>
    public SurveyAnalysisApiProfile()
    {
        CreateMap<AnalysisResult, AnalysisResultApiModel>(MemberList.Source)
            .ForMember(x => x.Errors, opt => opt.Ignore())
            .ReverseMap();
    }
}
