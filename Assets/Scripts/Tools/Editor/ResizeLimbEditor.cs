using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(ResizeLimb))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI() 
    {
        DrawDefaultInspector();

        ResizeLimb myScript = (ResizeLimb)target;
        if (GUILayout.Button("Resize Limb"))
        {
            myScript.ResizeMyLimb();
        }
    }
}
#endif