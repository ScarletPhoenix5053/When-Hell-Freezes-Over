using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeleeWeaponItem))]
public class MeleeWeaponItemEditor : Editor
{
    private Vector2 descScrollPos;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var weaponItem = target as MeleeWeaponItem;
        GUILayout.BeginVertical(GUILayout.Width(48));
        GUILayout.Label(TextureFromSprite(weaponItem.Icon), GUILayout.Width(48), GUILayout.Height(48));
        GUILayout.EndVertical();
        
        /*

        var windowWidth = Screen.width;
        var windowHieght = Screen.height;
        var mainArea = new Rect(0, 0, windowWidth, windowHieght);


        // Image block
        GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(48));
                GUILayout.Label(TextureFromSprite(weaponItem.Icon), GUILayout.Width(48), GUILayout.Height(48));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
                EditorGUILayout.ObjectField(weaponItem.Icon, typeof(Sprite), false, GUILayout.Width(128));
                EditorGUILayout.TextField(weaponItem.Name, GUILayout.Width(128));
            GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        // Text Block
        GUILayout.BeginHorizontal();
            descScrollPos = EditorGUILayout.BeginScrollView(descScrollPos, GUILayout.Height(70));
            EditorGUILayout.TextArea(weaponItem.Desc, GUILayout.Width(500), GUILayout.Height(50));
            EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();

        // Normal Attacks
        /*
        EditorGUILayout.LabelField("Normal Attacks", EditorStyles.boldLabel);
        if (weaponItem.NormalAtkData.Length == 0)
        {
            EditorGUILayout.HelpBox("Chain Length is 0! Must be between 1 and 5", MessageType.Warning);
        }
        else
        {
            for (int i = 0, i < weaponItem.NormalAtkData.Length, i++)
            {
                weaponItem.NormalAtkData[i] = (AttackData)EditorGUILayout.ObjectField(weaponItem.NormalAtkData[i]);
            }
        }*/

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