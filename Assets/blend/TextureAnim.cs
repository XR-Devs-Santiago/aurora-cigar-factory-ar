using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAnim : MonoBehaviour
{

    public float ScrollX = 0.5f;
    public float Scrolly = 0.5f;

    private void Update()
    {
        float offsetX = Time.time * ScrollX;
        float offsety = Time.time * Scrolly;

        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offsetX, offsety);


    }

}
