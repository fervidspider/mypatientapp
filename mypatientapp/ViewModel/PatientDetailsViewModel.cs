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

        public ObservableCollection<String> _mental { get; set; } = new();
        public ObservableCollection<String> _physical { get; set; } = new();
        public ObservableCollection<String> _medication { get; set; } = new();

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

        [ObservableProperty]
        public Patient passedpatient;

        PatientService patientService;

        public Command DeletePatientCommand { get; }

        public PatientDetailsViewModel(PatientService patientService)
		{
            this.patientService = patientService;

            DeletePatientCommand = new Command(async () => await DeletePatientAsync(Id));

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
                _mental = data.mental;
                _physical = data.physical;
                _medication = data.medication;
                CreatedOn = data._createdOn;
                LastEdited = data._lastEdited;

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

