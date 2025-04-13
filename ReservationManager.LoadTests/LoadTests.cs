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
        [Fact]
        public void Test_GetReservations_Load()
        {
            var scenario = Scenario.Create("getReservations", async context =>
                {
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

                    var users = new List<string>(){"letiziapitoni%40mail.it", "luanacavalloro%40mail.it", "francescasettimi%40mail.it", "claudiasabatini%40mail.it"}; 
                    var userEmail = users.ElementAt(new Random().Next(0, users.Count - 1));
                    
                    var day = DateOnly.FromDateTime(DateTime.Now).AddDays(new Random().Next(1, 365));
                    var resourceTypeId = 3;
                    
                    var filterObj = JsonConvert.SerializeObject( new
                    {
                        typeId = resourceTypeId,
                        resourceId = (int?)null,
                        day = day.ToString("yyyy-MM-dd"),
                        timeFrom = "00:00:00",
                        timeTo = "23:59:59"
                    });
                    var filterContent = new StringContent(filterObj, Encoding.UTF8, "application/json");


                    var filteredResource = await httpClient.PostAsync(
                        $"https://localhost:44309/api/Resource/filtered?email={userEmail}",
                        filterContent);
                    
                    if(!filteredResource.IsSuccessStatusCode)
                        return Response.Fail();
                    
                    var deserialized = JsonConvert.DeserializeObject<List<ResourceDto>>(
                        filteredResource.Content.ReadAsStringAsync().Result);
                    var index = new Random().Next(0, deserialized.Count - 1);
                    var newReservation = JsonConvert.SerializeObject(
                        new {
                            title = "Prenotazione",
                            description = "Prenotazione",
                            resourceId = deserialized.ElementAt(index).Id,
                            typeId = 4,
                            day = day
                        });
                    var newReservationContent = new StringContent(newReservation, Encoding.UTF8, "application/json");

                    var createRez = await httpClient.PostAsync(
                        $"https://localhost:44309/api/Reservation?email={userEmail}",
                        newReservationContent);
                         
                    if(!createRez.IsSuccessStatusCode)
                        return Response.Fail();
                    return Response.Ok();
                })
                .WithoutWarmUp()
                .WithLoadSimulations(
                    Simulation.Inject(rate: 5,
                        interval: TimeSpan.FromSeconds(10),
                        during: TimeSpan.FromSeconds(240))
                );

            var result = NBomberRunner
                .RegisterScenarios(scenario)
                .WithReportFolder(@"C:\Users\angiu\Documents\ReservationManager\ReservationManager.LoadTests\Reports")
                .WithReportFileName("getReservations")
                .WithTestName("Test_GetReservations_Load")
                .WithTestSuite("LoadTests")
                .WithReportFormats(ReportFormat.Html)
                .Run();
            
            Assert.True(result.AllRequestCount > 0);
        }
    }
}