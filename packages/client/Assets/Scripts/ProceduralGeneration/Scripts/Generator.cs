using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public MapGenerator baseGenerator;

    public void Create() {
        Generate();
        Render();
    }

    public virtual void Generate() {
        Debug.Log("Generating: " + this.ToString(), this);
    }

    public virtual void Render() {

    }
}
