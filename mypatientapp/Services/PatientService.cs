using System;
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
            Console.WriteLine($"{baseURL}/api/Patient/{id}");

            if (response.IsSuccessStatusCode)
            {

                patient = await response.Content.ReadFromJsonAsync<Patient>();

            }

            return patient;

        }



    }
}

