using mypatientapp.ViewModel;

namespace mypatientapp.View;

public partial class PatientCreate : ContentPage
{
	public PatientCreate(PatientCreateViewModel patientCreateViewModel)
	{
        InitializeComponent();
        BindingContext = patientCreateViewModel;
    }
}
