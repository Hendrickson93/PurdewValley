using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.SceneManagement;
using EdgarDev.NPCTool.Utils;

namespace EdgarDev.NPCTool
{
	public class NPCWindowEditor : EditorWindow
	{
		// menu item that create an starter npc movable with a shortcut (ctrl-g on Windows, cmd-g on macOS)
		[MenuItem("Tools/NPC/Create Starter NPC Movable %m", false, 2)]
		public static void CreateStarterNPCMovable()
		{
			GameObject npc = UtilNPC.CreateStarterNPCMovable();
			Selection.activeGameObject = npc;
		}

		// menu item that create an empty npc with a shortcut (ctrl-g on Windows, cmd-g on macOS)
		[MenuItem("Tools/NPC/Create Empty NPC %e", false, 1)]
		public static void CreateEmptyNPC()
		{
			GameObject npc = UtilNPC.CreateEmptyNPC();
			Selection.activeGameObject = npc;
		}

		[MenuItem("Tools/NPC/Open NPC Editor", false, 0)]
		public static void OpenNPCEditorWindow() => GetWindow<NPCWindowEditor>("NPC Editor");

		public string _NPCName;

		private SerializedObject _so;
		private SerializedProperty _propNPCName;

		private NPC _InspectedNPC;
		private NPCMovable _InspectedNPCMovable;
		private NPCMovableEditor _InspectedNPCMovableEditor;
		private bool _AbsoluteView;
		private bool _LockHierarchy;

		private void OnEnable()
		{
			_so = new SerializedObject(this);
			_propNPCName = _so.FindProperty("_NPCName");

			Selection.selectionChanged += Repaint;
			Undo.undoRedoPerformed += Repaint;
			SceneView.duringSceneGui += DuringSceneGUI;
			EditorSceneManager.activeSceneChangedInEditMode += (s, s2) => UtilNPC.GetMinMaxOfScene(s2);
			EditorSceneManager.sceneDirtied += s => UtilNPC.GetMinMaxOfScene(s);
		}

		private void OnDisable()
		{
			Selection.selectionChanged -= Repaint;
			Undo.undoRedoPerformed -= Repaint;
			SceneView.duringSceneGui -= DuringSceneGUI;
			EditorSceneManager.activeSceneChangedInEditMode -= (s, s2) => UtilNPC.GetMinMaxOfScene(s2);
			EditorSceneManager.sceneDirtied -= s => UtilNPC.GetMinMaxOfScene(s);
		}

		private void DuringSceneGUI(SceneView view)
		{
			if (_InspectedNPCMovable == null) return;

			if (Event.current.type == EventType.Repaint)
			{
				if (_InspectedNPCMovable.m_Pathpoints.Length > 0)
				{
					DrawPathOnSceneGUI();
				}
			}
		}

		private void DrawPathOnSceneGUI()
		{
			HandleNPCAbsoluteView();
			UtilNPCGUI.DrawLinearPath(_InspectedNPCMovable, _InspectedNPCMovableEditor.DeletePathpointAtIndex);
			Handles.zTest = CompareFunction.LessEqual;
		}

		private void HandleNPCAbsoluteView()
		{
			Handles.zTest = _AbsoluteView ? CompareFunction.Always : CompareFunction.LessEqual;
		}

		private void OnGUI()
		{
			if (!_LockHierarchy)
			{
				// check if selection is null
				_InspectedNPC = UtilNPC.TryGetNPCFromSelection(Selection.gameObjects);
				if (_InspectedNPC == null)
				{
					// should reset and stop
					_InspectedNPCMovable = null;
					_InspectedNPCMovableEditor = null;
					return;
				}
			}

			// found an npc
			// pull data and draw its gui
			UtilNPC.HandlePullNPCData(_InspectedNPC, ref _NPCName);
			HandleNPCGUI(_InspectedNPC);
		}

		private void HandleNPCGUI(NPC npc)
		{
			UtilNPCGUI.DrawHeader(npc, ref _AbsoluteView, ref _LockHierarchy);
			EditorGUILayout.Space();
			
			_so.Update();

			EditorGUILayout.PropertyField(_propNPCName);

			if (_so.ApplyModifiedProperties())
			{
				// send modified property to npc if needed
				UtilNPC.SetNPCName(npc, _NPCName);
			}

			// try get npc movable from inspected npc
			// draw its gui if has this component
			// otherwise draw button to add the component
			NPCMovable NPCMovableTMP = npc.GetComponent<NPCMovable>();
			if (NPCMovableTMP == null)
			{
				if (GUILayout.Button(UtilNPC.BUTTON_STR_ADD_PATH))
				{
					// add movable component to npc
					UtilNPC.AddNPCMovableComponent(npc, out _InspectedNPCMovable);

					// reset window
					_InspectedNPCMovableEditor = null;
				}
			}
			else
			{
				// check if different npc
				// should reset npc movable window
				if (_InspectedNPCMovable != NPCMovableTMP)
				{
					// reset window
					_InspectedNPCMovableEditor = null;
				}

				_InspectedNPCMovable = NPCMovableTMP;

				// create and draw editor of type npc movable
				DrawInspectedNPCMovableGUI();
			}
		}

		private void DrawInspectedNPCMovableGUI()
		{
			// create editor of type npc movable
			if (_InspectedNPCMovableEditor == null)
			{
				_InspectedNPCMovableEditor = (NPCMovableEditor)Editor.CreateEditor(_InspectedNPCMovable, typeof(NPCMovableEditor));
			}

			// draw editor independently
			_InspectedNPCMovableEditor.OnNPCMovableInspectorGUI(_InspectedNPCMovable);
		}

		private void OnDestroy()
		{
			SceneView.RepaintAll();
		}
	}
}