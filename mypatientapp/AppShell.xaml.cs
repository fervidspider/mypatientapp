using mypatientapp.View;

namespace mypatientapp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(PatientDetails), typeof(PatientDetails));
        Routing.RegisterRoute(nameof(PatientCreate), typeof(PatientCreate));

        Routing.RegisterRoute(nameof(AppointmentDetails), typeof(AppointmentDetails));
        // Routing.RegisterRoute(nameof(AppointmentCreate), typeof(AppointmentCreate));

    }
}

