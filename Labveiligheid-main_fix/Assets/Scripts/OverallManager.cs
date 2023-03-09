using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallManager : MonoBehaviour
{

    struct Task
    {
        public string toDo;
        public bool isDone;

        public Task(string taskToDo)
        {
            this.toDo = taskToDo;
            this.isDone = false;
        }

        public void setDone() {
            this.isDone = true;
        }
    }


    struct Level
    {
        public string _name;
        public List<Task> _tasks;
        public List<string> _interactions;
        public bool _isCompleted;
        public override string ToString()
        {
            string content = "";
            content += $"Level: {this._name} \n";
            
            if (this._tasks != null)
            {
                content += $"\t\tTasks in this level: \n";
                for (int i = 0; i < this._tasks.Count; i++)
                {
                    content += "\t\t\t- " + this._tasks[i].toDo + ", task was " + (this._tasks[i].isDone ? "finished" : "unfinished") + "\n";
                }
            }
            if (this._interactions != null)
            {
                content += "\t\tAll the interactions: \n";
                for (int i = 0; i < this._interactions.Count; i++)
                {
                    content += $"\t\t\t- {this._interactions[i]}";
                }
            }
            
            content += ("\t\tresult of this level: " + this._isCompleted);
            return content;
        }

        public Level(string name)
        {
            this._name = name;
            this._tasks = new List<Task>();
            this._interactions = new List<string>();
            this._isCompleted = false;
        }
    }

    //public attributes
    public List<GameObject> allClothePieces;

    //private attributes
    private List<string> clothePiecesSelected;
    private List<GameObject> clothePieceModelsSelected;

    private Level currentLevel;
    private List<Level> finishedLevels;
    private List<Level> unfinishedLevels;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        this.finishedLevels = new List<Level>();
        this.unfinishedLevels = new List<Level>();
        this.clothePiecesSelected = new List<string>();
        this.clothePieceModelsSelected = new List<GameObject>();

        Level placeHolder = new Level();
        placeHolder._name = "No Current Level";
        currentLevel = placeHolder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //sets the choosen clothe pieces
    //public void SetChoosenClothePieces(GameObject[] clothePieces)
    //{
    //    this.clothePiecesSelected = clothePieces;
    //}

    //sets the choosen clothe pieces
    public void SetChoosenClothePieces(List<string> choosenClothePieces)
    {
        for (int i = 0; i < choosenClothePieces.Count; i++)
        {
            for (int j = 0; j < this.allClothePieces.Count; j++)
            {
                if (this.allClothePieces[j].name == choosenClothePieces[i])
                    this.clothePieceModelsSelected.Add(this.allClothePieces[j]);
            }

        }

        for (int i = 0; i < choosenClothePieces.Count; i++)
        {
            this.clothePiecesSelected.Add(choosenClothePieces[i]);
        }

        Debug.Log("Number of clothes: " + this.clothePiecesSelected.Count);
    }



    //adds the choosen level to the hashtable
    public void StartChoosenLevel(string choosenLevel)
    {
        Level tmpNewLevel = new Level(choosenLevel);

        if (tmpNewLevel._name == "Burner")
        {
            tmpNewLevel._tasks.Add(new Task("pick up the burner"));
            tmpNewLevel._tasks.Add(new Task("use the different flames with the burner"));
            tmpNewLevel._tasks.Add(new Task("clean the lab after the exercise"));
        }
        
        currentLevel = tmpNewLevel;
    }

    public void finishTask(string taskName) {
        for (int t_index = 0; t_index < this.currentLevel._tasks.Count; t_index++)
        {
            if (this.currentLevel._tasks[t_index].toDo == taskName){
                this.currentLevel._tasks[t_index].setDone();
                break;
            }
                
        }
    }

    public void CurrentLevelEnd()
    {
        if(this.currentLevel._name == "No Current Level")
        {
            Debug.Log("Can't add current level, no current level selected!");
            return; 
        }

        if (IsLevelFinished(this.currentLevel))
        {
            finishedLevels.Add(this.currentLevel); 
        }
        else
        {
            unfinishedLevels.Add(this.currentLevel);
        }

        Level placeHolder = new Level();
        placeHolder._name = "No Current Level";
        currentLevel = placeHolder;
    }


    bool IsLevelFinished(Level lvl)
    {
        foreach (Task t in lvl._tasks)
        {
            if (t.isDone == false)
                return false;
        }
        return true;
    }

    //creates a file and added all the stored information
    void MakeReportFile()
    {
        string path = Application.dataPath + "/afterReport.txt";

        if (File.Exists(path))
            File.Delete(path);

        if (!File.Exists(path))
            File.WriteAllText(path, "After report Labsafety application \n \n");

        if (this.currentLevel._name != "No Current Level")
            this.unfinishedLevels.Add(this.currentLevel);

        string content = "";

        //add the selected clothe pieces
        content += "clothe pieces selected: \n";
        for (int i = 0; i < clothePiecesSelected.Count; i++)
        {
            content += "\t- " + clothePiecesSelected[i];
            content += "\n";
        }

        content += "\n";
        content += "List of unfinished level: \n";
        //add the levels which are unfinished
        for (int i = 0; i < this.unfinishedLevels.Count; i++)
        {
            content += "\t- " + this.unfinishedLevels[i];
        }

        File.AppendAllText(path, content);
    }


    //called when the application is closed, it creates the after report file with all the correct information
    private void OnDestroy()
    {
        MakeReportFile();
    }
}
