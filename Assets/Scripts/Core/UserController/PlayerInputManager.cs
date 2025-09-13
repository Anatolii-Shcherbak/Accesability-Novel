using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance { get; private set; }

        // The dynamic bool variable that can be accessed from any class
        public bool dynamicBool = true;
        // Start is called before the first frame update
   
        private void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();


            if (dynamicBool == true && Input.GetKeyDown(KeyCode.Space))
            {
                PromptAdvance();
            }
        }

        public void PromptAdvance()
        {

            if (dynamicBool == true)
            {
                MenuButtons.Instance.InAuto = false;
                DialougeSystem.instance.OnUserPrompt_Next();
                GameObject obj = GameObject.Find("BD");
                GameObject obj2 = GameObject.Find("GoodEnd1");
                GameObject obj3 = GameObject.Find("GoodEnd2");
                GameObject obj1 = GameObject.Find("Death");
                if (obj != null)
                {
                    SceneManager.LoadScene("Start");
                }
                if (obj2 != null)
                {
                    SceneManager.LoadScene("Start");
                }
                if (obj3 != null)
                {
                    SceneManager.LoadScene("Start");
                }
                if (obj1 != null)
                {
                    GameObject saveGameObject = GameObject.Find("SaveLoad"); // Replace with your GameObject's name

                    if (saveGameObject != null)
                    {
                        // Get the SaveScript component from the GameObject
                        SaveScript saveScript = saveGameObject.GetComponent<SaveScript>();

                        if (saveScript != null)
                        {
                            // Call the QuickSave method
                            saveScript.QuickLoad();
                        }
                        else
                        {
                            Debug.LogError("SaveScript component not found on GameObject.");
                        }
                    }
                    else
                    {
                        Debug.LogError("GameObject 'GameManager' not found in the scene.");
                    }
                }
            }
         
        }
    }
}



