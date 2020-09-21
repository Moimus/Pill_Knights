using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RuntimeMaterial : UnityEngine.Material
{
    public RuntimeMaterial(Shader shader, Texture2D texture, Texture2D normalMap) : base(shader)
    {
        SetTexture("Texture2D", texture);
        SetTexture("NormalMap", normalMap);
    }

}
