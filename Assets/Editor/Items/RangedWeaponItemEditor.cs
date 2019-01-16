using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RangedWeaponItem))]
public class RangedWeaponItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var weaponItem = target as RangedWeaponItem;
        GUILayout.BeginVertical(GUILayout.Width(48));
        GUILayout.Label(TextureFromSprite(weaponItem.Icon), GUILayout.Width(48), GUILayout.Height(48));
        GUILayout.EndVertical();
        base.OnInspectorGUI();
    }
    public static Texture2D TextureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
}