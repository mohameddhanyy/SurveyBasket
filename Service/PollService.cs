using Core.Models;
using Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PollService : IPollService
    {
        private static readonly List<Poll> _polls = [
      new Poll(){
            Id = 1,
            Name = "Poll 1",
            Description = "this first poll"
        }];

        public Poll? Add(Poll poll)
        {
            poll.Id += _polls.Count;
            _polls.Add(poll);
            return poll; 
        }


        public Poll? Get(int id) => _polls.SingleOrDefault(x => x.Id == id);

        public IEnumerable<Poll> GetAll() => _polls;

        public bool Update(int id, Poll poll)
        {
            var currentPoll = Get(id);
            if (currentPoll is null) return false;

            currentPoll.Description = poll.Description;
            currentPoll.Name = poll.Name;

            return true; 

        }
        public bool Delete(int id)
        {
            var currentPoll = Get(id);
            if (currentPoll is null) return false;

            _polls.Remove(currentPoll);
            return true;
        }
    }
}
