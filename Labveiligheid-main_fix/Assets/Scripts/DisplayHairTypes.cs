using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHairTypes : MonoBehaviour
{

    public Text textField;
    public string[] hairStyles;

    private int itemIndex;
    public string currentHairStyle;

    // Start is called before the first frame update
    void Start()
    {
        DisplayNewHairStyle();
    }
    

    public void NextHairStyle()
    {
        itemIndex++;
        if (itemIndex >= hairStyles.Length)
            itemIndex = 0;

        DisplayNewHairStyle();
    }

    public void PreviousHairStyle()
    {
        itemIndex--;
        if (itemIndex < 0)
            itemIndex = hairStyles.Length - 1;

        DisplayNewHairStyle();
    }


    void DisplayNewHairStyle()
    {
        textField.text = hairStyles[itemIndex];
        currentHairStyle = hairStyles[itemIndex];
    }

    public string NameOfCurrentHairStyle()
    {
        return this.currentHairStyle;
    }
}
