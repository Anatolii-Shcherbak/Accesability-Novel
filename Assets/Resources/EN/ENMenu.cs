using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ENMenu : MonoBehaviour
{
    // Start Menu Texts 
    public Text gamemenu, start, load, quit, help, achivments;


    // Start is called before the first frame update
    void Start()
    {
        if (TestDialogueFiles.Languague == "EN")
            Translate();
       
    }

    public void Translate()
    {
        gamemenu.text = "Against the Iron Sky";
        start.text = "Options";
    }
}
