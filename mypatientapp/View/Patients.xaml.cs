using mypatientapp.ViewModel;
using mypatientapp.Model;
using AndroidX.Lifecycle;

namespace mypatientapp.View;

public partial class Patients : ContentPage
{

    
    public Patients(PatientViewModel patientsViewModel)
	{

		InitializeComponent();
        BindingContext = patientsViewModel;

    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        var patient = ((VisualElement)sender).BindingContext as Patient;

        if (patient == null)
            return;
        

        await Shell.Current.GoToAsync(nameof(PatientDetails), true, new Dictionary<string, object>
        {
            {"passedpatient", patient }
        });
        
    }

}
