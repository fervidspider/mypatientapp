using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using mypatientapp.Model;
using mypatientapp.Services;

namespace mypatientapp.ViewModel
{

    [QueryProperty(nameof(Passedappointment), "passedappointment")]
    public partial class AppointmentDetailsViewModel : BaseViewModel
	{

        // Appointment


        [ObservableProperty]
        public string id;
        [ObservableProperty]
        public string patientid;
        [ObservableProperty]
        public string title;
        [ObservableProperty]
        public string host;

        [ObservableProperty]
        public DateTime appDate;

        [ObservableProperty]
        public TimeSpan appTime;




        [ObservableProperty]
        public string status;
        [ObservableProperty]
        public string notes;

        // Patient

        [ObservableProperty]
        public Patient patientDetails;



        // Other

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotEdit))]
        public bool isEdit;

        public bool IsNotEdit => !IsEdit;

        [ObservableProperty]
        public Appointment passedappointment;

        AppointmentService appointmentService;
        PatientService patientService;

        // Commands

        public Command DeleteAppointmentCommand { get; }
        public Command SetEditModeTrueCommand { get; }
        public Command SetEditModeFalseCommand { get; }
        public Command UpdateAppointmentCommand { get; }



        public AppointmentDetailsViewModel(AppointmentService appointmentService, PatientService patientService)
		{
            this.appointmentService = appointmentService;
            this.patientService = patientService;

            DeleteAppointmentCommand = new Command(async () => await DeleteAppointmentAsync(Id));

            SetEditModeTrueCommand = new Command(() => SetEditModeTrue());
            SetEditModeFalseCommand = new Command(() => SetEditModeFalse());


            UpdateAppointmentCommand = new Command(async () => await UpdateAppointment());

        }


        async partial void OnPassedappointmentChanged(Appointment value)
        {

            await GetAppointmentsAsync(Passedappointment.Id);
            await GetPatientAsync(Passedappointment.patientid);

        }

        async Task GetAppointmentsAsync(string id)
        {

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;



                var data = await appointmentService.GetAppointmentById(id);

                Id = data.Id;
                Patientid = data.patientid;
                Title = data.title;
                Host = data.host;
                AppDate = data.datetime;
                AppTime = DateTime.SpecifyKind(data.datetime, DateTimeKind.Utc).ToLocalTime().TimeOfDay;
                Status = data.status;
                Notes = data.notes;

                Console.WriteLine("APPDATE: " + AppDate);







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

        async Task DeleteAppointmentAsync(string id)
        {

            var answer = await Application.Current.MainPage.DisplayAlert(
                "Confirm Deletion!",
                $"{Title}\n{Host}\n",
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

                    await appointmentService.DeleteAppointmentById(id);

                    await Application.Current.MainPage.DisplayAlert("Success!", $"Appointment \"{id}\" Succesfully Deleted", "OK");

                    await Shell.Current.GoToAsync("..");

                }
                catch (Exception ex)
                {

                    Debug.WriteLine($"Unable to delete appointment {ex.Message}");
                    await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");

                }
                finally
                {
                    IsBusy = false;
                    Console.WriteLine("@DeletePatientAsync - Finally");
                }

            }
            

            


        }

        async Task UpdateAppointment()
        {

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;


                var updatedAppointment = new Appointment()
                {
                    Id = Id,
                    patientid = Patientid,
                    title = Title,
                    host = Host,
                    datetime = new DateTime(AppDate.Year, AppDate.Month, AppDate.Day, AppTime.Hours, AppTime.Minutes, AppTime.Seconds),
                    status = Status,
                    notes = Notes

            };

                Console.WriteLine("AppDateTime " + AppDate);
                Console.WriteLine("AppDateTime " + AppTime);
                Console.WriteLine("updatedTime " + updatedAppointment.datetime);

                await appointmentService.UpdateAppointmentById(updatedAppointment);

                await Application.Current.MainPage.DisplayAlert("Success!", "Appointment Updated", "OK");



            } catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                IsEdit = false;
                await GetAppointmentsAsync(Id);
            }


        }

        private void SetEditModeTrue()
        {
            IsEdit = true;
        }

        private async void SetEditModeFalse()
        {

            IsEdit = false;
            await GetAppointmentsAsync(Id);

        }

        async Task GetPatientAsync(string id)
        {

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;



                var patient = await patientService.GetPatientById(id);

                PatientDetails = patient;


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

