using UnityEngine;
using UnityEditor;


public enum EquipmentType
{
    Armor,
    MeleeWeapon,
    RangedWeapon,
    Material,
    Potion,
    Artefact,
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Generic")]
public class GenericItem : ScriptableObject
{
    //public int ID = 0;
    [SerializeField] string id;
    public string ID { get { return id; } }
    public string Name = "New Item";
    public string Desc = "A new item.";
    public Sprite Icon;

    public EquipmentType equipmentType;

    private void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }

}