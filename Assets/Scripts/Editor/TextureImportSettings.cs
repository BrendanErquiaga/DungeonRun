using UnityEditor;
using UnityEngine;

public class TextureImportSettings : AssetPostprocessor
{
    public int maxTextureSize = 4096;

    public void OnPreprocessTexture()
    {
        TextureImporter myTextureImporter = (TextureImporter)assetImporter;

        myTextureImporter.maxTextureSize = this.maxTextureSize;
		
		if(myTextureImporter.assetPath.Contains("GUI"))
			myTextureImporter.textureType = TextureImporterType.GUI;
    }
}
