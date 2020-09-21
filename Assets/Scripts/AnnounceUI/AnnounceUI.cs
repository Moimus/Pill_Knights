using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnounceUI : MonoBehaviour
{
    public static AnnounceUI instance = null;
    public GameObject popUpMessagePrefab;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void init()
    {
        instance = this;
    }

    public void spawnMessage(string text)
    {
        GameObject message = Instantiate(popUpMessagePrefab, transform);
        message.GetComponent<PopUpMessage>().setText(text);
    }
}
