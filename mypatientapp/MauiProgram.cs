using Microsoft.Extensions.Logging;
using mypatientapp.View;
using mypatientapp.ViewModel;
using mypatientapp.Services;

namespace mypatientapp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddTransient<Dashboard>();

        builder.Services.AddTransient<Patients>();
        builder.Services.AddTransient<PatientViewModel>();

        builder.Services.AddTransient<PatientDetails>();
        builder.Services.AddTransient<PatientDetailsViewModel>();

        builder.Services.AddTransient<PatientCreate>();
        builder.Services.AddTransient<PatientCreateViewModel>();

        builder.Services.AddSingleton<PatientService>();


        return builder.Build();
	}
}

