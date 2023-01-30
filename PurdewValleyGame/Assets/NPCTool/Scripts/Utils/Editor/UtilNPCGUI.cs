using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

/* util that provide the necessary GUI functions for the npc */

namespace EdgarDev.NPCTool.Utils
{
	public class UtilNPCGUI
	{
		public static void DrawLinearPath(NPCMovable inspectedNPCMovable, Action<int> callbackDelete)
		{
			// initialize start and end lists
			List<Transform> trStart = new List<Transform>();
			List<Transform> trEnd = new List<Transform>();

			// set start first value (always start path position)
			trStart.Add(inspectedNPCMovable.transform);

			// initialize pathpoint type, float interpolationColor and transform tmp
			PathpointType pathpointType = PathpointType.Static;
			float interpolationColorValue = 0;
			Transform tr = null;

			for (int i = 0; i < inspectedNPCMovable.m_Pathpoints.Length; i++)
			{
				// set tmp value according to the read value of array at index i
				tr = inspectedNPCMovable.m_Pathpoints[i];

				// check null value
				if (tr == null)
				{
					callbackDelete.Invoke(i);
					continue;
				}

				// check if current transform is a multipathpoint or a static pathpoint
				trEnd.Clear();
				if (tr.childCount == 0)
				{
					// is a static pathpoint
					// add end value
					trEnd.Add(tr);
				}
				else
				{
					// is a multi pathpoint
					// add each ends values
					foreach (Transform pathpoint in tr)
						trEnd.Add(pathpoint);
				}

				// loop into each end(s) value(s)
				foreach (Transform pathpoint in trEnd)
				{
					// color interpolation according to the y position
					interpolationColorValue = pathpoint.position.y / UtilNPC.MAP_MAX_Y;
					Handles.color = Color.Lerp(UtilNPC.LINE_COLOR_LOWEST_Y_PATHPOINT, UtilNPC.LINE_COLOR_HIGHEST_Y_PATHPOINT, interpolationColorValue);

					// loop into each start(s) value(s)
					foreach (Transform startPathpoint in trStart)
					{
						#if UNITY_2020_2_OR_NEWER
							Handles.DrawLine(startPathpoint.position, pathpoint.position, UtilNPC.LINE_SIZE_LINEAR);
						#else
							Handles.DrawLine(startPathpoint.position, pathpoint.position);
						#endif

						// draw marker to mark the end of a path index
						if (i != 0)
							UtilNPCGUI.DrawEndPathpointMarker(startPathpoint.position, pathpointType, Handles.color, UtilNPC.ICON_SIZE_PATHPOINT);
					}
				}

				trStart.Clear();
				foreach (Transform pathpoint in trEnd)
					trStart.Add(pathpoint);

				if (trStart.Count > 1)
					pathpointType = PathpointType.Multi;
				else if (UtilNPCMovable.IsMultiPathpoint(trStart[0].transform.parent))
					pathpointType = PathpointType.Multi;
				else
					pathpointType = PathpointType.Static;
			}

			// draw last path marker(s)
			foreach (Transform pathpoint in trEnd)
			{
				UtilNPCGUI.DrawEndPathpointMarker(pathpoint.position, pathpointType, Handles.color, UtilNPC.ICON_SIZE_PATHPOINT);
			}
		}

		public static void DrawEndPathpointMarker(Vector3 position, PathpointType pathpointType, Color defaultColor, float size)
		{
			Handles.color = UtilNPC.GetColorFromType(pathpointType);
			int id = GUIUtility.GetControlID(FocusType.Passive);
			Handles.SphereHandleCap(id, position, Quaternion.identity, size, EventType.Repaint);
			Handles.color = defaultColor;

			int controlID = GUIUtility.GetControlID(id, FocusType.Passive);
		}

		public static void DrawHeader(NPC npc, ref bool absoluteView, ref bool lockHierarchy)
		{
			// draw npc name that is in edit mode
			GUILayout.Label(UtilNPC.LABEL_STR_EDITING + npc.name, EditorStyles.helpBox);

			// draw tools bar
			using (new GUILayout.HorizontalScope())
			{
				// draw top view angle tool button
				if (GUILayout.Button(UtilIcon.ICON_TOP_VIEW, UtilEditor.BUTTON_FIXED_MID_HEIGHT))
				{
					SceneView.lastActiveSceneView.LookAt(npc.transform.position, Quaternion.Euler(90, 0, 0));
				}

				GUIContent absoluteGUIContent = absoluteView ? UtilIcon.ICON_ABSOLUTE_VIEW_ON : UtilIcon.ICON_ABSOLUTE_VIEW_OFF;
				// draw absolute tool button
				if (GUILayout.Button(absoluteGUIContent, UtilEditor.BUTTON_FIXED_MID_HEIGHT))
				{
					absoluteView = !absoluteView;
					SceneView.lastActiveSceneView.Repaint();
				}

				GUIContent lockGUIContent = lockHierarchy ? UtilIcon.ICON_LOCK : UtilIcon.ICON_UNLOCK;
				// draw lock tool button
				if (GUILayout.Button(lockGUIContent, UtilEditor.BUTTON_FIXED_MID_HEIGHT))
				{
					lockHierarchy = !lockHierarchy;

					if (!lockHierarchy) SceneView.lastActiveSceneView.Repaint();

					Debug.Log(npc.gameObject + (lockHierarchy ? " is lock" : " is unlock"));
				}
			}
		}

		public static void DrawAddPathButton(NPC npc, ref NPCMovable InspectedNPCMovable)
		{
			if (GUILayout.Button(UtilNPC.BUTTON_STR_ADD_PATH))
			{
				// add movable component to npc
				UtilNPC.AddNPCMovableComponent(npc, out InspectedNPCMovable);
			}
		}

		public static void DrawWarning(string warning)
		{
			GUILayout.Label(warning, EditorStyles.helpBox);
		}
	}
}
