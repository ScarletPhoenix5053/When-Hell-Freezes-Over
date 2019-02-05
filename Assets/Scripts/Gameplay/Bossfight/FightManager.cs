using System;
using UnityEngine;

/// <summary>
/// Loads and unloads instances which form a stage into a gameobject
/// </summary>
public class FightManager : MonoBehaviour
{
    public Stage[] Stages;
    public int CurrentStage;
    public Transform InstanceParent;

    public int StageCount { get { return Stages.Length; } }

    /// <summary>
    /// Loads the next stage. Unloads the previous stage by default, but this can be overriden using unloadPrevious overload.
    /// </summary>
    public void GoToNextStage(bool unloadPrevious = true)
    {
        // Unload prev stage
        if (CurrentStage != 0)
        {
            Stages[CurrentStage - 1].Unload();
        }

        // Incriment
        if (CurrentStage < StageCount) CurrentStage++;
        else
        {
            Debug.LogWarning("No more stages to load!");
            return;
        }

        // Load next Stage
        Stages[CurrentStage-1].Load();
    }
    /// <summary>
    /// Updates the parent object in every <see cref="InstanceContainer"/> on every <see cref="Stage"/> created by this
    /// script.
    /// </summary>
    public void UpdateInstanceParents()
    {
        if (StageCount <= 0) return;

        foreach (Stage stage in Stages)
        {
            stage.SetParent(InstanceParent);
        }
    }
}
