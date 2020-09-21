using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    Vector3 position = new Vector3();
    public float lifetime = 1.0f;
    public GameObject fxPrefab;
    public float delay = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
        StartCoroutine(impact());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator impact()
    {
        yield return new WaitForSeconds(delay);
        GameObject fx = Instantiate(fxPrefab, transform.position, transform.rotation);

        yield return null;
    }
}
