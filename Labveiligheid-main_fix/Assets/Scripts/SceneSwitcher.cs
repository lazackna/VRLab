using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneSwitcher : MonoBehaviour
{
    public Button startLevel;

    // Start is called before the first frame update
    void Start()
    {
        startLevel.onClick.AddListener(StartSelectedLevel);
    }

    void StartSelectedLevel()
    {
        SceneManager.LoadScene("Laboratory", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
