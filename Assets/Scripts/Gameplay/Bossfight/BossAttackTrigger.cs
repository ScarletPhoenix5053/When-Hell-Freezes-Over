using UnityEngine;
using System.Collections;

/// <summary>
/// Uses a 2D trigger collider to tell the boss to attack along a certain path.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class BossAttackTrigger : MonoBehaviour
{
    /// <summary>
    /// The boss controller.
    /// </summary>
    private IBossAttackTriggerResponder responder;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        responder.StartAttack();
    }

    /// <summary>
    /// Initialization method that sets <see cref="responder"/>. <see cref="responder"/>
    /// is used for callback when the player enters the attatched trigger.
    /// </summary>
    /// <param name="newResponder"></param>
    public void SetResponder(IBossAttackTriggerResponder newResponder)
    {
        responder = newResponder;
    }
}
public interface IBossAttackTriggerResponder
{
    /// <summary>
    /// Tell the boss to start an attack.
    /// </summary>
    /// <param name="attackPath"></param>
    void StartAttack();
}