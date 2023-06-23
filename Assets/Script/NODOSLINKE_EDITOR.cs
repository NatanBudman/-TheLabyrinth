using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NODOSLINKED))]
public class NODOSLINKE_EDITOR : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NODOSLINKED nodos = (NODOSLINKED)target;

        if (GUILayout.Button("NODOLINK"))
        {
            nodos.GetNeighboard();
        }

    }
}
