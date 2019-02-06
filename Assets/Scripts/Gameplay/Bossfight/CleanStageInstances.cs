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
            var go = child.gameObject;

            if (child.GetComponent<EnemyController>() ||
                child.GetComponent<BouncePad>())
            {
                if (Application.isEditor) DestroyImmediate(child.gameObject);
                else Destroy(child.gameObject);

                go = null;
            }
        }
    }
    public void ClearAll()
    {
        foreach (Transform child in transform)
        {
            var go = child.gameObject;

            if (Application.isEditor) DestroyImmediate(child.gameObject);
             else Destroy(child.gameObject);

            go = null;
        }
    }
}
