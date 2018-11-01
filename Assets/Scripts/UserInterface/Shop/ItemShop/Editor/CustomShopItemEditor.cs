using System;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ShopItem))]
public class CustomShopItemEditor : Editor {

    private ShopItem shopItem;
    private static string[] options = new string[] { "Heart", "Phone", "Sheild", "Magnet", "Sweeper", "Bomb", "GearsOfWar" };
    private static int index = 0;


    void OnEnable() {
        if(shopItem != null)
            index = shopItem.GetIndex();
    }


    public override void OnInspectorGUI() {
        DrawDefaultInspector();

 

        index = EditorGUILayout.Popup("Set String Item",index, options);

        shopItem = target as ShopItem;
       
        shopItem.set = Strings.GetStringSet(options[index]);
        shopItem.SetIndex(index);

        EditorUtility.SetDirty(target);
        
    }


}
