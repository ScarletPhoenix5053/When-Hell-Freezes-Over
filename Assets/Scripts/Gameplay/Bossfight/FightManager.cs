using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Loads and unloads instances which form a stage into a gameobject
/// </summary>
public class FightManager : MonoBehaviour
{
    [Serializable]
    public class FightManagerEvents
    {
        public UnityEvent OnStageChange;
        public UnityEvent OnLastStageEnd;
    }
    public FightManagerEvents Events;

    public Stage[] Stages;
    [HideInInspector]
    public int CurrentStage;
    public Transform InstanceParent;

    /// <summary>
    /// This script's core methods will not work if the bool is active
    /// </summary>
    [ReadOnly]
    public bool Completed = false;

    public int StageCount { get { return Stages.Length; } }

    /// <summary>
    /// Loads the next stage. Unloads the previous stage by default, but this can be overriden using unloadPrevious overload.
    /// </summary>
    public void GoToNextStage(bool unloadPrevious = true)
    {
        if (Completed) return;

        // Unload prev stage
        if (CurrentStage != 0)
        {
            Stages[CurrentStage - 1].Unload();
        }

        // Incriment
        if (CurrentStage < StageCount)
        {
            CurrentStage++;
            Events.OnStageChange.Invoke();
        }
        else
        {
            Debug.Log("No more stages to load!");
            Events.OnLastStageEnd.Invoke();
            Completed = true;
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
        if (StageCount == 0) return;
        if (Completed) return;

        foreach (Stage stage in Stages)
        {
            stage.SetParent(InstanceParent);
        }
    }
}
