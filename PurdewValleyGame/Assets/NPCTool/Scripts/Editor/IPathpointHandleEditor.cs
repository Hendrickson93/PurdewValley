using UnityEditor;
using UnityEngine;

public interface IPathpointHandleEditor
{
	void OnSceneGUI(SceneView sceneView);
	void DrawPathpointGUI(Vector3 position);
	void ApplyGravity();
}
