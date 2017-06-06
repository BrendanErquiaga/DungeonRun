using UnityEditor;
using UnityEngine;
using System.Collections;

public class MaterialMaker : EditorWindow
{
//	bool diffuse;
	static string extendedPath = "";

	[MenuItem ("Editor Controls/Material Maker")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		MaterialMaker window = (MaterialMaker) EditorWindow.GetWindow(typeof(MaterialMaker));
		window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, 200f, 400f);
	}

	static void CreateMaterials(string path)
	{
		foreach (Object selectedObject in Selection.objects)
		{
			if (selectedObject as Texture)
			{
				Material material = new Material(Shader.Find("Diffuse"));
				material.mainTexture = selectedObject as Texture;
				AssetDatabase.CreateAsset(material, path + extendedPath + selectedObject.name + ".mat");
			}
		}
	}

	static void CreateGeneric()
	{
		CreateMaterials(@"Assets/Art/_Materials/");
	}

	static void CreateGui()
	{
		CreateMaterials(@"Assets/Art/_Materials/_GUIMats/");
	}
	
	void OnGUI()
	{
//		diffuse = GUILayout.Toggle(diffuse, "Diffuse");
		extendedPath = GUILayout.TextField(extendedPath);
		if (GUILayout.Button("Materials Folder"))
			CreateGeneric();
		if (GUILayout.Button("GUI Folder"))
			CreateGui();
		if (GUILayout.Button("Name Objects Basd On Material"))
			NameObjectsBasedOnMaterial();
	}

	void NameObjectsBasedOnMaterial()
	{
		foreach (GameObject selectedObject in Selection.gameObjects)
		{
			if (selectedObject.GetComponent<Renderer>() && selectedObject.GetComponent<Renderer>().sharedMaterial)
				selectedObject.name = selectedObject.GetComponent<Renderer>().sharedMaterial.name;
		}
	}
}
