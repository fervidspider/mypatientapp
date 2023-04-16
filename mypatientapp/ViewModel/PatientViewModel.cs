using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using mypatientapp.Model;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using mypatientapp.Services;
using mypatientapp.View;
using System.Diagnostics;

namespace mypatientapp.ViewModel
{
    public class PatientViewModel : BaseViewModel
    {

        public ObservableCollection<Patient> Patients { get; } = new();
        public Command GetPatientsCommand { get; }
        public Command NavigateToCommand { get; }
        PatientService patientService;

        public PatientViewModel(PatientService patientService)
        {
            this.patientService = patientService;
            GetPatientsCommand = new Command(async () => await GetPatientsAsync());
            NavigateToCommand = new Command(() => NavigateToCreate());

            Title = "Patients"; 

            GetPatientsAsync();

        }

        private async void NavigateToCreate()
        {

            await Shell.Current.GoToAsync(nameof(PatientCreate), true);

        }

        public async Task GetPatientsAsync()
        {

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var patients = await patientService.GetPatients();

                if (Patients.Count != 0)
                    Patients.Clear();

                foreach (var patient in patients)
                    Patients.Add(patient);


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

