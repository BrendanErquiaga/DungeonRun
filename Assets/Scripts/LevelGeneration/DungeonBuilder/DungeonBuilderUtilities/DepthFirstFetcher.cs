using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirstFetcher : SimpleFloodFetcher
{
    protected override void AddConnectedPieces(List<DungeonPiece> connectedPieces)
    {
        foreach (DungeonPiece connectedPiece in connectedPieces)
        {
            if (this.AllowedToAddPiece(connectedPiece))
            {
                this.unfetchedPieces.AddFirst(connectedPiece);

                if (!this.uncheckedCenterPieces.Contains(connectedPiece))
                {
                    this.uncheckedCenterPieces.AddFirst(connectedPiece);
                }
            }
        }
    }
}
