using mypatientapp.ViewModel;

namespace mypatientapp.View;

public partial class Patients : ContentPage
{


    public Patients(PatientViewModel patientsViewModel)
	{

		InitializeComponent();
        BindingContext = patientsViewModel;


    }

}
