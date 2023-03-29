using System.Text;

namespace TimeRegistrationApp;

using ServiceReferenceBC;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Newtonsoft.Json;

public partial class MainPage : ContentPage
{
	HttpClient client;


	public MainPage()
	{
		InitializeComponent();

		client = new HttpClient();

		//Setting up the HttpClient
		var _token = $"admin:Password";
		var _tokenToBase64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_token));


		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _tokenToBase64);

		//Adding content from BC to the Picker
		List<string> pickerList = new List<string>();

		//Did this to go around converting error
		Task.Run(async () =>
		{
			pickerList = await GetProjectNames();
		}).GetAwaiter().GetResult();

		ProjectPicker.ItemsSource = pickerList;

	}
	
	
	internal class ERPJsonConverterClass
	{
		[JsonProperty("@odata.context")]
		public string odatacontext { get; set; }
		public string value { get; set; }
	}

	//Removing the unnecessary data from OData
	private String OdataContextRemover(String firstOdata)
	{
		ERPJsonConverterClass Json = JsonSerializer.Deserialize<ERPJsonConverterClass>(firstOdata);
		return Json.value;
	}

	//Getting the list of projects from the BC
	private async Task<List<string>> GetProjectNames()
	{
		var content = new StringContent("",Encoding.UTF8, "application/json");
		HttpResponseMessage responseMessage = await client.PostAsync("http://mybc:7048/BC/ODataV4/AppService_GetProjectNames?Company=CRONUS%20USA%2C%20Inc.",content);

		String data = await responseMessage.Content.ReadAsStringAsync();

		List<string> returnList = new List<string>();
		if (responseMessage.IsSuccessStatusCode)
		{
			String valueFromValue = OdataContextRemover(data);
			returnList = JsonSerializer.Deserialize<List<string>>(valueFromValue);
		}
		return returnList;
	}
	
	
	
	private async void AddTimeBtn(object sender, EventArgs e)
	{
		//Getting the selected project and displaying alert if null
		var selectedProject = ProjectPicker.SelectedItem.ToString();
		if (selectedProject == null)
        {
	        await DisplayAlert("Error", "No project selected", "OK");
            return;
        }
		
		//Getting the Employee ID entered by the user
		var employeeId = EmployeeIdEntry.Text; 
		
		if (employeeId == "")
        {
            await DisplayAlert("Error", "No employee ID entered", "OK");
            return;
        }
		
		//Getting the time entered by the user
		if (!int.TryParse(MinutesEntry.Text, out int minutes))
		{
			await DisplayAlert("Error", "No minutes entered", "OK");
		}
		
		//Creating the time entry based on user input
		var timeRegistration = new InsertTimeReg()
		{
			project =  selectedProject,
			consultant = employeeId,
            minutes = minutes
            
		};
		
		var TimeRegToJson = JsonSerializer.Serialize(timeRegistration);
		
		String jsonData = TimeRegToJson;
		
		var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

		HttpResponseMessage responseMessage = await client.PostAsync("http://mybc:7048/BC/ODataV4/AppService_InsertTimeReg?Company=CRONUS%20USA%2C%20Inc.",content);
	}
}


