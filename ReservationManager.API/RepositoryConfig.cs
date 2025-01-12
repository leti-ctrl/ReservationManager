
namespace ReservationManager.API
{
    public class RepositoryConfig
    {
        /// <summary>
        /// Percorso dell'assembly che contiene l'implementazione del repository.
        /// </summary>
        public string AssemblyPath { get; set; } = string.Empty;

        /// <summary>
        /// Nome completo del tipo del repository (namespace + nome classe).
        /// </summary>
        public string TypeName { get; set; } = string.Empty;
    }
}
