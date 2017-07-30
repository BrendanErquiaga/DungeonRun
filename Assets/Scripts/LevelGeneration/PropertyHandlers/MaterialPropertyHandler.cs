using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPropertyHandler : AnyDungeonPieceBasePropertyHandler
{
    [SerializeField]
    private PropertyMaterialMap propertyMaterialMap;

    protected override void HandlePropertyAdded(DungeonPiece dungeonPiece, string property)
    {
        base.HandlePropertyAdded(dungeonPiece, property);

        Extensions.ApplyMaterialToAllChildRenderers(dungeonPiece.gameObject, propertyMaterialMap[property]);
    }
}

[System.Serializable]
public class PropertyMaterialMap : SerializableDictionary<string, Material>
{

}
