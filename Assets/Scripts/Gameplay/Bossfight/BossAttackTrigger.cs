using UnityEngine;
using System.Collections;

/// <summary>
/// Uses a 2D trigger collider to tell the boss to attack along a certain path.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class BossAttackTrigger : MonoBehaviour
{
    /// <summary>
    /// <see cref="BossAttackPath"/> relevant to this <see cref="BossAttackTrigger"/>
    /// </summary>
    public BossAttackPath AttackPath;
    /// <summary>
    /// The boss controller.
    /// </summary>
    private IBossAttackTriggerResponder responder;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        responder.StartAttack(AttackPath);
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
    /// Tell the boss to start an attack along this <see cref="BossAttackPath"/>
    /// </summary>
    /// <param name="attackPath"></param>
    void StartAttack(BossAttackPath attackPath);
}
/// <summary>
/// Linear path for boss' attack to follow. Comprised of a direction and distance.
/// </summary>
public struct BossAttackPath
{
    public Vector2 Direction;
    public float Distance;
    /// <summary>
    /// Optional delay to add before an attack
    /// </summary>
    public float AdditionalDelay;
}