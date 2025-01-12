using ReservationManager.Core.Interfaces.Repositories;

namespace ReservationManager.API;

public interface IRepositorySwitcher
{
    IMockReservationRepository CurrentRepository { get; }
    void SwitchRepository(string repositoryType);
}

public class RepositorySwitcher : IRepositorySwitcher
{
    private readonly IConfiguration _configuration;
    private IMockReservationRepository _currentRepository;

    public IMockReservationRepository CurrentRepository => _currentRepository;

    public RepositorySwitcher(IConfiguration configuration)
    {
        _configuration = configuration;
        _currentRepository = LoadRepository("EF");  // carica il repository di default
    }

    public void SwitchRepository(string repositoryType)
    {
        var repositoriesConfig = _configuration.GetSection("Repositories").Get<Dictionary<string, RepositoryConfig>>();

        if (!repositoriesConfig.ContainsKey(repositoryType))
        {
            throw new ArgumentException($"Repository '{repositoryType}' non trovato.");
        }

        var repoConfig = repositoriesConfig[repositoryType];
        _currentRepository = RepositoryLoader.LoadRepository(repoConfig.AssemblyPath, repoConfig.TypeName);
    }

    private IMockReservationRepository LoadRepository(string repositoryType)
    {
        var repositoriesConfig = _configuration.GetSection("Repositories").Get<Dictionary<string, RepositoryConfig>>();
        var repoConfig = repositoriesConfig[repositoryType];
        return RepositoryLoader.LoadRepository(repoConfig.AssemblyPath, repoConfig.TypeName);
    }
}
