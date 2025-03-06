﻿using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Errors;
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


        public async Task<Result<PollResponse>> GetAsync(int id)
        {
            var poll = await _context.Polls.SingleOrDefaultAsync(x => x.Id == id);

            return poll is not null
                ? Result.Success(poll.Adapt<PollResponse>())
                : Result.Failure<PollResponse>(PollErrors.NoPollFound);
        }

        public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync()
        {
            var polls = await _context.Polls.AsNoTracking().ToListAsync();

            return polls is not null
                ? Result.Success(polls.Adapt<IEnumerable<PollResponse>>())
                : Result.Failure<IEnumerable<PollResponse>>(PollErrors.NoPollFound);
        }

        public async Task<Poll?> AddAsync(PollRequest poll)
        {
            await _context.Polls.AddAsync(poll.Adapt<Poll>());
            await _context.SaveChangesAsync();
            return poll.Adapt<Poll>();
        }


        public async Task<Result> UpdateAsync(int id, PollRequest updatedPollRequest)
        {
            var existingPoll = await _context.Polls.FindAsync(id);
            if (existingPoll is null)
            {
                return Result.Failure(PollErrors.NoPollFound);
            }

            existingPoll.Summary = updatedPollRequest.Summary;
            existingPoll.Title = updatedPollRequest.Title;
            existingPoll.StartsAt = updatedPollRequest.StartsAt;
            existingPoll.EndsAt = updatedPollRequest.EndsAt;

            await _context.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result<PollResponse>> DeleteAsync(int id)
        {
            var pollResult = await _context.Polls.SingleOrDefaultAsync(x => x.Id == id);
            if(pollResult is null)
            {
                return Result.Failure<PollResponse>(PollErrors.NoPollFound);
            }

            _context.Polls.Remove(pollResult);
            await _context.SaveChangesAsync();

            var pollResponse = pollResult.Adapt<PollResponse>();
            return Result.Success(pollResponse);
        }

        public async Task<Result<PollResponse>> TogglePublishStatusAsync(int id)
        {
            var pollResult = await _context.Polls.SingleOrDefaultAsync(x => x.Id == id);
            if (pollResult is null)
            {
                return Result.Failure<PollResponse>(PollErrors.NoPollFound);
            }
            pollResult.IsPublished = !pollResult.IsPublished;
            await _context.SaveChangesAsync();
            var pollResponse = pollResult.Adapt<PollResponse>();
            return Result.Success(pollResponse);
        }
    }

}
