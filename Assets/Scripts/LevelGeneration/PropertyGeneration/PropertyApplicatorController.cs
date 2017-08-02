using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyApplicatorController : MonoBehaviour
{
    [SerializeField]
    private DungeonPiecePropertyApplicator[] propertyApplicators;


    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        DungeonBuilder.DungeonFinishedBuilding += DungeonBuilder_DungeonFinishedBuilding;
    }

    private void DungeonBuilder_DungeonFinishedBuilding()
    {
        ActivateApplicators();
    }

    private void ActivateApplicators()
    {
        Debug.Log("ACTIVATE");
        foreach(DungeonPiecePropertyApplicator propertyApplicator in propertyApplicators)
        {
            propertyApplicator.ApplyPropertiesToAllDungeonPieces();
        }
    }
}
