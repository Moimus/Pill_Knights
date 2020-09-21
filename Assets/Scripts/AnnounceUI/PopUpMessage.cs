using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMessage : MonoBehaviour
{
    public Text text;
    public Text textTransparent;
    public float scaleFactor = 1.0f;
    public float lifeTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        scaleTextOverTime();
    }

    void scaleTextOverTime()
    {
        RectTransform rect = textTransparent.rectTransform;
        rect.localScale = new Vector3(rect.localScale.x + scaleFactor*Time.deltaTime, rect.localScale.y + scaleFactor * Time.deltaTime, rect.localScale.z);
        textTransparent.color = new Color(textTransparent.color.r, textTransparent.color.g, textTransparent.color.b, textTransparent.color.a - scaleFactor * 2 * Time.deltaTime);
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - scaleFactor * 0.8f * Time.deltaTime);
    }

    public void setText(string newText)
    {
        text.text = newText;
        textTransparent.text = newText;
    }


}
