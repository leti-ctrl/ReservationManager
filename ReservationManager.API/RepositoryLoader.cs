using ReservationManager.Core.Interfaces.Repositories;
using System.Reflection;

namespace ReservationManager.API;

public static class RepositoryLoader
{
    public static IMockReservationRepository LoadRepository(string assemblyPath, string typeName)
    {
        var assembly = Assembly.LoadFrom(assemblyPath);
        var type = assembly.GetType(typeName);

        if (type == null)
        {
            throw new InvalidOperationException($"Tipo {typeName} non trovato nell'assembly {assemblyPath}");
        }

        if (!typeof(IMockReservationRepository).IsAssignableFrom(type))
        {
            throw new InvalidOperationException($"Il tipo {typeName} non implementa IReservationRepository");
        }

        return (IMockReservationRepository)Activator.CreateInstance(type)!;
    }
}
