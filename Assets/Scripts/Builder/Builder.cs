using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Builder : MonoBehaviour
{
    public Transform buildPlaceMarker;
    public GameObject testPrefab; //TODO
    public GameObject[] selectedBuildList = null;
    public int selectedBuildIndex = 0;
    public string selectedBuildListPath = "Prefabs/Buildables/Decoration";
    public KeyCode hotkeyBuild = KeyCode.B;
    public KeyCode hotKeyRotatePositive = KeyCode.PageUp;
    public KeyCode hotKeyPlaceBuild = KeyCode.Space;
    public GameObject ui;
    bool open = false;
    public Text title;

    public Buildable selectedBuildInstance = null;
    public float selectedBuildRotation = 0f;
    public float rotationStep = 15f;
    public NavMeshSurface navMeshSurface;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifeCycle();
    }

    void lifeCycle()
    {
        if (Input.GetKeyDown(hotkeyBuild))
        {
            Debug.Log("buildMode");
            toggleBuildMode();
        }

        if(open)
        {
            if (Input.GetKeyDown(hotKeyRotatePositive))
            {
                rotateSelectedBuild();
            }

            if (selectedBuildInstance != null)
            {
                selectedBuildInstance.transform.rotation = Quaternion.Euler(0, selectedBuildRotation, 0);
                putSelectedDown();

                if(Input.GetKeyDown(hotKeyPlaceBuild))
                {
                    StartCoroutine(placeSelectedBuild());
                }
            }

            if(Input.mouseScrollDelta.y > 0.5f)
            {
                incrementSelectedBuild();
            }
        }
    }

    public void putSelectedDown()
    {
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(buildPlaceMarker.position, -transform.up, out raycastHit, 5, -1);
        if (hit)
        {
            selectedBuildInstance.transform.position = raycastHit.point;
        }
        else
        {
            selectedBuildInstance.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void toggleBuildMode()
    {
        if (open)
        {
            closeBuildMode();
        }
        else
        {
            openBuildMode();
        }
    }

    public void openBuildMode()
    {
        setSelectedBuildList(selectedBuildListPath);
        setSelectedBuild();
        ui.gameObject.SetActive(true);
        open = true;
        //updateUI();
    }

    public void closeBuildMode()
    {
        if (selectedBuildInstance != null)
        {
            Destroy(selectedBuildInstance.gameObject);
        }
        selectedBuildList = null;
        ui.gameObject.SetActive(false);
        navMeshSurface.BuildNavMesh();
        open = false;
    }

    public void setSelectedBuildList(string path)
    {
        selectedBuildList = Resources.LoadAll<GameObject>(path);
    }

    public void setSelectedBuild()
    {
        if(selectedBuildInstance != null)
        {
            Destroy(selectedBuildInstance.gameObject);
        }
        GameObject instance = Instantiate(selectedBuildList[selectedBuildIndex], buildPlaceMarker.position, Quaternion.Euler(0, 0, 0));
        instance.GetComponent<Collider>().enabled = false;
        instance.transform.parent = buildPlaceMarker;
        selectedBuildInstance = instance.GetComponent<Buildable>();
    }

    void rotateSelectedBuild()
    {
        if (open && selectedBuildInstance != null)
        {

            if (selectedBuildRotation < 360f)
            {
                selectedBuildRotation += rotationStep;
            }
            else
            {
                selectedBuildRotation = 0f;
                selectedBuildRotation += rotationStep;
            }
        }

    }

    IEnumerator placeSelectedBuild()
    {
        if(selectedBuildInstance != null)
        {
            selectedBuildInstance.transform.parent = null;
            selectedBuildInstance.activate();
            navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
            selectedBuildInstance = null;
            setSelectedBuild();
        }
        yield return null;
    }

    void incrementSelectedBuild()
    {
        if(selectedBuildIndex < selectedBuildList.Length -1)
        {
            selectedBuildIndex++;
            setSelectedBuild();
        }
        else
        {
            selectedBuildIndex = 0;
            setSelectedBuild();
        }

    }
}