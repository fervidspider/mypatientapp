using mypatientapp.ViewModel;
using mypatientapp.Model;

namespace mypatientapp.View;

public partial class Appointments : ContentPage
{

    
    public Appointments(AppointmentViewModel appointmentViewModel)
	{

		InitializeComponent();
        BindingContext = appointmentViewModel;

    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        var appointment = ((VisualElement)sender).BindingContext as Appointment;

        if (appointment == null)
            return;
        

        await Shell.Current.GoToAsync(nameof(AppointmentDetails), true, new Dictionary<string, object>
        {
            {"passedappointment", appointment }
        });
        
    }

}
