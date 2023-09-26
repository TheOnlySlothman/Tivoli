﻿namespace Tivoli.Models;

/// <summary>
///     Interface for all entities.
/// </summary>
public interface IEntity
{
    /// <summary>
    ///    Id of entity.
    /// </summary>
    Guid Id { get; set; }
}