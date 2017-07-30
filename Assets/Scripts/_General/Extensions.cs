using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public static class Extensions
{
	#region Vector3
	public static Vector3 XY(this Vector3 vector)
	{
		return new Vector3(vector.x, vector.y, 0);
	}

	public static Vector3 XZ(this Vector3 vector)
	{
		return new Vector3(vector.x, 0, vector.z);
	}

	public static Vector3 YZ(this Vector3 vector)
	{
		return new Vector3(0, vector.y, vector.z);
	}

	/// <summary>
	/// Used to set nonmodifiable vectors		
	/// </summary>
	public static Vector3 SetX(this Vector3 vector, float newX)
	{
		return new Vector3(newX, vector.y, vector.z);
	}

	/// <summary>
	/// Used to set nonmodifiable vectors		
	/// </summary>
	public static Vector3 SetY(this Vector3 vector, float newY)
	{
		return new Vector3(vector.x, newY, vector.z);
	}

	/// <summary>
	/// Used to set nonmodifiable vectors		
	/// </summary>
	public static Vector3 SetZ(this Vector3 vector, float newZ)
	{
		return new Vector3(vector.x, vector.y, newZ);
	}

	public static Vector3 Direction(this Vector3 from, Vector3 to)
	{
		return (to - from).normalized;
	}

	public static bool IsNaN(this Vector3 vector)
	{
		return float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z);
	}
	#endregion

	#region Array
	public static T[] Shuffle<T>(this T[] array)
	{
		for (int i = array.Length; i > 1; i--)
		{
			int j = UnityEngine.Random.Range(0, i);
			T tmp = array[j];
			array[j] = array[i - 1];
			array[i - 1] = tmp;
		}
		return array;
	}
	#endregion

	#region List
	public static void Shuffle<T>(this List<T> list)
	{
		for (int index = list.Count - 1; index > 0; index--)
		{
			int swapPosition = UnityEngine.Random.Range(0, index + 1);
			T value = list[swapPosition];
			list[swapPosition] = list[index];
			list[index] = value;
		}
	}

	public static T GetNearestToPosition<T>(this List<T> potentials, Vector3 position, params Func<Vector3, Vector3>[] functionsToModifiyPositionsForDistanceCheck) where T : Component
	{
		float nearestDistance = 999;
		T nearestObject = null;
		foreach (Func<Vector3, Vector3> functionToCall in functionsToModifiyPositionsForDistanceCheck)
			position = functionToCall(position);
		foreach (T nextObject in potentials)
		{
			Vector3 objectPositionToUse = nextObject.transform.position;
			foreach (Func<Vector3, Vector3> functionToCall in functionsToModifiyPositionsForDistanceCheck)
				objectPositionToUse = functionToCall(objectPositionToUse);
			foreach (Func<Vector3, Vector3> functionToCall in functionsToModifiyPositionsForDistanceCheck)
				position = functionToCall(position);
			float distance = Vector3.Distance(position, objectPositionToUse);
			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				nearestObject = nextObject;
			}
		}
		return nearestObject;
	}

	public static T Last<T>(this List<T> list)
	{
		return list[list.Count - 1];
	}
	#endregion

	#region Color
	/// <summary>
	/// Used to set nonmodifiable colors		
	/// </summary>
	public static Color SetR(this Color color, float newValue)
	{
		color.r = newValue;
		return color;
	}

	/// <summary>
	/// Used to change nonmodifiable colors		
	/// </summary>
	public static Color ModifyR(this Color color, float amount)
	{
		color.r += amount;
		return color;
	}

	/// <summary>
	/// Used to set nonmodifiable colors		
	/// </summary>
	public static Color SetG(this Color color, float newValue)
	{
		color.g = newValue;
		return color;
	}

	/// <summary>
	/// Used to change nonmodifiable colors		
	/// </summary>
	public static Color ModifyG(this Color color, float amount)
	{
		color.g += amount;
		return color;
	}

	/// <summary>
	/// Used to set nonmodifiable colors		
	/// </summary>
	public static Color SetB(this Color color, float newValue)
	{
		color.b = newValue;
		return color;
	}

	/// <summary>
	/// Used to change nonmodifiable colors		
	/// </summary>
	public static Color ModifyB(this Color color, float amount)
	{
		color.b += amount;
		return color;
	}

	/// <summary>
	/// Used to set nonmodifiable colors		
	/// </summary>
	public static Color SetA(this Color color, float newValue)
	{
		color.a = newValue;
		return color;
	}

	/// <summary>
	/// Used to change nonmodifiable colors		
	/// </summary>
	public static Color ModifyA(this Color color, float amount)
	{
		color.a += amount;
		return color;
	}

	public static bool IsApproximately(this Color lhs, Color rhs, float threshold = 0.01f)
	{
		return (lhs.GetVector4() - rhs.GetVector4()).magnitude < threshold;
	}

	public static Vector4 GetVector4(this Color color)
	{
		return new Vector4(color.r, color.g, color.b, color.a);
	}
	#endregion

	#region Renderer
	public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
	{//Add layer checking
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}

    public static void ApplyMaterialToAllChildRenderers(GameObject gameObject, Material newMaterial)
    {
        MeshRenderer[] childRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer childRenderer in childRenderers)
        {
            childRenderer.materials = new Material[0];
            childRenderer.material = newMaterial;
        }
    }
    #endregion

    #region GameObject
    public static void SetLayer(this GameObject gameObject, int layer, bool setChildren = false)
	{
		gameObject.layer = layer;
		if (setChildren)
			for (int index = 0; index < gameObject.transform.childCount; index++)
				gameObject.transform.GetChild(index).gameObject.SetLayer(layer, setChildren);
	}

	public static T GetComponentInChildren<T>(this GameObject gameObject, bool includeInactive) where T : Component
	{
		if (includeInactive)
		{
			T[] allComponents = gameObject.GetComponentsInChildren<T>(true);
			if (allComponents.Length > 0)
				return allComponents[0];
			else
				return null;
		}
		else
			return gameObject.GetComponentInChildren<T>();
	}
	#endregion

	#region LayerMask
	public static bool Contains(this LayerMask layerMask, int layer)
	{
		return (layerMask == (layerMask | (1 << layer)));
	}
	#endregion

	#region Transform
	public static void CopyTransformDataTo(this Transform source, Transform destination)
	{
		destination.position = source.position;
		destination.rotation = source.rotation;
		destination.localScale = source.localScale;
	}
	#endregion

	#region String
	public static int GetWordCount(this string source)
	{
		int wordCount = source.Split(' ').Length;
		//		Debug.Log(string.Format("Word count of {0} is {1}", source, wordCount));
		return wordCount;
	}
	#endregion

	#region Font
	public static Vector2 GetTextSize(this Font font, int fontSize, string text)
	{
		GUIStyle style = new GUIStyle();
		style.font = font;
		style.fontSize = fontSize;
		return style.CalcSize(new GUIContent(text));
	}
    #endregion

    #region Physics
    public static Bounds GetBoundsOfChildren(GameObject parentObject)
    {
        Bounds combinedBounds = new Bounds(parentObject.transform.position, Vector3.zero);
        Renderer parentRenderer = parentObject.GetComponent<Renderer>();

        var renderers = parentObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer render in renderers)
        {
            if (render != parentRenderer) combinedBounds.Encapsulate(render.bounds);
        }

        return combinedBounds;
    }

    public static void IgnoreAllChildColliders(GameObject parentObject)
    {
        Collider[] colliders = parentObject.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            foreach (Collider childCollider in colliders)
            {
                Physics.IgnoreCollision(collider, childCollider);
            }
        }
    }

    public static void ToggleChildColliders(GameObject parentObject, Boolean collidersEnabled = true)
    {
        Collider[] colliders = parentObject.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = collidersEnabled;
        }
    }
    #endregion
}

