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

        public Command DeletePatientCommand { get; }

        public PatientDetailsViewModel(PatientService patientService)
		{
            this.patientService = patientService;

            DeletePatientCommand = new Command(async () => await DeletePatientAsync(Patient.Id));

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

        async Task DeletePatientAsync(string id)
        {

            var answer = await Application.Current.MainPage.DisplayAlert(
                "Confirm Deletion!",
                $"{Patient.Id}\n{Patient.firstname} {Patient.lastname}\n",
                "Delete", "Cancel");

            if (answer == false)
                return;

            else
            {

                if (IsBusy)
                    return;

                try
                {
                    IsBusy = true;

                    await patientService.DeletePatientById(id);

                    await Application.Current.MainPage.DisplayAlert("Success!", $"Patient \"{id}\" Succesfully Deleted", "OK");

                    await Shell.Current.GoToAsync("..");

                }
                catch (Exception ex)
                {

                    Debug.WriteLine($"Unable to retrieve patients {ex.Message}");
                    await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");

                }
                finally
                {
                    IsBusy = false;
                    Console.WriteLine("@DeletePatientAsync - Finally");
                }

            }
            

            


        }



    }
}

