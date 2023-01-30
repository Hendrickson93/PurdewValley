using System.IO;
using UnityEditor;
using UnityEngine;

/* comment here what NPCToolInitializer script does */

namespace EdgarDev
{
	public class NPCToolInitializer
    {
		public static string PROJECT_MASTER_NAME = "NPCTool";
		
#if UNITY_EDITOR
		[InitializeOnLoadMethod]
		public static void ImportDependencies()
		{
			// check if master project
			if (IsMasterProject()) return;

			// import dependencies
			// delete useless file
			NPCTool.Utils.UtilNPC.ImportDependencies();
			Delete();
		}
#endif		

		private static bool IsMasterProject()
		{
			return GetProjectName() == PROJECT_MASTER_NAME;
		}

		private static string GetProjectName()
		{
			string[] s = Application.dataPath.Split('/');
			string projectName = s[s.Length - 2];
			Debug.Log("project = " + projectName);
			return projectName;
		}

		private static void Delete()
		{
			string filePath = Application.dataPath + "/" + "NPCTool" + "/" + "NPCToolInitializer.cs";

			File.Delete(filePath);
			RefreshEditorProjectWindow();
		}

		private static void RefreshEditorProjectWindow()
		{
			AssetDatabase.Refresh();
		}
	}
}
