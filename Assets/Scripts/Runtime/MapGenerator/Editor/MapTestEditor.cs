using UnityEditor;
using UnityEngine;

namespace ProjectGame
{
    [CustomEditor(typeof(MapTest))]
    public class MapTestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MapTest myScript = (MapTest)target;
            
            if (GUILayout.Button("GenerateSeed"))
            {
                myScript.GenerateSeed();
            }
            
            if (GUILayout.Button("GenerateMap"))
            {
                myScript.GenerateMap();
            }
        }
    }
}