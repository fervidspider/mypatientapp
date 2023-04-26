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
        builder.Services.AddTransient<DashboardViewModel>();

        // Patient Pages

        builder.Services.AddTransient<Patients>();
        builder.Services.AddTransient<PatientViewModel>();

        builder.Services.AddTransient<PatientDetails>();
        builder.Services.AddTransient<PatientDetailsViewModel>();

        builder.Services.AddTransient<PatientCreate>();
        builder.Services.AddTransient<PatientCreateViewModel>();

        // Appointment Pages

        builder.Services.AddTransient<Appointments>();
        builder.Services.AddTransient<AppointmentViewModel>();

        builder.Services.AddTransient<AppointmentDetails>();
        builder.Services.AddTransient<AppointmentDetailsViewModel>();

        // Services

        builder.Services.AddSingleton<PatientService>();
        builder.Services.AddSingleton<AppointmentService>();


        return builder.Build();
	}
}

