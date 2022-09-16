using Ipe.Domain.Models;
using Ipe.Domain.Errors;
using Ipe.UseCases.Interfaces;
using BCryptLib = BCrypt.Net.BCrypt;
using Ipe.Domain;
using Ipe.UseCases.Login;
using Ipe.UseCases.Interfaces.Services;
using Ipe.External.Services.DTOs;
using Ipe.UseCases.Register;
using Ipe.UseCases.Interfaces.Repositories;

namespace Ipe.UseCases.GoogleLogin
{
    public class GoogleLoginUseCase : IUseCase<GoogleLoginUseCaseInput, LoginUseCaseOutput>
    {

        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IGoogleService _googleService;
        private readonly IPlantRepository _plantRepository;

        public GoogleLoginUseCase(
            IAuthService authService,
            IUserRepository userRepository,
            IGoogleService googleService,
            IPlantRepository plantRepository
        )
        {
            _authService = authService;
            _userRepository = userRepository;
            _googleService = googleService;
            _plantRepository = plantRepository;
        }

        public async Task<LoginUseCaseOutput> Run(GoogleLoginUseCaseInput Input)
        {

            var GoogleUser = await _googleService.GetUserInfoByGoogleToken(Input.Token);

            if(GoogleUser.Email is null)
            {
                throw new InvalidEmailException();
            }

            var User = await _userRepository.GetByEmail(GoogleUser.Email);

            if (User is null)
            {
                await CreateUser(GoogleUser);
                User = await _userRepository.GetByEmail(GoogleUser.Email);
                await RecoveryTrees(User);
            }

            if (User.EmailVerified is false)
            {
                await UpdateUserWithGoogleData(GoogleUser, User);
                User = await _userRepository.GetByEmail(GoogleUser.Email);
            }

            if(GoogleUser.Picture is not null && GoogleUser.Picture != "" && GoogleUser.Picture != User.Photo)
            {
                User.Photo = GoogleUser.Picture;
                await UpdateUserPhoto(User);
            }

            ValidateUser(GoogleUser, User);

            return BuildResponse(User);
        }

        private async Task RecoveryTrees(User User)
        {
            await _plantRepository.RecoveryPlants(User.Id, User.Email);
        }

        private async Task CreateUser(GoogleUserResponse GoogleUser)
        {
            await _userRepository.Create(new User
            {
                Email = GoogleUser.Email,
                Name = GoogleUser.Name,
                Password = BCryptLib.HashPassword(new Random().Next().ToString()),
                EmailVerified = true,
                Origin = Origins.Google.ToString(),
                Photo = GoogleUser.Picture
            });
        }

        private static void ValidateUser(GoogleUserResponse GoogleUser, User User)
        {
            if (GoogleUser.EmailVerified == "false")
                throw new UnverifiedEmailException();

            if (User is null)
                throw new InvalidPasswordException();

            if (User.EmailVerified is false)
                throw new UnverifiedEmailException();
        }

        private async Task UpdateUserPhoto(User User)
        {
            await _userRepository.Update(User);
        }

        private LoginUseCaseOutput BuildResponse(User User)
        {
            return new LoginUseCaseOutput
            {
                AccessToken = _authService.GenerateToken(User, Roles.User),
                TokenType = "Bearer",
                User = new OutputUser
                {
                    Id = User.Id,
                    Email = User.Email,
                    Name = User.Name,
                    Photo = User.Photo
                }
            };
        }

        private async Task UpdateUserWithGoogleData(GoogleUserResponse GoogleUser, User User)
        {
            User.EmailVerified = true;
            User.Origin = Origins.Google.ToString();
            await _userRepository.Update(User);
        }
    }
}