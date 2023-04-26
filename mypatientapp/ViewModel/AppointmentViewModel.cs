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
    public partial class AppointmentViewModel : BaseViewModel
    {

        public ObservableCollection<Appointment> Appointments { get; } = new();

        [ObservableProperty]
        public int pagenum = 1;
        [ObservableProperty]
        public int skip = 0;

        [ObservableProperty]
        public string searchQuery;

        public Command GetAppointmentsCommand { get; }
        public Command NavigateToCommand { get; }


        AppointmentService appointmentService;




        public AppointmentViewModel(AppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
            GetAppointmentsCommand = new Command(async () => await GetAppointmentsAsync());
            NavigateToCommand = new Command(() => NavigateToCreate());


            Title = "Patients"; 

            GetAppointmentsAsync();

        }

        private async void NavigateToCreate()
        {

            // await Shell.Current.GoToAsync(nameof(AppointmentCreate), true);

        }

        public async Task GetAppointmentsAsync()
        {

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var appointments = await appointmentService.GetAppointments();

                if (Appointments.Count != 0)
                    Appointments.Clear();

                foreach (var appointment in appointments) {
                    appointment.datetime = DateTime.SpecifyKind(appointment.datetime, DateTimeKind.Utc).ToLocalTime();
                    Appointments.Add(appointment);
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



    }

    
}

