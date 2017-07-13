using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPiece : MonoBehaviour
{
    [SerializeField]
    protected DungeonPieceType pieceType = DungeonPieceType.ROOM;
    [SerializeField]
    protected Transform[] exits;
    [SerializeField]
    protected Transform[] entrances;
    [SerializeField]
    protected SerializableDictionary<string, string> pieceProperties;
    [SerializeField]
    private List<Transform> openExits;

    public DungeonPieceType PieceType
    {
        get
        {
            return pieceType;
        }

        set
        {
            pieceType = value;
        }
    }

    public Transform[] Entrances
    {
        get
        {
            return entrances;
        }

        set
        {
            entrances = value;
        }
    }

    public Transform[] Exits
    {
        get
        {
            return exits;
        }

        set
        {
            exits = value;
        }
    }

    public List<Transform> OpenExits
    {
        get
        {
            return openExits;
        }

        set
        {
            openExits = value;
        }
    }

    public SerializableDictionary<string, string> PieceProperties
    {
        get
        {
            return pieceProperties;
        }

        set
        {
            pieceProperties = value;
        }
    }

    private void Awake()
    {
        InitPiece();
    }

    protected void InitPiece()
    {
        openExits = new List<Transform>();
        
        foreach(Transform exit in exits)
        {
            openExits.Add(exit);
        }

        if(entrances.Length == 0)
        {
            entrances = exits;
        }
    }

    public Transform GetNextOpenExit()
    {
        Transform exit = null;

        if (OpenExits.Count == 1)
        {
            exit = OpenExits[0];
            OpenExits.RemoveAt(0);
            return exit;
        }

        int r = Random.Range(0, OpenExits.Count);
        exit = OpenExits[r];

        OpenExits.RemoveAt(r);

        return exit;
    }

    public void AttachToExit(Transform exit)
    {
        Transform entranceToUse = GetNextOpenExit();

        entranceToUse.transform.parent = this.gameObject.transform.parent;

        this.gameObject.transform.parent = entranceToUse;

        entranceToUse.SetPositionAndRotation(exit.position, exit.rotation * Quaternion.Euler(0,180,0));

        this.gameObject.transform.parent = entranceToUse.parent;
        entranceToUse.parent = this.gameObject.transform;
    }
}

public enum DungeonPieceType
{
    ROOM,
    CONNECTOR,
    INTERSECTION
}