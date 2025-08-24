using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class HandManager : MonoBehaviour
{
    public float boxUnit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxUnit = GetComponent<BoxCollider2D>().size.x / 10f;
    }

    public abstract void UpdateHandView();


    public abstract void ResizeBounds(int numCards);
}
