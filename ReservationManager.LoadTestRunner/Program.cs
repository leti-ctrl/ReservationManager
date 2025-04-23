using System.Net.Http.Headers;
using System.Text;
using NBomber.Contracts.Stats;
using NBomber.CSharp;
using Newtonsoft.Json;
using ReservationManager.Core.Dtos;
var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "https://localhost:44309";

var scenario = Scenario.Create("getReservations", async context =>
{
    using var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Accept.Clear();
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

    var users = new List<string>
    {
        "letiziapitoni%40mail.it", "luanacavalloro%40mail.it", 
        "francescasettimi%40mail.it", "claudiasabatini%40mail.it"
    };
    var userEmail = users[new Random().Next(users.Count)];

    var day = DateOnly.FromDateTime(DateTime.Now).AddDays(new Random().Next(1, 365));
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
    var filteredResource = await httpClient.PostAsync(
        $"{{baseUrl}}/api/Resource/filtered?email={userEmail}", filterContent);

    if (!filteredResource.IsSuccessStatusCode)
        return Response.Fail();

    var deserialized = JsonConvert.DeserializeObject<List<ResourceDto>>(
        await filteredResource.Content.ReadAsStringAsync());

    if (deserialized == null || deserialized.Count == 0)
        return Response.Fail();

    var index = new Random().Next(deserialized.Count);
    var newReservation = JsonConvert.SerializeObject(new
    {
        title = "Prenotazione",
        description = "Prenotazione",
        resourceId = deserialized[index].Id,
        typeId = 4,
        day = day
    });

    var newReservationContent = new StringContent(newReservation, Encoding.UTF8, "application/json");
    var createRez = await httpClient.PostAsync(
        $"{{baseUrl}}/api/Reservation?email={userEmail}", newReservationContent);

    return createRez.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
})
.WithoutWarmUp()
.WithLoadSimulations(
    Simulation.Inject(rate: 5, interval: TimeSpan.FromSeconds(10), during: TimeSpan.FromSeconds(20))
);

NBomberRunner
    .RegisterScenarios(scenario)
    .WithTestName("LoadTest")
    .WithTestSuite("ReservationManager")
    .WithReportFileName("LoadTestReport")
    .WithReportFolder("./Reports")
    .WithReportFormats(ReportFormat.Html)
    .Run();
