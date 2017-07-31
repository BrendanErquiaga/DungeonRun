using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RandomPieceFetcher : DungeonPieceFetcher
{
    protected override DungeonPiece FetchNextObjectActual()
    {
        return this.unfetchedPieces.ElementAt<DungeonPiece>(Random.Range(0, this.unfetchedPieces.Count));
    }
}
