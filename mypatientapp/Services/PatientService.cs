using System;
using System.Net.Http.Json;
using mypatientapp.Model;

namespace mypatientapp.Services
{
	public class PatientService
	{

		List<Patient> patientList = new();
		HttpClient httpClient;

		public PatientService()
		{
			this.httpClient = new HttpClient();
		}

		public async Task<List<Patient>> GetPatients()
		{

			var response = await httpClient.GetAsync("http://localhost:5264/api/Patient");

			if (response.IsSuccessStatusCode)
			{

				patientList = await response.Content.ReadFromJsonAsync<List<Patient>>();

            }

            return patientList;

        }

	}
}

