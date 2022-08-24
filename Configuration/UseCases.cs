using Ipe.UseCases;
using Ipe.UseCases.CheckEmail;
using Ipe.UseCases.ConfigurationPaymentPublicKeyUseCase;
using Ipe.UseCases.GetCitiesByState;
using Ipe.UseCases.GetStates;
using Ipe.UseCases.GetUserInfo;
using Ipe.UseCases.Login;
using Ipe.UseCases.PlantCustomizeUseCase;
using Ipe.UseCases.PlantUseCase.CreatePlant;
using Ipe.UseCases.Register;
using Ipe.UseCases.SendVerificationEmail;
using Ipe.UseCases.TreeUseCase.GetActiveTreeBiomes;
using Ipe.UseCases.TreeUseCase.GetTreesByFilter;
using Ipe.UseCases.UserPasswordChange;
using Ipe.UseCases.UserPasswordReset;
using Ipe.UseCases.ValidateEmail;

namespace Ipe.Configuration
{
	public static class UseCases
	{
		public static void Configure(WebApplicationBuilder builder)
        {
            #region Email
			builder.Services.AddScoped<IUseCase<ValidateEmailUseCaseInput, ValidateEmailUseCaseOutput>, ValidateEmailUseCase>();
			builder.Services.AddScoped<IUseCase<CheckEmailUseCaseInput, CheckEmailUseCaseOutput>, CheckEmailUseCase>();
			builder.Services.AddScoped<IUseCase<SendVerificationEmailUseCaseInput, SendVerificationEmailUseCaseOutput>, SendVerificationEmailUseCase>();
			builder.Services.AddScoped<IUseCase<UserPasswordResetUseCaseInput, UserPasswordResetUseCaseOutput>, UserPasswordResetUseCase>();
            #endregion

            #region User
            builder.Services.AddScoped<IUseCase<LoginUseCaseInput, LoginUseCaseOutput>, LoginUseCase>();
			builder.Services.AddScoped<IUseCase<UserRegisterUseCaseInput, UserRegisterUseCaseOutput>, UserRegisterUseCase>();
			builder.Services.AddScoped<IUseCase<UserPasswordChangeUseCaseInput, UserPasswordChangeUseCaseOutput>, UserPasswordChangeUseCase>();
			builder.Services.AddScoped<IUseCase<GetUserInfoUseCaseInput, GetUserInfoUseCaseOutput>, GetUserInfoUseCase>();
            #endregion

            #region Trees
			builder.Services.AddScoped<IUseCase<GetTreesByFilterUseCaseInput, GetTreesByFilterUseCaseOutput>, GetTreesByFilterUseCase>();
			builder.Services.AddScoped<IUseCase<GetActiveTreeBiomesUseCaseInput, GetActiveTreeBiomesUseCaseOutput>, GetActiveTreeBiomesUseCase>();
            #endregion

            #region States
            builder.Services.AddScoped<IUseCase<GetStatesUseCaseInput, GetStatesUseCaseOutput>, GetStatesUseCase>();
			builder.Services.AddScoped<IUseCase<GetCitiesByStateUseCaseInput, GetCitiesByStateUseCaseOutput>, GetCitiesByStateUseCase>();
            #endregion

			#region Plant
			builder.Services.AddScoped<IUseCase<PlantUseCaseInput, PlantUseCaseOutput>, PlantUseCase>();
			builder.Services.AddScoped<IUseCase<PlantCustomizeUseCaseInput, PlantCustomizeUseCaseOutput>, PlantCustomizeUseCase>();
            #endregion

            #region Configuration
            builder.Services.AddScoped<IUseCase<ConfigurationPaymentUseCaseInput, ConfigurationPaymentUseCaseOutput>, ConfigurationPaymentUseCase>();
            #endregion
        }
    }
}