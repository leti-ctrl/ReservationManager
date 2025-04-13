using NBomber.Contracts.Stats;
using NBomber.CSharp;

namespace ReservationManager.LoadTests
{
    public class LoadTests
    {
        [Fact]
        public void Test_GetReservations_Load()
        {
            using var httpClient = new HttpClient();

            var scenario = Scenario.Create("getReservations", async context =>
                {
                    var response = await httpClient.GetAsync("https://localhost:44309/api/Reservation/user?email=letiziapitoni%40mail.it");

                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                })
                .WithoutWarmUp()
                .WithLoadSimulations(
                    Simulation.Inject(rate: 10,
                        interval: TimeSpan.FromSeconds(1),
                        during: TimeSpan.FromSeconds(30))
                );
            
            var result = NBomberRunner
                .RegisterScenarios(scenario)
                .WithReportFolder(@"C:\Users\angiu\Documents\ReservationManager\ReservationManager.LoadTests\Reports")
                .WithReportFileName("getReservations")
                .WithReportFormats(ReportFormat.Html, ReportFormat.Md)  // (HTML + Markdown ad esempio)
                .Run();
            
            Console.WriteLine("OK: ", result.AllOkCount);
            Console.WriteLine("FAIL: ", result.AllFailCount);
        }

        [Fact]
        public async Task Test_LoadScenario()
        {
            using var httpClient = new HttpClient();

            var scenario = Scenario.Create("hello_world_scenario", async context =>
                {
                    var response = await httpClient.GetAsync("https://nbomber.com");

                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                })
                .WithoutWarmUp()
                .WithLoadSimulations(
                    Simulation.Inject(rate: 10,
                        interval: TimeSpan.FromSeconds(1),
                        during: TimeSpan.FromSeconds(30))
                );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }
    }
}