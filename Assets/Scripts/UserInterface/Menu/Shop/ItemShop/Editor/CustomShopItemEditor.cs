using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShopItem)), CanEditMultipleObjects]
public class CustomShopItemEditor : Editor {

    //private ShopItem[] shopItem;

    private ShopItem shopItem;
    private string[] options = new string[] { "Heart", "Phone", "Sheild", "Magnet", "Sweeper", "Bomb", "GearsOfWar" };
    private int index = 0;
    
    public SerializedProperty indexOptions;

    void OnEnable() {

        indexOptions = serializedObject.FindProperty("index");
    
    }

    public override void OnInspectorGUI() {

        serializedObject.Update();

        DrawDefaultInspector();

        index = indexOptions.intValue;
    
        if (index < 0)
            index = 0;

       
        index = EditorGUILayout.Popup("Set String Item", index, options);

        indexOptions.intValue = index;
        
        shopItem = target as ShopItem;
        shopItem.set = Strings.GetStringSet(options[index]);
        
        EditorUtility.SetDirty(target);   

        serializedObject.ApplyModifiedProperties(); 
    }


}
