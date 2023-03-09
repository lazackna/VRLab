using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothePieceNextButton : MonoBehaviour
{
    private bool isColliding = false;
    public DisplayClothePiece DCP;

    private void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;
        Debug.Log("Next Button Pressed");
        DCP.NextClothePiece();
        StartCoroutine("Reset");
    }

    IEnumerable Reset()
    {
        isColliding = false;
        yield return new WaitForEndOfFrame();
    }
}
