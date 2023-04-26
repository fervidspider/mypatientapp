using System.ComponentModel;
using mypatientapp.ViewModel;

namespace mypatientapp.View;



public partial class AppointmentDetails : ContentPage
{
    

    public AppointmentDetails(AppointmentDetailsViewModel appointmentDetailsViewModel)
	{
		InitializeComponent();
		BindingContext = appointmentDetailsViewModel;
	}

    void DatePicker_DateSelected(System.Object sender, Microsoft.Maui.Controls.DateChangedEventArgs e)
    {
        var viewModel = (AppointmentDetailsViewModel)BindingContext;
        viewModel.AppDate = e.NewDate;

    }

    void TimePicker_PropertyChanged(System.Object sender, PropertyChangedEventArgs e)
    {
        var viewModel = (AppointmentDetailsViewModel)BindingContext;
        var timePicker = (TimePicker)sender;

        viewModel.AppTime = timePicker.Time;
    }



}
