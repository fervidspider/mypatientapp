using mypatientapp.View;

namespace mypatientapp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(PatientDetails), typeof(PatientDetails));
	}
}

