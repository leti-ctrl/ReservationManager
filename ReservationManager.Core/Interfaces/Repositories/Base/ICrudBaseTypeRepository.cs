﻿using ReservationManager.DomainModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Interfaces.Repositories.Base
{
    public interface ICrudBaseTypeRepository<T> where T :BaseType
    {
        Task<IEnumerable<T>> GetAllTypesAsync(CancellationToken cancellationToken = default);
        Task<T?> GetTypeById(int id, CancellationToken cancellationToken = default);
        Task<T?> GetTypeByCode(string code, CancellationToken cancellationToken = default);
        Task<T> CreateTypeAsync(T typeToCreate, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// UpdateTypeAsync not cached for teaching purposes
        /// </summary>
        /// <remarks>Not cached</remarks>
        /// <param name="typeToUpdate">T</param>
        /// <param name="cancellationToken">default</param>
        /// <returns>T</returns>
        Task<T?> UpdateTypeAsync(T typeToUpdate, CancellationToken cancellationToken = default);
        
        Task DeleteTypeAsync(T typeToDelete, CancellationToken cancellationToken = default);
    }
}
