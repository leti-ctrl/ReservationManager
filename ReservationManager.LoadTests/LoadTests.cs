using System.Net.Http.Headers;
using System.Text;
using FluentValidation.Validators;
using NBomber.Contracts.Stats;
using NBomber.CSharp;
using Newtonsoft.Json;
using ReservationManager.Core.Dtos;

namespace ReservationManager.LoadTests
{
    public class LoadTests
    {
        private readonly string _resourceFilteredUrl = "https://localhost:44309/api/Resource/filtered?email=";
        private readonly string _reservationCreateUrl = "https://localhost:44309/api/Reservation?email=";
        private readonly string _outputReportPath =
            @"C:\Users\angiu\Documents\ReservationManager\ReservationManager.LoadTests\Reports";
        
        
        [Fact]
        public void Test_GetReservations_Load()
        {
            var reportFileName =
                $"RetrieveResourceAndCreateReservations_{DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "")}";
            
            var scenario = Scenario.Create(reportFileName, async context =>
                {
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

                    var userEmail = GetUser();
                    
                    var day = DateOnly.FromDateTime(DateTime.Now).AddDays(new Random().Next(800, 900));
                    
                    var filterContent = GetFilterBody(day);
                    var filterResourceUrl = _resourceFilteredUrl + userEmail;
                    var filteredResource = await httpClient.PostAsync(filterResourceUrl, filterContent);
                    if(!filteredResource.IsSuccessStatusCode)
                        return Response.Fail();
                    
                    var newReservationContent = GetReservationBody(filteredResource, day);
                    var newReservationUrl = _reservationCreateUrl + userEmail;

                    var createRez = await httpClient.PostAsync(newReservationUrl, newReservationContent);
                    return !createRez.IsSuccessStatusCode ? Response.Fail() : Response.Ok();
                })
                .WithoutWarmUp()
                .WithLoadSimulations(
                    Simulation.Inject(rate: 5,
                        interval: TimeSpan.FromSeconds(10),
                        during: TimeSpan.FromSeconds(240))
                );

            var result = NBomberRunner
                .RegisterScenarios(scenario)
                .WithReportFolder(_outputReportPath)
                .WithReportFileName(reportFileName)
                .WithTestName("GetResource_InsertReservation")
                .WithTestSuite("LoadTests")
                .WithReportFormats(ReportFormat.Html)
                .Run();
            
            Assert.True(result.AllRequestCount > 0);
        }

        private static StringContent GetReservationBody(HttpResponseMessage filteredResource, DateOnly day)
        {
            var deserialized = JsonConvert.DeserializeObject<List<ResourceDto>>(
                filteredResource.Content.ReadAsStringAsync().Result);
            var index = new Random().Next(0, deserialized.Count - 1);
            var newReservation = JsonConvert.SerializeObject(
                new {
                    title = "Prenotazione",
                    description = "Prenotazione",
                    resourceId = deserialized.ElementAt(index).Id,
                    typeId = 4,
                    day = day.ToString("yyyy-MM-dd"),
                });
            var newReservationContent = new StringContent(newReservation, Encoding.UTF8, "application/json");
            return newReservationContent;
        }

        private static StringContent GetFilterBody(DateOnly day)
        {
            var resourceTypeId = 3;
                    
            var filterObj = JsonConvert.SerializeObject(new
            {
                typeId = resourceTypeId,
                resourceId = (int?)null,
                day = day.ToString("yyyy-MM-dd"),
                timeFrom = "00:00:00",
                timeTo = "23:59:59"
            });
            var filterContent = new StringContent(filterObj, Encoding.UTF8, "application/json");
            return filterContent;
        }

        private static string GetUser()
        {
            var users = new List<string>()
            {
                "letiziapitoni%40mail.it", 
                "luanacavalloro%40mail.it", 
                "francescasettimi%40mail.it", 
                "claudiasabatini%40mail.it"
            }; 
            return users.ElementAt(new Random().Next(0, users.Count - 1));
        }
    }
}