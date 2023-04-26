using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using mypatientapp.Model;

namespace mypatientapp.Services
{
	public class PatientService
	{

		List<Patient> patientList = new();
        Patient patient = new();
		HttpClient httpClient;
        JsonSerializerOptions _serializerOptions;
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

		public async Task<List<Patient>> GetPatients(int skip)
		{

			var response = await httpClient.GetAsync($"{baseURL}/api/Patient?%24top=18&%24skip={skip}");

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


            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"{id} Successfully Deleted");
            }
            else
            {
                Debug.WriteLine($"Error Deleting {id}");
            }
        }

        public async Task CreatePatient(Patient newpatient)
        {

            string json = JsonSerializer.Serialize<Patient>(newpatient, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{baseURL}/api/Patient", content);

            Console.WriteLine(response.Content.ToString());

            if (response.IsSuccessStatusCode)
                Debug.WriteLine("Patient successfully created.");

            else
                Debug.WriteLine("Failed to create patient");


        }

        public async Task UpdatePatientById(Patient patient)
        {

            string json = JsonSerializer.Serialize<Patient>(patient, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{baseURL}/api/Patient/{patient.Id}", content);


            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"{patient.Id} Successfully Updated");
            }
            else
            {
                Debug.WriteLine($"Error Updating {patient.Id}");
            }
        }



    }
}

