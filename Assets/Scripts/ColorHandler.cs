using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ColorHandler : MonoBehaviour
{
    private Color currentColor;
    private Material material;
    private GameManager gameManager;
    private float r;
    private float g;
    private float b;
    private float a;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        r = material.color.r;
        g = material.color.g;
        b = material.color.b;
    }

    // Update is called once per frame
    void Update()
    {
        r = material.color.r;
        g = material.color.g;
        b = material.color.b;
    }

    public void MergeColor(Material newMaterial)
    {
        if (material.color == Color.white)
        {
            material.color = newMaterial.color;
        }
        else
        {
            r = (r + newMaterial.color.r) % 2;
            g = (g + newMaterial.color.g) % 2;
            b = (b + newMaterial.color.b) % 2;
            a = newMaterial.color.a;

            Color mergedColor = new(r, g, b, a);
            material.color = mergedColor;

        }
        string text = "Find RGB colors\r\n" + "(" + r + ", " + g + ", " + b + ")";
        gameManager.UpdateMission(text);
    }
}
