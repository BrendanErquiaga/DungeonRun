using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroupsHolder : MonoBehaviour {

    [SerializeField]
    private List<SpawnGroup> spawnGroups;

    public List<SpawnGroup> SpawnGroups
    {
        get
        {
            return spawnGroups;
        }

        set
        {
            spawnGroups = value;
        }
    }
}
