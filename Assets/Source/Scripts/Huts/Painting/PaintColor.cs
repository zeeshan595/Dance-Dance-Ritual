using UnityEngine;
using System.Collections;

public class PaintColor : MonoBehaviour
{
    public Color color;

    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = color;
    }
}
