using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPropertyHandler : AnyDungeonPieceBasePropertyHandler
{
    [SerializeField]
    private PropertyMaterialMap propertyMaterialMap;

    protected override void HandlePropertyAdded(DungeonPiece dungeonPiece, string propertyKey, string property)
    {
        base.HandlePropertyAdded(dungeonPiece, propertyKey, property);

        if (PropertyKeysMatch(propertyKey) && propertyMaterialMap.ContainsKey(property))
        {
            Extensions.ApplyMaterialToAllChildRenderers(dungeonPiece.gameObject, propertyMaterialMap[property]);
        }        
    }
}
