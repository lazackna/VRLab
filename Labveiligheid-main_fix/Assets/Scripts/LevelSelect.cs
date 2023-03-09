using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{

    public Text textOnWall;
    public List<string> levels;

    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(levels != null) 
            this.textOnWall.text = levels[currentIndex];
    }

    //Scrolls to the next level 
    public void NextLevel()
    {
        Debug.Log("Displaying next level...");
        this.currentIndex++;
        if (this.currentIndex >= levels.Count)
            this.currentIndex = 0;

        EditTextToDisplay();
    }

    //Scrolls to the previous level
    public void PreviousLevel()
    {
        Debug.Log("Displaying previous level...");
        this.currentIndex--;
        if (this.currentIndex < 0)
            this.currentIndex = levels.Count - 1;

        EditTextToDisplay();
    }

    //sends the current level which is displays on the canvas at the wall 
    public string GetCurrentLevel()
    {
        return levels[currentIndex];
    }

    //edits the text on the wall to display the correct level
    void EditTextToDisplay()
    {
        this.textOnWall.text = this.GetCurrentLevel();
    }

    public void ConfirmLevelChoice()
    {
        Debug.Log("Confirmed Level: " + this.GetCurrentLevel() + ", loading...");
        GameObject.Find("Game manager").GetComponent<OverallManager>().StartChoosenLevel(GetCurrentLevel());
        SceneManager.LoadSceneAsync("LaboratoryModelTesting", LoadSceneMode.Single);
        //SceneManager.LoadSceneAsync("Laboratory", LoadSceneMode.Single);
    }
}
