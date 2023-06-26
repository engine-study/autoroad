using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType {Population,Attack,Resource,Defend}
public class Building : Structure
{
    public BuildingType buildingType;
    
}
