
using UnityEditor;
using UnityEngine;
/*
[CanEditMultipleObjects]
[CustomEditor(typeof(PageController))]
*/
public class CustomPageController : Editor {

    

    void OnEnable() {
        
        
    }


    public override void OnInspectorGUI() {
        

        

    }


}


/*
 * // getting value of custom class
 * 
 * 
 * EditorGUILayout.PropertyField(pager, true);
 * 
 * SerializedProperty img = pager.GetArrayElementAtIndex(1).FindPropertyRelative("background");
 * 
 * Debug.Log((Sprite)img.objectReferenceValue);
 * 
 * serializedObject.ApplyModifiedProperties();
 *
 * 
 */
