using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using mypatientapp.Model;
using mypatientapp.Services;

namespace mypatientapp.ViewModel
{

    [QueryProperty(nameof(Passedpatient), "passedpatient")]
    public partial class PatientDetailsViewModel : BaseViewModel
	{

        [ObservableProperty]
        public Patient passedpatient;

        [ObservableProperty]
        public Patient patient = new();

        PatientService patientService;

        public PatientDetailsViewModel(PatientService patientService)
		{
            this.patientService = patientService;

        }

        partial void OnPassedpatientChanged(Patient value)
        {

            GetPatientsAsync(Passedpatient.Id);

        }

        async Task GetPatientsAsync(string id)
        {

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var pulledpatient = await patientService.GetPatientById(id);

                Patient = pulledpatient;                

            }
            catch (Exception ex)
            {

                Debug.WriteLine($"Unable to retrieve patients {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");

            }
            finally
            {
                IsBusy = false;
            }


        }



    }
}

