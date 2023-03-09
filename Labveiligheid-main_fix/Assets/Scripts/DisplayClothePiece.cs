using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayClothePiece : MonoBehaviour
{
    public Transform spawnPoint;
    //the prefabs/models which will be displayed on the "altar" on a specific point
    public GameObject[] clothesToDisplay;
    public float rotationSpeed = 2.0f;

    private int itemIndex = 0;
    public GameObject currentClothePiece;


    // Start is called before the first frame update
    void Start()
    {
        //currentClothePiece = Instantiate(clothesToDisplay[itemIndex], spawnPoint.position, spawnPoint.rotation);
        DisplayNewClothePiece();
        Debug.Log("Starting object: " +  currentClothePiece.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentClothePiece != null)
        {
            currentClothePiece.transform.Rotate(new Vector3(0.0f, rotationSpeed, 0.0f));
        }
    }

    //method to increase the index
    public void NextClothePiece()
    {
        itemIndex++;
        if (itemIndex >= clothesToDisplay.Length)
            itemIndex = 0;

        DisplayNewClothePiece();
    }

    //method to decrease the index
    public void PreviousClothePiece()
    {
        itemIndex--;
        if (itemIndex < 0)
            itemIndex = clothesToDisplay.Length - 1;

        DisplayNewClothePiece();
    }

    private void DisplayNewClothePiece()
    {
        if(currentClothePiece != null)
            Destroy(currentClothePiece);

        currentClothePiece = Instantiate(clothesToDisplay[itemIndex], spawnPoint.position, spawnPoint.rotation);
        currentClothePiece.name = currentClothePiece.name.Replace("(Clone)", "");
    }

    public string NameOfCurrentClothePiece()
    {
        return currentClothePiece.name;
    }
}
