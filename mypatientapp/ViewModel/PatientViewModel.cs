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
        public ObservableCollection<Patient> filteredPatients { get; } = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotFiltered))]
        public bool isFiltered;

        public bool IsNotFiltered => !IsFiltered;


        [ObservableProperty]
        public string searchQuery;

        public Command GetPatientsCommand { get; }
        public Command NavigateToCommand { get; }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                FilterCollection();
                OnPropertyChanged(nameof(Patients));
            }
        }

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
        
        private void FilterCollection()
        {
            if (string.IsNullOrEmpty(searchText))
            {
                IsFiltered = false;
            }
            else
            {

                IsFiltered = true;

                var filPatients = new ObservableCollection<Patient>(Patients.Where(item =>
                item.firstname.ToLower().Contains(searchText.ToLower()) ||
                item.middlename.ToLower().Contains(searchText.ToLower()) ||
                item.lastname.ToLower().Contains(searchText.ToLower())
                ) );

                filteredPatients.Clear();

                foreach (var patient in filPatients)
                    filteredPatients.Add(patient);
            }
        }


    }

    
}

