using mypatientapp.ViewModel;

namespace mypatientapp.View;



public partial class PatientDetails : ContentPage
{
    

    public PatientDetails(PatientDetailsViewModel patientDetailsViewModel)
	{
		InitializeComponent();
		BindingContext = patientDetailsViewModel;
	}
}
