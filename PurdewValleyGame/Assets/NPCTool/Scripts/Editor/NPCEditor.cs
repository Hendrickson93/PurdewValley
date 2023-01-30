using UnityEditor;
using UnityEngine;

namespace EdgarDev.NPCTool
{
	[CustomEditor(typeof(NPC))]
	public class NPCEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			// draw edit npc button
			if (GUILayout.Button("Edit NPC"))
			{
				// open npc window editor
				NPCWindowEditor _NPCWindowEditor = EditorWindow.GetWindow(typeof(NPCWindowEditor)) as NPCWindowEditor;
				_NPCWindowEditor.Show();
			}
		}
	}
}