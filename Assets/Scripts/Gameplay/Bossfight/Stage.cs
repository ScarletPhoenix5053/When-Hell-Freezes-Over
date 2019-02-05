using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Group of InstanceContainers, which holds all of the instances to load in for a stage
/// </summary>
[Serializable]
public class Stage
{
    public InstanceContainer[] Instances;
    /// <summary>
    /// Spawn in all instances contained in this stage
    /// </summary>
    public void Load()
    {
        foreach (InstanceContainer ic in Instances)
        {
            ic.SpawnInstances();
        }
    }
    /// <summary>
    /// Destroy all instances ever spawned by this stage
    /// </summary>
    public void Unload()
    {
        foreach (InstanceContainer ic in Instances)
        {
            ic.DespawnInstances();
        }
    }
    /// <summary>
    /// Assigns <see cref="InstanceContainer.Parent"/> for all <see cref="InstanceContainer"/> children.
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(Transform parent)
    {
        foreach (InstanceContainer ic in Instances)
        {
            ic.Parent = parent;
        }
    }
}
/// <summary>
/// Contains a prefab and an array of spawn locations
/// </summary>
[Serializable]
public class InstanceContainer
{
    public GameObject Prefab;
    public Transform Parent;
    public Vector2[] SpawnLocations;

    /// <summary>
    /// Tracks all of the instances spawned by this class
    /// </summary>
    private List<GameObject> SpawnedObjects = new List<GameObject>();

    /// <summary>
    /// Number of prefabs this Instance container will spawn
    /// </summary>
    public int Count { get { return SpawnLocations.Length; } }

    /// <summary>
    /// Instantiate all prefabs within this container into the current scene.
    /// </summary>
    public void SpawnInstances()
    {
        if (Prefab == null)
        {
            Debug.LogError("Instance containers need a prefab to spawn");
            return;
        }
        if (Parent == null)
        {
            Debug.LogError("Instance containers need a parent oject to spawn instances into!");
            return;
        }
        if (SpawnLocations == null || SpawnLocations.Length == 0)
        {
            Debug.LogWarning("There must be at least one spawn location for InstanceContainer to work!");
        }

        throw new NotImplementedException();
    }
    /// <summary>
    /// Destroy all instances spawned by this class
    /// </summary>
    public void DespawnInstances()
    {
        throw new NotImplementedException();
    }
}