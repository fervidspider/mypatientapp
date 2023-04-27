using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using mypatientapp.Model;

namespace mypatientapp.Services
{
	public class AppointmentService
	{

		List<Appointment> appointmentList = new();
        Appointment appointment = new();
		HttpClient httpClient;
        JsonSerializerOptions _serializerOptions;
        string baseURL = "https://mypatientapi.azurewebsites.net";

        public static HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

        public AppointmentService()
		{
			this.httpClient = new HttpClient(GetInsecureHandler());
		}

		public async Task<List<Appointment>> GetAppointments()
		{

			var response = await httpClient.GetAsync($"{baseURL}/api/Appointment");

			if (response.IsSuccessStatusCode)
			{

				appointmentList = await response.Content.ReadFromJsonAsync<List<Appointment>>();

            }

            return appointmentList;

        }

        public async Task<List<Appointment>> GetAppointmentsToday()
        {
            DateTime today = DateTime.Now;
            var response = await httpClient.GetAsync($"{baseURL}/api/Appointment?%24filter=datetime%20gt%20{today.Year}-{today.Month.ToString("d2")}-{today.Day.ToString("d2")}T00%3A00%3A00Z%20and%20datetime%20lt%20{today.Year}-{today.Month.ToString("d2")}-{today.Day.ToString("d2")}T23%3A59%3A59Z");


            if (response.IsSuccessStatusCode)
            {

                appointmentList = await response.Content.ReadFromJsonAsync<List<Appointment>>();

            }

            return appointmentList;

        }

        public async Task<Appointment> GetAppointmentById(string id)
        {

            var response = await httpClient.GetAsync($"{baseURL}/api/Appointment/{id}");
            

            if (response.IsSuccessStatusCode)
            {

                appointment = await response.Content.ReadFromJsonAsync<Appointment>();

            }

            return appointment;

        }

        public async Task DeleteAppointmentById(string id)
        {

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{baseURL}/api/Appointment/{id}");
            var response = await httpClient.SendAsync(request);


            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"{id} Successfully Deleted");
            }
            else
            {
                Debug.WriteLine($"Error Deleting {id}");
            }
        }

        public async Task CreateAppointment(Appointment newAppointment)
        {

            string json = JsonSerializer.Serialize<Appointment>(newAppointment, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{baseURL}/api/Appointment", content);

            Console.WriteLine(response.Content.ToString());

            if (response.IsSuccessStatusCode)
                Debug.WriteLine("Appointment successfully created.");

            else
                Debug.WriteLine("Failed to create appointment");


        }

        public async Task UpdateAppointmentById(Appointment appointment)
        {

            string json = JsonSerializer.Serialize<Appointment>(appointment, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{baseURL}/api/Appointment/{appointment.Id}", content);


            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"{appointment.Id} Successfully Updated");
            }
            else
            {
                Debug.WriteLine($"Error Updating {appointment.Id}");
            }
        }



    }
}

