using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/* util that provide the necessary GUI functions for the npc movable */

namespace EdgarDev.NPCTool.Utils
{
	public class UtilNPCMovableGUI
	{
		// Draw button only if selection is a multi pathpoint or a child of it
		public static void HandleButtonSelectMultiPathpointChild()
		{
			// get selection
			// return if have not parent
			Transform selection = Selection.activeTransform;

			if (selection == null) return;

			// check if it is a multi pathpoint
			bool IsMultiPathpoint = UtilNPCMovable.IsMultiPathpoint(selection);

			// check if it is a multipathpoint
			if (IsMultiPathpoint)
			{
				// draw button select multi pathpoint child 
				SelectMultiPathpointChild(selection);
			}
			else if (UtilNPCMovable.IsMultiPathpoint(selection.parent))
			{
				SelectMultiPathpointChild(selection.parent, selection);
			}
		}

		public static void SelectMultiPathpointChild(Transform parent, Transform child = null)
		{
			if (child == null)
			{
				// select first child of multi pathpoint
				Selection.activeGameObject = parent.GetChild(0).gameObject;
				return;
			}

			int currentIndex = child.GetSiblingIndex();
			int newIndex = currentIndex + 1;

			// check if it is the last index
			// if true new selection will be first child
			if (currentIndex == parent.childCount - 1) newIndex = 0;

			// select pathpoint child of multi pathpoint
			Selection.activeGameObject = parent.GetChild(newIndex).gameObject;
		}
	}
}

