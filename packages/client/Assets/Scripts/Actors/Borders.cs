using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;

public class Borders : MonoBehaviour
{

    public bool OnBounds(Vector3 newPos) { return OnBounds(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.z)); }
    public bool OnBounds(int x, int y) {return x >= Left && x <= Right && y <= Up && y >= Down;}
    public int Left, Right, Up, Down;

    [Header("Bounds")]
    [SerializeField] GameObject borderVisuals;
    [SerializeField] Transform front, back;
    [SerializeField] Transform left, right;
    [SerializeField] private Vector4 boundVector;

    public void SetBorder(Vector4 newBorder) {

        boundVector = newBorder;

        Left = (int)newBorder.x;
        Right = (int)newBorder.y;
        Up = (int)newBorder.z;
        Down = (int)newBorder.w;

        front.localPosition = Vector3.forward * (Up + 0.5f);

        front.localScale = Vector3.one + Vector3.right * (Right-Left);
        back.localScale = Vector3.one + Vector3.right * (Right-Left);

        left.localPosition = Vector3.right * (Left - 0.5f) - Vector3.forward * 0.5f;
        right.localPosition = Vector3.right * (Right + 0.5f) - Vector3.forward * 0.5f;

        left.localScale = Vector3.one + Vector3.forward * (Up);
        right.localScale = Vector3.one + Vector3.forward * (Up);

    }
}
