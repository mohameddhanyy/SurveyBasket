using SurveyBasket.Api.DTOs.Polls;
using SurveyBasket.Api.Presistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBasket.Api.ServiceContracts
{
    public interface IPollService
    {
        Task<Result<IEnumerable<PollResponse>>> GetAllAsync();
        Task<Result<PollResponse>> GetAsync(int id);

        Task<Result<PollResponse>> AddAsync(PollRequest poll);
        Task<Result> UpdateAsync(int id, PollRequest poll);

        Task<Result<PollResponse>> DeleteAsync(int id);
        Task<Result<PollResponse>> TogglePublishStatusAsync(int id);

    }
}
