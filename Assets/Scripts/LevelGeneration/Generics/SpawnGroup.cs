using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroup : MonoBehaviour
{

    [SerializeField]
    private List<Transform> spawnLocations;
    [SerializeField]
    private int spawnGroupWeighting = 1;

    public List<Transform> SpawnLocations
    {
        get
        {
            return spawnLocations;
        }

        set
        {
            spawnLocations = value;
        }
    }

    public int SpawnGroupWeighting
    {
        get
        {
            return spawnGroupWeighting;
        }

        set
        {
            spawnGroupWeighting = value;
        }
    }
}
