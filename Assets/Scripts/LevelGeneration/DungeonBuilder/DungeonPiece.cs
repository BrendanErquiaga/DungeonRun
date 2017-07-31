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
    private List<PieceConnection> exitPieceConnections;
    protected List<Transform> openExits;
    protected List<Transform> openEntrances;

    #region Properties
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

    public List<Transform> OpenEntrances
    {
        get
        {
            return openEntrances;
        }

        set
        {
            openEntrances = value;
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
            staticPropertyString = "";
        }
    }

    public List<PieceConnection> ExitPieceConnections
    {
        get
        {
            return exitPieceConnections;
        }

        set
        {
            exitPieceConnections = value;
        }
    }

    public bool HasPieceConnections
    {
        get { return (ExitPieceConnections.Count > 0) ? true : false; }
    }
    #endregion

    #region Events
    public delegate void DungeonPieceEvent(DungeonPiece dungeonPiece);
    public delegate void DungeonPiecePropertyEvent(DungeonPiece dungeonPiece, string propertyKey, string property);
    public static event DungeonPieceEvent DungeonPieceSpawned;
    public static event DungeonPieceEvent DungeonPieceDestroyed;    
    public static event DungeonPiecePropertyEvent AnyPieceDungeonPropertyAdded;
    public event DungeonPiecePropertyEvent DungeonPropertyAdded;

    protected static void OnDungeonPieceSpawned(DungeonPiece dungeonPiece)
    {
        DungeonPieceEvent handler = DungeonPieceSpawned;
        if (handler != null)
            handler(dungeonPiece);
    }

    protected static void OnDungeonPieceDestroyed(DungeonPiece dungeonPiece)
    {
        DungeonPieceEvent handler = DungeonPieceDestroyed;
        if (handler != null)
            handler(dungeonPiece);
    }

    protected void OnDungeonPropertyAdded(DungeonPiece dungeonPiece, string propertyKey, string propertyAdded)
    {
        DungeonPiecePropertyEvent handler = DungeonPropertyAdded;
        if (handler != null)
            handler(dungeonPiece, propertyKey, propertyAdded);

        OnAnyPieceDungeonPropertyAdded(dungeonPiece, propertyKey, propertyAdded);
    }

    protected static void OnAnyPieceDungeonPropertyAdded(DungeonPiece dungeonPiece, string propertyKey, string propertyAdded)
    {
        DungeonPiecePropertyEvent handler = AnyPieceDungeonPropertyAdded;
        if (handler != null)
            handler(dungeonPiece, propertyKey, propertyAdded);
    }
    #endregion

    private void Awake()
    {
        InitPiece();
        OnDungeonPieceSpawned(this);
    }

    private void OnDestroy()
    {
        OnDungeonPieceDestroyed(this);
    }

    protected void InitPiece()
    {
        openExits = new List<Transform>();
        openEntrances = new List<Transform>();
        exitPieceConnections = new List<PieceConnection>();
        pieceProperties = new SerializableDictionary<string, string>();

        foreach (Transform exit in exits)
        {
            openExits.Add(exit);
            if(exit.GetComponent<ExitMarker>() == null)
            {
                ExitMarker exitMarker = exit.gameObject.AddComponent<ExitMarker>();
                exitMarker.ParentDungeonPiece = this;
            }
        }

        if(entrances.Length == 0)
        {
            entrances = exits;
        }

        foreach (Transform entrance in entrances)
        {
            openEntrances.Add(entrance);
            if (entrance.GetComponent<ExitMarker>() == null)
            {
                ExitMarker exitMarker = entrance.gameObject.AddComponent<ExitMarker>();
                exitMarker.ParentDungeonPiece = this;
            }
        }
    }

    public Transform GetNextOpenEntrance()
    {
        Transform entrance = GetRandomOpenEntrance();

        OpenEntrances.Remove(entrance);

        return entrance;
    }

    public Transform GetRandomOpenEntrance()
    {
        if (OpenEntrances.Count == 1)
        {
            return OpenEntrances[0];
        }

        int r = Random.Range(0, OpenEntrances.Count);
        return OpenEntrances[r];        
    }

    public void RemoveEntrance(Transform entranceUsed)
    {
        if(OpenEntrances.Contains(entranceUsed))
            OpenEntrances.Remove(entranceUsed);

        if(OpenExits.Contains(entranceUsed))
            OpenExits.Remove(entranceUsed);
    }

    /// <summary>
    /// Attaches random entrance to given exit
    /// </summary>
    /// <param name="exit">Exit used</param>
    /// <returns></returns>
    public Transform AttachToExit(Transform exit)
    {
        Transform entranceToUse = GetRandomOpenEntrance();

        entranceToUse.transform.parent = this.gameObject.transform.parent;

        this.gameObject.transform.parent = entranceToUse;

        entranceToUse.SetPositionAndRotation(exit.position, exit.rotation * Quaternion.Euler(0,180,0));

        this.gameObject.transform.parent = entranceToUse.parent;
        entranceToUse.parent = this.gameObject.transform;

        return entranceToUse;
    }

    //Sets Piece B connection to piece A
    public void ConnectPieceToExit(Transform childExit, Transform exitConnectedTo)
    {
        this.RemoveEntrance(childExit);

        ExitMarker exitMarker = exitConnectedTo.GetComponent<ExitMarker>();

        if (exitMarker != null)
        {
            DungeonPiece pieceConnectedTo = exitMarker.ParentDungeonPiece;

            this.ExitPieceConnections.Add(new PieceConnection(childExit, pieceConnectedTo));

            pieceConnectedTo.ConnectExitToPiece(exitConnectedTo, this);
        }
        else
        {
            Debug.LogWarning("Uh-oh... bad things will happen");
        }
    }

    //Sets Piece A connection to Piece B
    public void ConnectExitToPiece(Transform childExit, DungeonPiece pieceConnectedTo)
    {
        this.RemoveEntrance(childExit);

        this.ExitPieceConnections.Add(new PieceConnection(childExit, pieceConnectedTo));
    }

    public void RemoveAllExitConnections()
    {
        foreach(PieceConnection connection in ExitPieceConnections)
        {
            connection.connectedPiece.RemovePieceFromConnections(this);
        }

        ExitPieceConnections.Clear();
    }

    public void RemovePieceFromConnections(DungeonPiece pieceToRemove)
    {
        PieceConnection connectionToRemove = null;

        foreach (PieceConnection connection in ExitPieceConnections)
        {
            if(connection.connectedPiece == pieceToRemove)
            {
                OpenExits.Add(connection.exit);
                connectionToRemove = connection;
                break;
            }
        }

        if(connectionToRemove != null)
        {
            exitPieceConnections.Remove(connectionToRemove);
        }
    }

    public void AddProperty(string propertyKey, string propertyValue)
    {
        if (!this.PieceProperties.ContainsKey(propertyKey))
        {
            this.PieceProperties.Add(propertyKey, propertyValue);
        } else
        {
            this.PieceProperties[propertyKey] = propertyValue;
        }

        this.staticPropertyString = "";
        OnDungeonPropertyAdded(this, propertyKey, propertyValue);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.1f, 0.1f, 0.5f, 0.1f);

        Bounds objectBounds = Extensions.GetBoundsOfChildren(this.gameObject);

        Gizmos.DrawCube(objectBounds.center, objectBounds.size);

        if (exitPieceConnections != null)
        {
            Gizmos.color = new Color(0, 1, 0, 1);
            for (int i = 0; i < exitPieceConnections.Count; i++)
            {
                Vector3 connectionPosition = exitPieceConnections[i].exit.position;
                connectionPosition = connectionPosition.SetY(connectionPosition.y + 0.5f);
                Gizmos.DrawSphere(connectionPosition, 0.5f);
            }
        }
        
        if(OpenExits != null)
        {
            Gizmos.color = new Color(1, 0, 0, 1);
            for (int i = 0; i < OpenExits.Count; i++)
            {
                Vector3 connectionPosition = OpenExits[i].position;
                connectionPosition = connectionPosition.SetY(connectionPosition.y + 0.5f);
                Gizmos.DrawSphere(connectionPosition, 0.5f);
            }
        }
        
        if(PieceProperties != null)
        {
            Gizmos.color = Color.white;
            UnityEditor.Handles.Label(this.transform.position, GetPropertyString());
        }
    }

    string staticPropertyString = "";
    private string GetPropertyString()
    {
        if (staticPropertyString != "")
        {
            return staticPropertyString;
        }

        string propertyString = "";

        foreach(KeyValuePair<string, string> property in PieceProperties)
        {
            propertyString += property.Key + " : " + property.Value + "\n";
        }

        staticPropertyString = propertyString;

        return propertyString;
    }
}

public enum DungeonPieceType
{
    ROOM,
    CONNECTOR,
    INTERSECTION
}

public class PieceConnection
{
    public Transform exit;
    public DungeonPiece connectedPiece;

    public PieceConnection(Transform exit, DungeonPiece connectedPiece)
    {
        this.exit = exit;
        this.connectedPiece = connectedPiece;
    }
}