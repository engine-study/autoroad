using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectInfo", menuName = "Engine/Object/Info", order = 1)]
public class ObjectInfo : ScriptableObject
{
    public string objectName;
    [TextArea(1,4)]
    public string description;
    public int health;
    public int energy; 
    public int attack;
    public Effect [] effects;
    
}
