using UnityEngine;
using UnityEditor;
using System.Collections;

public class GridSnapper : EditorWindow
{
	static Vector3 customSnapDistance = Vector3.one;
	static string[] buttonValues = {"0.5", "1", "5", "10", "100", "125"};

	[MenuItem ("Window/Grid Snapper")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		GridSnapper window = (GridSnapper) EditorWindow.GetWindow(typeof(GridSnapper));
        window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, 200f, 400f);
	}

	void OnGUI()
	{
		GUILayout.BeginHorizontal();
		foreach (string value in buttonValues)
			if (GUILayout.Button(value))
				Snap(Vector3.one * int.Parse(value));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		customSnapDistance.x = float.Parse(GUILayout.TextField(customSnapDistance.x.ToString()));
		customSnapDistance.y = float.Parse(GUILayout.TextField(customSnapDistance.y.ToString()));
		customSnapDistance.z = float.Parse(GUILayout.TextField(customSnapDistance.z.ToString()));
		GUILayout.EndHorizontal();
		if (GUILayout.Button("Custom Snap"))
			CustomSnap();
	}

	[MenuItem("Editor Controls/Custom Snap %&s")]
	static void CustomSnap()
	{
		Snap(customSnapDistance);
	}

	static void Snap(Vector3 snapVector)
	{
		foreach (Transform transform in Selection.transforms)
		{
			Undo.RecordObject(transform, "Snap Selection");
			Vector3 pos = transform.position;
			transform.position = new Vector3(Utilities.RoundToNearestMultiple(pos.x, (int)snapVector.x), 
			                                 Utilities.RoundToNearestMultiple(pos.y, (int)snapVector.y), 
			                                 Utilities.RoundToNearestMultiple(pos.z, (int)snapVector.z));
		}
	}
}
