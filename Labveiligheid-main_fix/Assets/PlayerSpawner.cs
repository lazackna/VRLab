using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public bool isLab = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find(playerPrefab.name) == null && GameObject.Find(playerPrefab.name + "(Clone)") == null)
        {
            Instantiate(playerPrefab);
        }

        if (isLab)
            GameObject.Find(playerPrefab.name + "(Clone)").transform.position = new Vector3(3.5f, 0.0f, -12.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
