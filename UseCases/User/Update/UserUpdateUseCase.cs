using System;
using Ipe.Domain.Models;
using Ipe.UseCases.Interfaces;

namespace Ipe.UseCases.Update
{
    public class UserUpdateUseCase : IUseCase<UserUpdateUseCaseInput, UserUpdateUseCaseOutput>
    {
        private readonly IUserRepository _userRepository;

        public UserUpdateUseCase(
            IUserRepository userRepository

        )
        {
            _userRepository = userRepository;
        }

        public async Task<UserUpdateUseCaseOutput> Run(UserUpdateUseCaseInput Input)
        {
            var User = await _userRepository.GetById(Input.Id);
            User = UpdateUser(Input, User);
            await _userRepository.Update(User);
            return new UserUpdateUseCaseOutput();
        }

        private User UpdateUser(UserUpdateUseCaseInput Input, User User)
        {
            User.Name = Input.Name;
            User.City = Input.City;
            User.State = Input.State;
            User.AllowNewsletter = Input.AllowNewsletter;
            return User;
        }
    }
}

