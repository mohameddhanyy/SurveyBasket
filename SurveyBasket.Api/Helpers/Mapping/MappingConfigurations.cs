using Mapster;
using SurveyBasket.Api.DTOs.Requests;
using SurveyBasket.Api.DTOs.Responses;
using SurveyBasket.Api.Presistance.Models;

namespace SurveyBasket.Api.Helpers.Mapping
{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<PollRequest, Poll>()
            //    .Ignore(dest => dest.Id);
        }
    }
}
