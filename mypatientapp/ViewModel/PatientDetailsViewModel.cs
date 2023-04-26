using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using mypatientapp.Model;
using mypatientapp.Services;

namespace mypatientapp.ViewModel
{

    [QueryProperty(nameof(Passedpatient), "passedpatient")]
    public partial class PatientDetailsViewModel : BaseViewModel
	{

        // Patient

        [ObservableProperty]
        public string id;
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
        [ObservableProperty]
        public DateTime createdOn;
        [ObservableProperty]
        public DateTime lastEdited;

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

        // Other

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotEdit))]
        public bool isEdit;

        public bool IsNotEdit => !IsEdit;

        [ObservableProperty]
        public Patient passedpatient;

        PatientService patientService;

        // Commands

        public Command DeletePatientCommand { get; }
        public Command SetEditModeTrueCommand { get; }
        public Command SetEditModeFalseCommand { get; }
        public Command AddMentalDisabilityCommand { get; }
        public Command AddPhysicalDisabilityCommand { get; }
        public Command AddMedicationCommand { get; }
        public Command UpdatePatientCommand { get; }



        public PatientDetailsViewModel(PatientService patientService)
		{
            this.patientService = patientService;

            DeletePatientCommand = new Command(async () => await DeletePatientAsync(Id));

            SetEditModeTrueCommand = new Command(() => SetEditModeTrue());
            SetEditModeFalseCommand = new Command(() => SetEditModeFalse());

            AddMentalDisabilityCommand = new Command(async () => await AddMentalDisability());
            AddPhysicalDisabilityCommand = new Command(async () => await AddPhysicalDisability());
            AddMedicationCommand = new Command(async () => await AddMedication());
            UpdatePatientCommand = new Command(async () => await UpdatePatient());

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

                if (_mental != null)
                {
                    _mental.Clear();
                }

                if (_physical != null)
                {
                    _physical.Clear();
                }

                if (_medication != null)
                {
                    _medication.Clear();
                }


                var data = await patientService.GetPatientById(id);

                Id = data.Id;
                Firstname = data.firstname;
                Middlename = data.middlename;
                Lastname = data.lastname;
                Dateofbirth = data.dateofbirth;
                Firstline = data.address.firstline;
                Secondline = data.address.secondline;
                Postcode = data.address.postcode;
                City = data.address.city;
                County = data.address.county;
                Height = data.height;
                Weight = data.weight;
                Notes = data.notes;
                // _mental = data.mental;
                // _physical = data.physical;
                // _medication = data.medication;
                CreatedOn = data._createdOn;
                LastEdited = data._lastEdited;

                foreach (string item in data.mental)
                {
                    _mental.Add(item);
                }

                foreach (string item in data.physical)
                {
                    _physical.Add(item);
                }

                foreach (string item in data.medication)
                {
                    _medication.Add(item);
                }

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
                $"{Id}\n{Firstname} {Lastname}\n",
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

                    Debug.WriteLine($"Unable to delete patient {ex.Message}");
                    await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");

                }
                finally
                {
                    IsBusy = false;
                    Console.WriteLine("@DeletePatientAsync - Finally");
                }

            }
            

            


        }

        async Task UpdatePatient()
        {

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var updatedAddress = new Address()
                {
                    firstline = Firstline,
                    secondline = Secondline,
                    postcode = Postcode,
                    city = City,
                    county = County
                };

                var updatedPatient = new Patient()
                {
                    Id = Id,
                    firstname = Firstname,
                    middlename = Middlename,
                    lastname = Lastname,
                    dateofbirth = Dateofbirth,
                    address = updatedAddress,
                    height = Height,
                    weight = Weight,
                    notes = Notes,
                    mental = _mental,
                    physical = _physical,
                    medication = _medication,

                };

                await patientService.UpdatePatientById(updatedPatient);

                await Application.Current.MainPage.DisplayAlert("Success!", "Patient Updated", "OK");



            } catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                IsEdit = false;
                await GetPatientsAsync(Id);
            }


        }

        private void SetEditModeTrue()
        {
            IsEdit = true;
        }

        private async void SetEditModeFalse()
        {

            IsEdit = false;
            await GetPatientsAsync(Id);

        }

        async Task AddMentalDisability()
        {

            string result = await App.Current.MainPage.DisplayPromptAsync("Add New Mental Disability", "Enter Disability:");

            try
            {
                _mental.Add(result);

            }
            catch (Exception ex)
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

