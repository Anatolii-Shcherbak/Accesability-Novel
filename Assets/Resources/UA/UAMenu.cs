using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UAMenu : MonoBehaviour
{
    public Text gamemenu, start, load, quit, help, achivments;


    // Start is called before the first frame update
    void Start()
    {
        if (TestDialogueFiles.Languague == "UA")
            Translate();
    }

    public void Translate()
    {
        gamemenu.text = "Проти залізного неба";
        start.text = "Опції";
    }
}
