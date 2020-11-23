using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScrollerScript : MonoBehaviour
{
    public float offsetSpeed = -0.0006f;
    private Renderer myRenderer;

    [HideInInspector]
    public bool canScroll;

    void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canScroll)
        {
            myRenderer.material.mainTextureOffset -= new Vector2(offsetSpeed, 0);
        }
    }
}
