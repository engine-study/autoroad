using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundMaterial {Dust, Soil, Grass, Sand, Rock, Forest, Water}
public class Ground : SPBase
{
    public GroundMaterial material;
    public Renderer r;

    public override void Init()
    {
        base.Init();
        MapGenerator.Instance.AddGround(MapGenerator.PositionRound(transform.position), this);
    }

    public void SetMaterial(GroundMaterial newMaterial) {
        material = newMaterial;
    }

}
