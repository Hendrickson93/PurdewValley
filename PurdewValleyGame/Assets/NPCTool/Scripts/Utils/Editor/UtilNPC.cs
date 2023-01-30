using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* util that provide the necessary functions for the npc */

namespace EdgarDev.NPCTool.Utils
{
	public enum PathpointType
	{
		Static,
		Multi
	}

	public class UtilNPC
	{
		// hierarchy
		public static string HIERARCHY_STR_NPC = "NPC_";
		public static string HIERARCHY_STR_PATHPOINT = "Pathpoint";
		public static string HIERARCHY_STR_MULTIPATHPOINT = "MultiPathpoint";
		public static string HIERARCHY_STR_SEPARATOR = "_";
		public static string HIERARCHY_STR_PARENT_PATHPOINT = "Pathpoints";

		// gui
		public static float ICON_SIZE_PATHPOINT = 0.5f;
		public static float LINE_SIZE_LINEAR = 6f;
		public static Color ICON_COLOR_STATIC_PATHPOINT = Color.green;
		public static Color ICON_COLOR_MULTI_PATHPOINT = Color.blue;
		public static Color ICON_COLOR_SELECTED_PATHPOINT = Color.white;
		public static Color LINE_COLOR_LOWEST_Y_PATHPOINT = Color.white;
		public static Color LINE_COLOR_HIGHEST_Y_PATHPOINT = Color.white;
		public static Color LINE_COLOR_MULTI_PATHPOINT = Color.yellow;

		// button
		public static string BUTTON_STR_ADD_PATH = "Add path";
		public static string BUTTON_STR_ADD_STATIC_PATHPOINT = "Add pathpoint";
		public static string BUTTON_STR_ADD_MULTI_PATHPOINT = "Add multi pathpoint";
		public static string BUTTON_STR_SELECT_MULTI_CHILD_PATHPOINT = "Select multi pathpoint child";
		public static string BUTTON_STR_DELETE_LAST_PATHPOINT = "Delete last";

		// label
		public static string LABEL_STR_EDITING = "Editing ";
		public static string LABEL_STR_PATHPOINTS = "Pathpoints ";
		public static string LABEL_STR_PATHPOINTEDITOR = "Path Editor";

		// warning
		public static string WARNING_STR_NOSELECTION = "Please select an NPC to edit";
		public static string WARNING_STR_TOOMUCHSELECTION = "Too much selection";
		public static string WARNING_STR_INACTIVESELECTION = "Inactive selection";
		public static string WARNING_STR_NOTNPC = " is not an NPC";

		// undo
		public static string UNDO_STR_ADDPATHPOINT = "Add Pathpoint";
		public static string UNDO_STR_ADDMULTIPATHPOINT = "Add Multi Pathpoint";
		public static string UNDO_STR_DELETEPATHPOINT = "Delete Pathpoint";
		public static string UNDO_STR_MOVEPATHPOINT = "Move Pathpoint";

		// others
		public static int NUMBER_MULTIPATHPOINTDEPENDENCIES = 3;
		public static float MAP_MAX_Y = 100;
		public static float MAP_MIN_Y = 0;

#if UNITY_EDITOR
		public static NPC TryGetNPCFromSelection(GameObject[] selection)
		{
			// check if selection does not exists
			if (selection.Length == 0)
			{
				// no selection should return warning feedback
				UtilNPCGUI.DrawWarning(WARNING_STR_NOSELECTION);
				return null;
			}

			// check if selection is multiple
			if (selection.Length > 1)
			{
				// more than one selection should return warning feedback
				UtilNPCGUI.DrawWarning(WARNING_STR_TOOMUCHSELECTION);
				return null;
			}

			// check if selection is inactive
			if (!selection[0].activeInHierarchy)
			{
				// inactive selection should return a warning feedback
				UtilNPCGUI.DrawWarning(WARNING_STR_INACTIVESELECTION);
				return null;
			}

			// try to get npc of selection
			NPC npc = null;
			bool isNPCParent = selection[0].TryGetComponent(out npc);
			if (!isNPCParent)
			{
				// check if selection is child of an npc
				npc = selection[0].GetComponentInParent<NPC>();

				// this is not a child of an npc
				if (npc == null)
				{
					// selection is not an npc should return warning feedback
					UtilNPCGUI.DrawWarning(selection[0].name + WARNING_STR_NOTNPC);
					return null;
				}
			}

			return npc;
		}

		public static void GetMinMaxOfScene(Scene scene)
		{
			GameObject[] gameObjects = scene.GetRootGameObjects();
			List<Renderer> renderers = new List<Renderer>();

			// get every renderer of scene's gameObjects
			foreach (GameObject go in gameObjects)
			{
				Renderer[] rnd_tmp = go.GetComponentsInChildren<Renderer>();
				foreach (Renderer rnd in rnd_tmp)
				{
					renderers.Add(rnd);
				}
			}

			// no renderer in scene
			if (renderers.Count == 0) return;

			// get lowest and highest point of scene's gameObjects
			float lowest = renderers[0].bounds.max.y;
			float highest = renderers[0].bounds.max.y;
			foreach (Renderer rnd in renderers)
			{
				if (rnd.bounds.max.y > highest)
				{
					highest = rnd.bounds.max.y;
				}
				else if (rnd.bounds.max.y < lowest)
				{
					lowest = rnd.bounds.max.y;
				}
			}

			// round values
			MAP_MIN_Y = Mathf.Floor(lowest);
			MAP_MAX_Y = Mathf.Ceil(highest);

			/*Debug.Log("MAP_MIN_Y : " + MAP_MIN_Y + " / " + "MAP_MAX_Y : " + MAP_MAX_Y);*/
		}

		public static void HandlePullNPCData(NPC npc, ref string npcName)
		{
			// check name is different
			if (npcName != npc._NPCName)
			{
				npcName = npc._NPCName;
			}
		}
#endif

		public static void SetNPCName(NPC npc, string newName)
		{
			// set new name reference
			npc._NPCName = newName;

			// set new name in hierarchy
			SetNPCNameOnHierarchy(npc, newName);
		}

		private static void SetNPCNameOnHierarchy(NPC npc, string newName)
		{
			// delete previous custom name if exists
			int index = npc.gameObject.name.IndexOf("(");
			if (index >= 0)
				npc.gameObject.name = npc.gameObject.name.Substring(0, index);

			// set new custom name if is not empty
			if (newName != "")
				npc.gameObject.name += "(" + newName + ")";
		}

		public static void AddNPCMovableComponent(NPC npc, out NPCMovable npcMovable)
		{
			// add npc movable component to npc
			// assign it to out argument
			npcMovable = npc.gameObject.AddComponent<NPCMovable>();
		}

		public static int GetNumberOfNPCOnScene()
		{
			int n = 0;

			// get root gameobjects of scene
			// loop on it
			GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			for (int i = 0; i < gameObjects.Length; i++)
			{
				// increment n if the gameobject is a npc
				if (gameObjects[i].GetComponent<NPC>() != null)
					n++;
			}

			return n;
		}

		public static string GetStringIDName(int index)
		{
			string name = "";
			if (index.ToString().Length == 1) name += "0";
			name += index.ToString();

			return name;
		}

		public static GameObject CreateEmptyNPC()
		{
			// get new npc index name
			int npcIndex = GetNumberOfNPCOnScene() + 1;
			string npcID = GetStringIDName(npcIndex);

			// create new gameobject and set its child
			GameObject npc = new GameObject(HIERARCHY_STR_NPC + npcID, typeof(NPC));
			GameObject colliders = new GameObject("Colliders");
			GameObject graphics = new GameObject("Graphics");
			colliders.transform.SetParent(npc.transform);
			graphics.transform.SetParent(npc.transform);

			return npc;
		}

		public static GameObject CreateStarterNPCMovable()
		{
			// get new npc index name
			int npcIndex = GetNumberOfNPCOnScene() + 1;
			string npcID = GetStringIDName(npcIndex);

			// create new gameobject and set its child
			GameObject npc = new GameObject(HIERARCHY_STR_NPC + npcID, typeof(StarterNPCMovableInitializer));

			return npc;
		}

		public static Color GetColorFromType(PathpointType type)
		{
			Color color = Color.magenta;

			switch (type)
			{
				case PathpointType.Static:
					color = ICON_COLOR_STATIC_PATHPOINT;
					break;
				case PathpointType.Multi:
					color = ICON_COLOR_MULTI_PATHPOINT;
					break;
			}

			return color;
		}

		public static void ImportDependencies()
		{
			UtilTagLayer.CreateTag("Pathpoint");
			UtilTagLayer.CreateTag("MultiPathpoint");
		}
	}
}