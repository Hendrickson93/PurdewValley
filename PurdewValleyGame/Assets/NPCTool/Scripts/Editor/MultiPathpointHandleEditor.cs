using EdgarDev.NPCTool.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace EdgarDev.NPCTool
{
	[CustomEditor(typeof(MultiPathpointHandle))]
	public class MultiPathpointHandleEditor : Editor, IPathpointHandleEditor
	{
		private MultiPathpointHandle _InspectedMultiPathpoint;
		private Tool m_LastTool = Tool.None;
		private bool firstChange = true;

		private void OnEnable()
		{
			_InspectedMultiPathpoint = (MultiPathpointHandle)target;

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

			if (_InspectedMultiPathpoint == null) return;

			// get inspected pathpoint position
			Vector3 pos = _InspectedMultiPathpoint.transform.position;

			DrawPathpointGUI(pos);

			// create handle position
			Handles.zTest = CompareFunction.Always;
			pos = Handles.PositionHandle(pos, Quaternion.identity);
			Handles.zTest = CompareFunction.LessEqual;

			if (GUI.changed || _InspectedMultiPathpoint.transform.hasChanged)
			{
				if (firstChange)
				{
					_InspectedMultiPathpoint.transform.hasChanged = false;
					firstChange = false;
					return;
				}

				// for undo operation
				Undo.RecordObject(target, UtilNPC.UNDO_STR_MOVEPATHPOINT);

				// apply changes back to inspected pathpoint
				_InspectedMultiPathpoint.transform.position = pos;

				// apply gravity to inspected pathpoint
				ApplyGravity();

				_InspectedMultiPathpoint.transform.hasChanged = false;
			}
		}

		public void DrawPathpointGUI(Vector3 position)
		{
			Handles.zTest = CompareFunction.Less;

			Handles.color = UtilNPC.LINE_COLOR_MULTI_PATHPOINT;
			// loop into each pathpoint on multi pathpoint parent and draw intersections
			foreach (Transform pathpoint in _InspectedMultiPathpoint.transform)
			{
				#if UNITY_2020_2_OR_NEWER
					Handles.DrawLine(position, pathpoint.position, UtilNPC.LINE_SIZE_LINEAR);
				#else
					Handles.DrawLine(position, pathpoint.position);
				#endif
			}

			Handles.color = Color.white;
		}

		public void ApplyGravity()
		{
			float maxY = UtilNPC.MAP_MAX_Y;
			float maxDistance = UtilNPC.MAP_MAX_Y - UtilNPC.MAP_MIN_Y;

			// get inspected pathpoint position
			// and set its Y value to skybox value

			// THIS IS NOT UPDATE CORRECTLY, HEIGHT IS SET ONLY THE FIRST TIME YOU HANDLE THE POSITION HANDLE
			Vector3 pos = _InspectedMultiPathpoint.transform.position;
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
				_InspectedMultiPathpoint.transform.position = hit.point;
			}
		}
	}
}