using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using TESTING;
using UnityEngine;
using UnityEngine.UI;


public class HideOptions : MonoBehaviour
{
    public GameObject[] objectsToHide;
    private TestDialogueFiles buttonDataHandler;
    public GameObject Menubut;
    

    private void Awake()
    {
        // Create a GameObject to hold the TestDialogueFiles component
        GameObject handlerObject = new GameObject("ButtonDataHandler");
        buttonDataHandler = handlerObject.AddComponent<TestDialogueFiles>();
    }
  

    public void OnButtonClick()
      {
         // Menubut.SetActive(true);
         // PlayerInputManager.Instance.dynamicBool = true;
          foreach (GameObject obj in objectsToHide)
          {
              obj.SetActive(false);
          }
      }
        
    public void OnButtonClicked(Button button)
    {
        string buttonName = button.name;
        buttonDataHandler.OnButtonClicked(buttonName);
    }
}
