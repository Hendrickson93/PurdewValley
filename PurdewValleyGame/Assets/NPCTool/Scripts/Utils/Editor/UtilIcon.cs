using UnityEditor;
using UnityEngine;

/* util that provide the necessary icons for the editor */

namespace EdgarDev.NPCTool.Utils
{
	public class UtilIcon
	{
		private static GUIContent icon_top_view;
		public static GUIContent ICON_TOP_VIEW
		{
			get
			{
				if (icon_top_view == null)
				{
					icon_top_view = EditorGUIUtility.IconContent("AvatarPivot");
					icon_top_view.tooltip = "Top View Angle";
				}
				return icon_top_view;
			}
		}

		private static GUIContent icon_focus;
		public static GUIContent ICON_FOCUS
		{
			get
			{
				if (icon_focus == null)
				{
					icon_focus = EditorGUIUtility.IconContent("d_scenepicking_pickable_hover");
					icon_focus.tooltip = "Frame NPC";
				}
				return icon_focus;
			}
		}

		private static GUIContent icon_move_tool;
		public static GUIContent ICON_MOVE_TOOL
		{
			get
			{
				if (icon_move_tool == null)
				{
					icon_move_tool = EditorGUIUtility.IconContent("MoveTool on");
					icon_move_tool.tooltip = "Move Tool";
				}
				return icon_move_tool;
			}
		}

		private static GUIContent icon_absolute_view_on;
		public static GUIContent ICON_ABSOLUTE_VIEW_ON
		{
			get
			{
				if (icon_absolute_view_on == null)
				{
					icon_absolute_view_on = EditorGUIUtility.IconContent("animationvisibilitytoggleon");
					icon_absolute_view_on.tooltip = "Disable Absolute Visibility";
				}
				return icon_absolute_view_on;
			}
		}

		private static GUIContent icon_absolute_view_off;
		public static GUIContent ICON_ABSOLUTE_VIEW_OFF
		{
			get
			{
				if (icon_absolute_view_off == null)
				{
					icon_absolute_view_off = EditorGUIUtility.IconContent("animationvisibilitytoggleoff");
					icon_absolute_view_off.tooltip = "Enable Absolute Visibility";
				}
				return icon_absolute_view_off;
			}
		}

		private static GUIContent icon_first;
		public static GUIContent ICON_FIRST
		{
			get
			{
				if (icon_first == null)
				{
					icon_first = EditorGUIUtility.IconContent("Animation.FirstKey");
					icon_first.tooltip = "Select First";
				}
				return icon_first;
			}
		}

		private static GUIContent icon_last;
		public static GUIContent ICON_LAST
		{
			get
			{
				if (icon_last == null)
				{
					icon_last = EditorGUIUtility.IconContent("Animation.LastKey");
					icon_last.tooltip = "Select Last";
				}
				return icon_last;
			}
		}

		private static GUIContent icon_next;
		public static GUIContent ICON_NEXT
		{
			get
			{
				if (icon_next == null)
				{
					icon_next = EditorGUIUtility.IconContent("Animation.NextKey");
					icon_next.tooltip = "Select Next";
				}
				return icon_next;
			}
		}

		private static GUIContent icon_previous;
		public static GUIContent ICON_PREVIOUS
		{
			get
			{
				if (icon_previous == null)
				{
					icon_previous = EditorGUIUtility.IconContent("Animation.PrevKey");
					icon_previous.tooltip = "Select Previous";
				}
				return icon_previous;
			}
		}

		private static GUIContent icon_sort;
		public static GUIContent ICON_SORT
		{
			get
			{
				if (icon_sort == null)
				{
					icon_sort = EditorGUIUtility.IconContent("AlphabeticalSorting");
					icon_sort.tooltip = "Sort Pathpoints";
				}
				return icon_sort;
			}
		}

		private static GUIContent icon_lock;
		public static GUIContent ICON_LOCK
		{
			get
			{
				if (icon_lock == null)
				{
					icon_lock = EditorGUIUtility.IconContent("LockIcon-On");
					icon_lock.tooltip = "Unlock";
				}
				return icon_lock;
			}
		}

		private static GUIContent icon_unlock;
		public static GUIContent ICON_UNLOCK
		{
			get
			{
				if (icon_unlock == null)
				{
					icon_unlock = EditorGUIUtility.IconContent("LockIcon");
					icon_unlock.tooltip = "Lock";
				}
				return icon_unlock;
			}
		}

		private static GUIContent icon_dependencies;
		public static GUIContent ICON_DEPENDENCIES
		{
			get
			{
				if (icon_dependencies == null)
				{
					icon_dependencies = EditorGUIUtility.IconContent("d_UnityEditor.FindDependencies");
					icon_dependencies.tooltip = "Select Dependencies";
				}
				return icon_dependencies;
			}
		}
	}
}
