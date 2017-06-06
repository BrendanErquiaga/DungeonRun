using UnityEditor;

public class ModelImportSettings : AssetPostprocessor 
{

	public void OnPreprocessModel()
	{
		ModelImporter myModelImporter = this.assetImporter as ModelImporter;
		
		if(myModelImporter.assetPath.Contains("KeepMaterialsOnImport"))//Don't import materials 
			myModelImporter.importMaterials = true;
		else
			myModelImporter.importMaterials = false;
		
		if(myModelImporter.assetPath.Contains("StaticMeshes"))//Don't import animations
		{
			myModelImporter.importAnimation = false;
			myModelImporter.animationType = ModelImporterAnimationType.None;
		}
		
		myModelImporter.generateSecondaryUV = true;
	}
}
