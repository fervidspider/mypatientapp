using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using mypatientapp.Model;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using mypatientapp.Services;
using mypatientapp.View;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace mypatientapp.ViewModel
{
    public partial class PatientViewModel : BaseViewModel
    {

        public ObservableCollection<Patient> Patients { get; } = new();

        [ObservableProperty]
        public int pagenum = 1;
        [ObservableProperty]
        public int skip = 0;

        [ObservableProperty]
        public string searchQuery;

        public Command GetPatientsCommand { get; }
        public Command NavigateToCommand { get; }
        public Command IncreasePageCommand { get; }
        public Command DecreasePageCommand { get; }

        PatientService patientService;




        public PatientViewModel(PatientService patientService)
        {
            this.patientService = patientService;
            GetPatientsCommand = new Command(async () => await GetPatientsAsync());
            NavigateToCommand = new Command(() => NavigateToCreate());
            IncreasePageCommand = new Command(() => IncreasePage());
            DecreasePageCommand = new Command(() => DecreasePage());

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

                var patients = await patientService.GetPatients(Skip);

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

        private async void IncreasePage()
        {

            Pagenum += 1;
            Skip += 18;

            await GetPatientsAsync();
        }

        private async void DecreasePage()
        {
            if (Skip == 0)
                return;

            Pagenum -= 1;
            Skip -= 18;

            await GetPatientsAsync();
        }


    }

    
}

