using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Generic")]
public class GenericItem : ScriptableObject
{
    public int ID = 0;
    public string Name = "New Item";
    public string Desc = "A new item.";
    public Texture2D Icon;
}