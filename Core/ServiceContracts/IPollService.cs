using Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ServiceContracts
{
    public interface IPollService
    {
        IEnumerable<Poll> GetAll();
        Poll? Get(int id);

        Poll? Add(Poll poll);

        bool Update(int id, Poll poll);
        bool Delete(int id);
    }
}
