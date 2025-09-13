using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndOption : MonoBehaviour
{
    public static string DeathReason;
    public void OnButClick()
    {
        string[] objectNames = { "Backgrund", "2 - Characters", "4-Dialogue", "Choose", "ButtonsMenu" };

        foreach (string name in objectNames)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                obj.SetActive(false);
            }
        } 
       

        GameObject Textt2 = GameObject.Find("3-Cinematic");
        Transform Textt = Textt2.transform.Find("Death");
        Textt.gameObject.SetActive(true);

        GameObject targetObject = GameObject.FindWithTag("PlayerEnding");
        TextMeshProUGUI tmp = targetObject.GetComponent<TextMeshProUGUI>();

        //SHOULD BE SPECIFIED FOR DIFERENT ENDS!!!
        tmp.text = DeathReason;
    }
}
