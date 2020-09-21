using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationGenerator : MonoBehaviour
{
    public float tileSize = 1f;
    public GameObject vegetationPrefab;
    public float boundsMaxX = 50;
    public float boundsMaxY = 50;
    public float scaleFactor = 2f;

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
        placeVegetation();
    }

    void placeVegetation()
    {
        int testX = 0;
        for(float x = boundsMaxX; x >= 0; x -= tileSize)
        {
            testX += 1;
            Debug.Log(testX);
            RaycastHit raycastHitX;
            bool hitX = Physics.Raycast(new Vector3(transform.position.x - x, transform.position.y - 0.1f, transform.position.z), -transform.up, out raycastHitX, 100, -1);
            if (hitX && raycastHitX.collider.gameObject.tag == ("Terrain"))
            {
                GameObject vegetationInstance = Instantiate(vegetationPrefab, transform);
                vegetationInstance.transform.position = raycastHitX.point;
                vegetationInstance.transform.rotation = Quaternion.Euler(vegetationPrefab.transform.localRotation.x, Random.Range(0, 361), vegetationPrefab.transform.localRotation.z); //Random.Range(0,361)
                float randomScale = Random.Range(vegetationInstance.transform.localScale.x, vegetationInstance.transform.localScale.x + scaleFactor);
                vegetationInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            }
            for (float y = boundsMaxX; y >= 0; y -= tileSize)
            {
                RaycastHit raycastHitY;
                bool hitY = Physics.Raycast(new Vector3(transform.position.x - x, transform.position.y - 0.1f, transform.position.z - y), -transform.up, out raycastHitY, 100, -1);
                if (hitY && raycastHitY.collider.gameObject.tag == ("Terrain"))
                {
                    GameObject vegetationInstance = Instantiate(vegetationPrefab,transform);
                    vegetationInstance.transform.position = raycastHitY.point;
                    vegetationInstance.transform.rotation = Quaternion.Euler(vegetationPrefab.transform.localRotation.x, Random.Range(0, 361), vegetationPrefab.transform.localRotation.z);
                    float randomScale = Random.Range(vegetationInstance.transform.localScale.x, vegetationInstance.transform.localScale.x + scaleFactor);
                    vegetationInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                }
            }
        }
    }
}
