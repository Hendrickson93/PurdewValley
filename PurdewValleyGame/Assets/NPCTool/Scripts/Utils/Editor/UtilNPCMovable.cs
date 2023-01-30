using UnityEngine;

/* util that provide the necessary functions for the npc movable  */

namespace EdgarDev.NPCTool.Utils
{
	public class UtilNPCMovable : MonoBehaviour
	{
		public static bool IsMultiPathpoint(Transform selection)
		{
			if (selection == null) return false;

			// check if it is a multi pathpoint regarding the tag
			return selection.CompareTag(UtilNPC.HIERARCHY_STR_MULTIPATHPOINT);
		}
	}
}
