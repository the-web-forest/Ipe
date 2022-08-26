using System;
using Ipe.External.Services.DTOs;

namespace Ipe.UseCases.Interfaces.Services
{
    public interface IGoogleService
    {
        Task<GoogleUserResponse> GetUserInfoByGoogleToken(string GoogleToken);
    }
}

