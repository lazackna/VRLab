using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevelSelect : MonoBehaviour
{
    public GameObject wallToBeDestroyed;
    public GameObject[] hitboxesToBeDisabled;

    public OverallManager gm;

    private void DestroyWall()
    {
        Destroy(wallToBeDestroyed);
    }

    private void DisableClotheSelect()
    {
        GameObject[] clotheStances = GameObject.FindGameObjectsWithTag("ClothePieceStand");
        List<string> clothePiecesSelected = new List<string>();
        for (int i = 0; i < clotheStances.Length; i++)
        {
            if (clotheStances[i].GetComponent<DisplayHairTypes>())
            {
                Debug.Log("In if statement for DisplayHairTypes");
                clothePiecesSelected.Add(clotheStances[i].GetComponent<DisplayHairTypes>().NameOfCurrentHairStyle());
                Debug.Log("hair type added: " + clothePiecesSelected[i]);
            }
            else if (clotheStances[i].GetComponent<DisplayClothePiece>())
            {
                Debug.Log("In if statement for DisplayClothePieces");
                clothePiecesSelected.Add(clotheStances[i].GetComponent<DisplayClothePiece>().NameOfCurrentClothePiece());
                Debug.Log("Clothe piece added: " + clothePiecesSelected[i]);
            }

           
            
        }

        gm.SetChoosenClothePieces(clothePiecesSelected);
        for (int i = 0; i < hitboxesToBeDisabled.Length; i++)
        {
            hitboxesToBeDisabled[i].GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void OpenUpLevelSelect()
    {
        DestroyWall();
        DisableClotheSelect();
        Destroy(this);
    }   
}
