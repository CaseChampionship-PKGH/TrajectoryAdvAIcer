using AutoMapper;
using TrajectoryAdvAIcer.Analysis.Contracts.Models;
using TrajectoryAdvAIcer.Api.Models;

namespace TrajectoryAdvAIcer.Api.AutoMappers;

/// <inheritdoc />
public class TrajectoryAnalysisApiProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TrajectoryAnalysisApiProfile"/>
    /// </summary>
    public TrajectoryAnalysisApiProfile()
    {
        CreateMap<AnalysisResult, AnalysisResultApiModel>(MemberList.Source)
            .ForMember(x => x.Errors, opt => opt.Ignore())
            .ReverseMap();
    }
}
