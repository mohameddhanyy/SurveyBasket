using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Presistance;
using SurveyBasket.Api.Presistance.Models;
using SurveyBasket.Api.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PollService(SurveyBasketDBContext context) : IPollService
    {
        private readonly SurveyBasketDBContext _context = context;

        public async Task<Poll?> AddAsync(Poll poll)
        {
            await _context.Polls.AddAsync(poll);
            await _context.SaveChangesAsync();
            return poll;
        }

        public async Task<Poll?> GetAsync(int id)
        {
            var poll = await _context.Polls.FindAsync(id);
            return poll;
        }

        public async Task<IEnumerable<Poll>> GetAllAsync()
        {
            var polls = await _context.Polls.AsNoTracking().ToListAsync();
            return polls;
        }

        public async Task<bool> UpdateAsync(int id, Poll updatedPoll)
        {
            var poll = await GetAsync(id);
            if (poll is null) return false;

            poll.Summary = updatedPoll.Summary;
            poll.Title = updatedPoll.Title;
            poll.StartsAt = updatedPoll.StartsAt;
            poll.EndsAt = updatedPoll.EndsAt;

            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> DeleteAsync(int id)
        {
            var poll = await GetAsync(id);
            if (poll is null) return false;

            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> TogglePublishStatusAsync(int id)
        {
            var poll = await GetAsync(id);
            if (poll is null) return false;

            poll.IsPublished = !poll.IsPublished;
            await _context.SaveChangesAsync();
            return true;


        }
    }
}
