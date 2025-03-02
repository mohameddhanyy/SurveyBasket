﻿namespace SurveyBasket.Api.DTOs.Requests
{
    public record PollRequest(
        string Title,
        string Summary,
        bool IsPublished,
        DateOnly StartsAt,
        DateOnly EndsAt);
}
