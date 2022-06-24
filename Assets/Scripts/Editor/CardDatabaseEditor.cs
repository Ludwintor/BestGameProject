using System.Collections;
using System.IO;
using System.Collections.Generic;
using ProjectGame.Cards;
using UnityEditor;
using UnityEngine;

namespace ProjectGame.Editor
{
    [CustomEditor(typeof(CardDatabase))]
    public class CardDatabaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Load all cards in this directory"))
                LoadCards();
            DrawDefaultInspector();
        }

        private void LoadCards()
        {
            CardDatabase database = (CardDatabase)target;
            string databaseDir = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(database));
            string[] subfolders = AssetDatabase.GetSubFolders(databaseDir);
            SerializedProperty array = serializedObject.FindProperty("_cards");
            array.ClearArray();
            string[] guids = AssetDatabase.FindAssets("t:CardData", subfolders);
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CardData card = AssetDatabase.LoadAssetAtPath<CardData>(path);
                array.InsertArrayElementAtIndex(0);
                SerializedProperty prop = array.GetArrayElementAtIndex(0);
                prop.objectReferenceValue = card;
            }
            serializedObject.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
        }
    }
}
