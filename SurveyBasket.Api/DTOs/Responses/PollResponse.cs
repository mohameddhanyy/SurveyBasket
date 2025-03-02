﻿namespace SurveyBasket.Api.DTOs.Responses
{
    public record PollResponse(
        int Id,
        string Title,
        string Summary,
        bool IsPublished,
        DateOnly StartsAt,
        DateOnly EndsAt);
}
