using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class TextMeshWrapper : MonoBehaviour
{
	[SerializeField] float paragraphWidth;
	TextMesh textMesh;
	Renderer textMeshRenderer;

	void Start()
	{
		SetupReferences();
		SetTextMeshText(textMesh.text);
	}

	void SetupReferences()
	{
		textMesh = GetComponent<TextMesh>();
		textMeshRenderer = GetComponent<Renderer>();
	}

	string BreakPartIfNeeded(string part)
	{
		string saveText = textMesh.text;
		textMesh.text = part;

		if (textMeshRenderer.bounds.extents.x > paragraphWidth)
		{
			string remaining = part;
			part = "";
			while (true)
			{
				int length;
				for (length = 2; length <= remaining.Length; length++)
				{
					textMesh.text = remaining.Substring(0, length);
					if (textMeshRenderer.bounds.extents.x > paragraphWidth)
					{
						length--;
						break;
					}
				}
				if (length >= remaining.Length)
				{
					part += remaining;
					break;
				}
				part += remaining.Substring(0, length) + System.Environment.NewLine;
				remaining = remaining.Substring(length);
			}
			part = part.TrimEnd();
		}
		textMesh.text = saveText;
		return part;
	}

	public void SetTextMeshText(string text)
	{
		if (!textMesh)
			SetupReferences();

		if (paragraphWidth == 0)
		{
			textMesh.text = text;
			return;
		}
		string builder = "";
		textMesh.text = "";
		string[] parts = text.Split(' ');
		for (int index = 0; index < parts.Length; index++)
		{
			string part = BreakPartIfNeeded(parts[index]);
			textMesh.text += part + " ";
			if (textMeshRenderer.bounds.extents.x > paragraphWidth)
				textMesh.text = builder.TrimEnd() + System.Environment.NewLine + part + " ";
			builder = textMesh.text;
		}
	}

	public void SetFont(Font newFont)
	{
		textMesh.font = newFont;
		SetTextMeshText(textMesh.text);
	}
}
