using System;
using BC;



namespace TimeRegistrationAPI
{
    public class ProgramTest
    {
        public void Main(string[] args)
        {
            var serviceRoot = "http://mybc:7048/BC/ODataV4/";
            var context = new BC.NAV(new Uri(serviceRoot));
            context.BuildingRequest += Context_BuildingRequest;

            var data = context.AccountantPortalUserTasks.Execute();
            foreach (var task in data)
            {
                Console.WriteLine(task);
            }
        }

        private void Context_BuildingRequest(object? sender, Microsoft.OData.Client.BuildingRequestEventArgs e)
        {
            e.RequestUri = new Uri(e.RequestUri.ToString().Replace("V4/" , "V4/Company"));
            e.Headers.Add("Authorization", "Basic YWRtaW46UGFzc3dvcmQ=");
        }
    }
}
