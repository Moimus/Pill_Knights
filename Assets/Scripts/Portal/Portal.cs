using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string[] scenePool;
    public float loadDelay = 0.5f;
    public float fovDistortion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator loadRandomScene()
    {
        AnnounceUI.instance.spawnMessage("saving...");
        yield return new WaitForSeconds(loadDelay);
        int randomRoll = Random.Range(0, scenePool.Length);
        SceneManager.LoadScene(scenePool[randomRoll], LoadSceneMode.Single);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<SoulsLiteController>().savePlayer();
            StartCoroutine(loadRandomScene());
        }
    }
}
