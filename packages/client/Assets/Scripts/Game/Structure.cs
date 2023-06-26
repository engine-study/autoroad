using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : mud.Client.MUDEntity
{

    public MeshRenderer [] mesh;

    public override void Init()
    {
        base.Init();
        MapGenerator.Instance.AddEntity(MapGenerator.PositionRound(transform.position), this);
        
    }
    
}
