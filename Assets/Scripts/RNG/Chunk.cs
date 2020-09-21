using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chunk : MonoBehaviour
{
    public string biome = "";
    public int AICount = 2;
    public NavMeshSurface navMeshSurface;
    public GameObject navMeshPrefab;
    public Transform objectsMarkers;
    public List<Transform> waypoints;
    public Transform objects;
    public Transform AI;
    public List<SoulsLiteAIController> npcs = new List<SoulsLiteAIController>();

    // Start is called before the first frame update
    void Start()
    {
        spawnRandomObjects();
        spawnRandomAI(AICount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO move to a seperate system
        /*if(other.tag == "Player")
        {
            AnnounceUI.instance.spawnMessage("Now Entering:\n" + biome);
        }*/ 
    }

    private void spawnRandomObjects()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/RNGObjects/" + biome);
        foreach(GameObject n in prefabs)
        {
            Debug.Log(n.name);
        }
        foreach(Transform marker in objectsMarkers)
        {
            int rand = Random.Range(0, prefabs.Length);
            int randRotation = Random.Range(0, 360);
            Quaternion finalRotation = Quaternion.Euler(0, randRotation, 0);
            GameObject instance = Instantiate(prefabs[rand], marker.transform.position, finalRotation);
            instance.transform.parent = objects;
            if(instance.GetComponent<PointOfInterest>() != null)
            {
                Transform[] wp = instance.GetComponent<PointOfInterest>().waypoints;
                foreach (Transform n in wp)
                {
                    waypoints.Add(n);
                }
            }
        }
        navMeshSurface.BuildNavMesh();
        
    }

    private void spawnRandomAI(int instances)
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/RNGAI/");
        foreach (GameObject n in prefabs)
        {
            Debug.Log(n.name);
        }

        for(int i = 0; i < instances; i++)
        {
            int randPrefab = Random.Range(0, prefabs.Length);
            int randWaypoint = Random.Range(0, waypoints.Count);
            GameObject instance = Instantiate(prefabs[randPrefab], waypoints[randWaypoint].transform.position, waypoints[randWaypoint].transform.rotation);
            instance.transform.parent = AI;
            instance.GetComponent<SoulsLiteAIController>().parentChunk = this;
            npcs.Add(instance.GetComponent<SoulsLiteAIController>());
        }
    }

    public void resetAllAIPaths() //TODO debug
    {
        foreach(SoulsLiteAIController nb in npcs)
        {
            nb.GetComponent<NavMeshAgent>().enabled = false;
            nb.GetComponent<NavMeshAgent>().enabled = true;
            //nb.GetComponent<NavMeshAgent>().navMeshOwner 

        }
    }

}
