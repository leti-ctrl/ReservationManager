using ReservationManager.Core.Interfaces.Repositories;

namespace ReservationManager.API;

public interface IRepositoryProvider
{
    IMockReservationRepository CurrentRepository { get; }
    void SwitchRepository(string repositoryType);
}

public class RepositoryProvider : IRepositoryProvider
{
    private IMockReservationRepository _currentRepository;

    public IMockReservationRepository CurrentRepository => _currentRepository;

    
    public void SwitchRepository(string repositoryType)
    { 
        var repo = new RepositoryConfig()
        {
            AssemblyPath = @$".\\..\\ReservationManager.{repositoryType}Repository\\bin\\Debug\\net7.0\\ReservationManager.{repositoryType}Repository.dll",
            TypeName = $"ReservationManager.{repositoryType}Repository.ReservationRepository"
        };
        
        _currentRepository = RepositoryLoader.LoadRepository(repo.AssemblyPath, repo.TypeName);
    }
}
