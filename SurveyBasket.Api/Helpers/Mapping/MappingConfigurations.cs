using Core.Models;
using Mapster;
using SurveyBasket.Api.DTOs.Responses;

namespace SurveyBasket.Api.Helpers.Mapping
{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<Poll, PollResponse>()
            //    .Map(dest => dest.Notes, src => src.Description);
        }
    }
}
