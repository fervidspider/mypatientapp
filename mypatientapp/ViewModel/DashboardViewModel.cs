using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using mypatientapp.Model;
using mypatientapp.Services;

namespace mypatientapp.ViewModel
{
	public class DashboardViewModel : BaseViewModel
	{

        public ObservableCollection<Appointment> Appointments { get; } = new();

        public Command GetAppointmentTodayCommand { get; }

		AppointmentService appointmentService;

		public DashboardViewModel(AppointmentService appointmentService)
		{

			this.appointmentService = appointmentService;
			GetAppointmentTodayCommand = new Command(async () => await GetAppointmentsTodayAsync());
            GetAppointmentsTodayAsync();

		}

		public async Task GetAppointmentsTodayAsync()
		{

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var appointments = await appointmentService.GetAppointmentsToday();

                if (Appointments.Count != 0)
                    Appointments.Clear();

                foreach (var appointment in appointments)
                {
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

