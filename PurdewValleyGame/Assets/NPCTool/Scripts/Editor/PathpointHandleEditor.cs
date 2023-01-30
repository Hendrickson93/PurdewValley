using EdgarDev.NPCTool.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace EdgarDev.NPCTool
{
	[CustomEditor(typeof(PathpointHandle))]
	public class PathpointHandleEditor : Editor, IPathpointHandleEditor
	{
		private PathpointHandle _InspectedPathpoint;
		private Tool m_LastTool = Tool.None;
		private bool firstChange = true;

		private void OnEnable()
		{
			_InspectedPathpoint = (PathpointHandle)target;

			m_LastTool = Tools.current;
			Tools.current = Tool.None;

			SceneView.duringSceneGui += OnSceneGUI;
		}

		private void OnDisable()
		{
			Tools.current = m_LastTool;

			SceneView.duringSceneGui -= OnSceneGUI;
		}

		public void OnSceneGUI(SceneView sceneView)
		{
			// check if npc window editor is open
			// should return if it is closed
			if (!EditorWindow.HasOpenInstances<NPCWindowEditor>()) return;

			if (_InspectedPathpoint == null) return;

			// get inspected pathpoint position
			Vector3 pos = _InspectedPathpoint.transform.position;

			DrawPathpointGUI(pos);

			// create handle position
			Handles.zTest = CompareFunction.Always;
			pos = Handles.PositionHandle(pos, Quaternion.identity);
			Handles.zTest = CompareFunction.LessEqual;

			if (GUI.changed || _InspectedPathpoint.transform.hasChanged)
			{
				if (firstChange)
				{
					_InspectedPathpoint.transform.hasChanged = false;
					firstChange = false;
					return;
				}

				// for undo operation
				Undo.RecordObject(target, UtilNPC.UNDO_STR_MOVEPATHPOINT);

				// apply changes back to inspected pathpoint
				_InspectedPathpoint.transform.position = pos;

				// apply gravity to inspected pathpoint
				ApplyGravity();

				_InspectedPathpoint.transform.hasChanged = false;
			}
		}

		public void DrawPathpointGUI(Vector3 position)
		{
			// draw selected pathpoint
			Handles.color = UtilNPC.ICON_COLOR_SELECTED_PATHPOINT;
			Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), position, Quaternion.identity, UtilNPC.ICON_SIZE_PATHPOINT, EventType.Repaint);
			Handles.color = Color.white;
		}

		public void ApplyGravity()
		{
			float maxY = UtilNPC.MAP_MAX_Y;
			float maxDistance = UtilNPC.MAP_MAX_Y - UtilNPC.MAP_MIN_Y;

			// get inspected pathpoint position
			// and set its Y value to skybox value

			// THIS IS NOT UPDATE CORRECTLY, HEIGHT IS SET ONLY THE FIRST TIME YOU HANDLE THE POSITION HANDLE
			Vector3 pos = _InspectedPathpoint.transform.position;
			pos.y += 2;
			
			RaycastHit hitRoof;
			if (Physics.Raycast(pos, Vector3.up, out hitRoof, maxDistance))
			{
				pos.y = hitRoof.point.y;
			}
			else
			{
				pos.y = maxY;
			}

			// draw a raycast down and set the raycasthit value
			// set inspected pathpoint position to the hit position
			RaycastHit hit;
			if (Physics.Raycast(pos, Vector3.down, out hit, maxDistance))
			{
				_InspectedPathpoint.transform.position = hit.point;
			}
		}
	}
}