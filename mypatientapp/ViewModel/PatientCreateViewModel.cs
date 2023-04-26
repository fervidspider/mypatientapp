using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using mypatientapp.Model;
using mypatientapp.Services;

namespace mypatientapp.ViewModel
{
	public partial class PatientCreateViewModel : BaseViewModel
	{

        // Patient

        [ObservableProperty]
        public string firstname;
        [ObservableProperty]
        public string middlename;
        [ObservableProperty]
        public string lastname;
        [ObservableProperty]
        public DateTime dateofbirth;
        [ObservableProperty]
        public int height;
        [ObservableProperty]
        public int weight;
        [ObservableProperty]
        public string notes;

        // Collections / Lists

        public ObservableCollection<String> _mental { get; } = new();
        public ObservableCollection<String> _physical { get; } = new();
        public ObservableCollection<String> _medication { get; } = new();

        // Address

        [ObservableProperty]
        public string firstline;
        [ObservableProperty]
        public string secondline;
        [ObservableProperty]
        public string postcode;
        [ObservableProperty]
        public string city;
        [ObservableProperty]
        public string county;

        // Services

        PatientService patientService;

        // Commands

        public Command CreatePatientCommand { get; }
        public Command AddMentalDisabilityCommand { get; }
        public Command AddPhysicalDisabilityCommand { get; }
        public Command AddMedicationCommand { get; }


        public PatientCreateViewModel(PatientService patientService)
		{

            this.patientService = patientService;

            Dateofbirth = DateTime.Today;

            CreatePatientCommand = new Command(async () => await CreatePatient());

            AddMentalDisabilityCommand = new Command(async () => await AddMentalDisability());
            AddPhysicalDisabilityCommand = new Command(async () => await AddPhysicalDisability());
            AddMedicationCommand = new Command(async () => await AddMedication());

        }

        async Task CreatePatient()
        {

            if (IsBusy)
                return;

            try
            {
                var newaddress = new Address()
                {
                    firstline = Firstline,
                    secondline = Secondline,
                    postcode = Postcode,
                    city = City,
                    county = County
                };

                var newpatient = new Patient()
                {
                    firstname = Firstname,
                    middlename = Middlename,
                    lastname = Lastname,
                    dateofbirth = Dateofbirth,
                    address = newaddress,
                    height = Height,
                    weight = Weight,
                    notes = Notes,
                    mental = _mental,
                    physical = _physical,
                    medication = _medication,

                };
                
                await patientService.CreatePatient(newpatient);

                await Application.Current.MainPage.DisplayAlert("Success!", $"Patient Succesfully Created", "OK");

            }
            catch (Exception ex)
            {

                Debug.WriteLine($"Unable to create patient {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");

            }
            finally
            {
                IsBusy = false;
            }


        }

        async Task AddMentalDisability()
        {

            string result = await App.Current.MainPage.DisplayPromptAsync("Add New Mental Disability", "Enter Disability:");

            try
            {
                _mental.Add(result);

            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        async Task AddPhysicalDisability()
        {

            string result = await App.Current.MainPage.DisplayPromptAsync("Add New Physical Disability", "Enter Disability:");

            try
            {
                _physical.Add(result);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        async Task AddMedication()
        {

            string result = await App.Current.MainPage.DisplayPromptAsync("Add New Medication", "Enter Medication:");

            try
            {
                _medication.Add(result);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

    }
}

