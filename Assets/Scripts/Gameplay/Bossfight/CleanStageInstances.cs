using UnityEngine;

/// <summary>
/// Delets any child gameobjects with an <see cref="EnemyController"/> or <see cref="BouncePad"/>
/// </summary>
public class CleanStageInstances : MonoBehaviour
{
    public void ClearCommon()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<EnemyController>() ||
                child.GetComponent<BouncePad>())
            {
                Destroy(child.gameObject);
            }
        }
    }
}
