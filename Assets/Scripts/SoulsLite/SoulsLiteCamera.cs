using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsLiteCamera : MonoBehaviour
{
    public enum renderLayers
    {
        builtIn0 = 0,
        builtIn1 = 1,
        builtIn2 = 2,
        builtIn3 = 3,
        builtIn4 = 4,
        builtIn5 = 5,
        builtIn6 = 6,
        builtIn7 = 7,
        NPC = 8,
        Terrain = 9,
        Grass = 10,
        empty0 = 11,
        empty1 = 12,
        empty2 = 13,
        empty3 = 14,
        empty4 = 15,
        empty5 = 16,
        empty6 = 17,
        empty7 = 18,
        empty8 = 19,
        empty9 = 20,
        empty10 = 21,
        empty11 = 22,
        empty12 = 23,
        empty13 = 24,
        empty14 = 25,
        empty15 = 26,
        empty16 = 27,
        empty17 = 28,
        empty18 = 29,
        empty19 = 30,
        empty20 = 31,
    }

    public Camera camera;
    public int grassRenderDistance = 30;

    // Start is called before the first frame update
    void Start()
    {
        setCullDistances();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Zero values in cull distances means "use far plane distance"
    void setCullDistances()
    {
        float[] distances = new float[32];
        distances[(int)renderLayers.Grass] = grassRenderDistance;
        camera.layerCullDistances = distances;
    }
}
