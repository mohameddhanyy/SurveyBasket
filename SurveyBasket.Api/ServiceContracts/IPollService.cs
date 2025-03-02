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
        Task<IEnumerable<Poll>> GetAllAsync();
        Task<Poll?> GetAsync(int id);

        Task<Poll?> AddAsync(Poll poll);

        Task<bool> UpdateAsync(int id, Poll poll);
        Task<bool> DeleteAsync(int id);
        Task<bool> TogglePublishStatusAsync(int id);

    }
}
