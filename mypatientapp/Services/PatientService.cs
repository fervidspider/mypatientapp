using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using mypatientapp.Model;

namespace mypatientapp.Services
{
	public class PatientService
	{

		List<Patient> patientList = new();
        Patient patient = new();
		HttpClient httpClient;
        string baseURL = "https://192.168.68.201:7192";

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

        public PatientService()
		{
			this.httpClient = new HttpClient(GetInsecureHandler());
		}

		public async Task<List<Patient>> GetPatients()
		{

			var response = await httpClient.GetAsync($"{baseURL}/api/Patient");

			if (response.IsSuccessStatusCode)
			{

				patientList = await response.Content.ReadFromJsonAsync<List<Patient>>();

            }

            return patientList;

        }

        public async Task<Patient> GetPatientById(string id)
        {

            var response = await httpClient.GetAsync($"{baseURL}/api/Patient/{id}");
            

            if (response.IsSuccessStatusCode)
            {

                patient = await response.Content.ReadFromJsonAsync<Patient>();

            }

            return patient;

        }

        public async Task DeletePatientById(string id)
        {

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{baseURL}/api/Patient/{id}");
            var response = await httpClient.SendAsync(request);

            Console.WriteLine($"SERVICE: {baseURL}/api/Patient/{id}");

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"{id} Successfully Deleted");
                Console.WriteLine("Success");
            }
            else
            {
                Debug.WriteLine($"Error Deleting {id}");
                Console.WriteLine("Error");
            }
        }



    }
}

