using Mapster;
using SurveyBasket.Api.Presistance.Models;

namespace SurveyBasket.Api.Helpers.Mapping
{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<QuestionRequest, Question>()
                .Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer { Content = answer }));

        }
    }
}
