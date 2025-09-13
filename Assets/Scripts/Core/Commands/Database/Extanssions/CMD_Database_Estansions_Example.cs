using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Specialized;
using COMMANDS;
using DIALOGUE;
using UnityEngine.UI;
using UnityEditor;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using TMPro;
using Unity.VisualScripting;
using System.Xml.Linq;


namespace TESTING
{
    public class CMD_Database_Estansions_Example : CMD_DatabaseEstansions
    {
        public static string End1, End2, End3;
        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("print", new Action(PrintDefaultMessage));
            database.AddCommand("print_1p", new Action<string>(PrintUsermessage));
            database.AddCommand("print_mp", new Action<string[]>(PrintLines));

            database.AddCommand("lambda", new Action(() => { Debug.Log("Printing a default message to console from lambda command."); }));
            database.AddCommand("lambda_1p", new Action<string>((arg) => { Debug.Log($"Log user Lambda Message: '{arg}'"); }));
            database.AddCommand("lambda_mp", new Action<string[]>((args) => { Debug.Log(string.Join(", ", args)); }));

            database.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
            database.AddCommand("process_1p", new Func<string, IEnumerator>(LineProcess));
            database.AddCommand("process_mp", new Func<string[], IEnumerator>(MultiLineProcess));

            database.AddCommand("moveChar", new Func<string[], IEnumerator>(MoveCharacter));

  

           database.AddCommand("Chscene", new Func<string, IEnumerator>(Ñhscen));

            database.AddCommand("Chchar", new Func<string[], IEnumerator>(Chchar));

            database.AddCommand("Chmus", new Func<string, IEnumerator>(Chmus));
            database.AddCommand("Chsou", new Func<string, IEnumerator>(Chsou));

            database.AddCommand("Invischar", new Func<string, IEnumerator>(Invischar));

            database.AddCommand("unact", new Func<string, IEnumerator>(UnActiveCharacter));

            database.AddCommand("Hide", new Func<string, IEnumerator>(Hide));
            database.AddCommand("show", new Func<string[], IEnumerator>(Show));
            
            database.AddCommand("Chtext", new Func<string[], IEnumerator>(Chtext));
            database.AddCommand("GetAchiv", new Func<string, IEnumerator>(GetAchiv));
            database.AddCommand("GetAchiv2", new Func<string, IEnumerator>(GetAchiv2));
            database.AddCommand("GetAchiv3", new Func<string, IEnumerator>(GetAchiv3));

            database.AddCommand("ChEndBut", new Func<string, IEnumerator>(ChEndBut));

            database.AddCommand("QuickSave", new Func<IEnumerator>(QuickSave));

            database.AddCommand("DeadReason", new Func<string, IEnumerator>(DeadReason));

            database.AddCommand("ResetCharPos", new Action<string>(ResetCharPos));

            /*
            
            database.AddCommand("RotateChar", new Func<string, IEnumerator>(RotateChar));
            */


        }

      

        private static void PrintDefaultMessage()
        {
            Debug.Log("Printing a default message to console. ");
        }

        private static void PrintUsermessage(string message)
        {
            Debug.Log($"User Message : '{message}'");
        }

        private static void PrintLines(string[] lines)
        {
            int i = 1;
            foreach (string line in lines)
            {
                Debug.Log($"{i++}. '{line}'");
            }
        }

        private static IEnumerator SimpleProcess()
        {
            for (int i = 1; i <= 5; i++)
            {
                Debug.Log($"Process Runing... [{i}]");
                yield return new WaitForSeconds(1);
            }
        }

        private static IEnumerator LineProcess(string data)
        {
            if (int.TryParse(data, out int num))
            {
                for (int i = 1; i <= 5; i++)
                {
                    Debug.Log($"Process Runing... [{i}]");
                    yield return new WaitForSeconds(1);
                }
            }
        }

        private static IEnumerator MultiLineProcess(string[] data)
        {
            foreach (string line in data)
            {
                Debug.Log($"Process Message '{line}'");
                yield return new WaitForSeconds(0.5f);
            }
        }

        private static IEnumerator QuickSave()
        {
            // Find the GameObject by its name
            GameObject saveGameObject = GameObject.Find("SaveLoad"); // Replace with your GameObject's name

            if (saveGameObject != null)
            {
                // Get the SaveScript component from the GameObject
                SaveScript saveScript = saveGameObject.GetComponent<SaveScript>();

                if (saveScript != null)
                {
                    // Call the QuickSave method
                    saveScript.PlotSave();
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
          
                
            yield return null;
        }
        private static IEnumerator DeadReason(string reason)
        {
           EndOption.DeathReason = reason; 
            yield return null;
        }
        
        private static IEnumerator UnActiveCharacter(string Character)
        {
            if (Character == "GG") Character = TestDialogueFiles.mainCharacter;
            if (Character == "SUP") Character = TestDialogueFiles.SupportCharacter;

            Transform character = GameObject.Find("Character1").transform;
            Image characterImage = character.GetComponent<Image>();
            string imageName = characterImage.sprite.name;
            string[] nameParts = imageName.Split('.');

            if (nameParts.Length > 0 && nameParts[0] == Character)
            {
                Color colorr = characterImage.color;
                colorr.a = 0.5f;
                characterImage.color = colorr;
            }
          

            Transform character2 = GameObject.Find("Character2").transform;
            Image characterImage2 = character2.GetComponent<Image>();
            string imageName2 = characterImage2.sprite.name;
            string[] nameParts2 = imageName2.Split('.');

            if (nameParts2.Length > 0 && nameParts2[0] == Character)
            {
                Color color2 = characterImage2.color;
                color2.a = 0.5f;
                characterImage2.color = color2;
            }

            yield return null;

        }

        private static IEnumerator Show(string[] objects)
        {

            GameObject obj = GameObject.Find(objects[0]);
            Debug.Log(obj);

            for (int i = 1; i < objects.Length; i++)
            {
                Debug.Log(objects[i]);
                Transform chosenObject = obj.transform.Find(objects[i]);
                chosenObject.gameObject.SetActive(true);
            }

            yield return null;
        }
        
        private static IEnumerator Hide(string objects)
        {
            GameObject targetObject = GameObject.Find(objects);
            targetObject.SetActive(false);
            yield return null;
        }

        private static IEnumerator GetAchiv(string objects)
        {
            if (objects != null)
            {
                if (PlayerPrefs.HasKey("End1"))
                {
                    // Load the saved value from PlayerPrefs
                    End1 = PlayerPrefs.GetString("End1");
                }
                else
                {
                    // If no saved value exists, set End1 to the default value
                    End1 = "End1";
                }

            
                if (End1 == objects && End1 != "" && End1 != null) 
                {
                    //End1 = "";
                    PlayerPrefs.SetString(End1, "");
                    PlayerPrefs.Save();
                }
            }
            else
            {
                Debug.Log(objects);
                Debug.LogError("Achivment hasnt found");
            }
            Debug.Log(objects);
            Debug.Log(End1);
            yield return null;
        }

        private static IEnumerator GetAchiv2(string objects)
        {
            if (objects != null)
            {
                if (PlayerPrefs.HasKey("End2"))
                {
                    // Load the saved value from PlayerPrefs
                    End2 = PlayerPrefs.GetString("End2");
                }
                else
                {
                    // If no saved value exists, set End1 to the default value
                    End2 = "End2";
                }


                if (End2 == objects && End2 != "" && End2 != null)
                {
                    //End1 = "";
                    PlayerPrefs.SetString(End2, "");
                    PlayerPrefs.Save();
                }
            }
            else
            {
                Debug.Log(objects);
                Debug.LogError("Achivment hasnt found");
            }
            Debug.Log(objects);
            Debug.Log(End2);
            yield return null;
        }

        private static IEnumerator GetAchiv3(string objects)
        {
            if (objects != null)
            {
                if (PlayerPrefs.HasKey("End3"))
                {
                    // Load the saved value from PlayerPrefs
                    End3 = PlayerPrefs.GetString("End3");
                }
                else
                {
                    // If no saved value exists, set End1 to the default value
                    End3 = "End3";
                }


                if (End3 == objects && End3 != "" && End3 != null)
                {
                    //End1 = "";
                    PlayerPrefs.SetString(End3, "");
                    PlayerPrefs.Save();
                }
            }
            else
            {
                Debug.Log(objects);
                Debug.LogError("Achivment hasnt found");
            }
            Debug.Log(objects);
            Debug.Log(End1);
            yield return null;
        }

        private static IEnumerator Chtext(string[] objects)
        {

            GameObject targetObject = GameObject.FindWithTag(objects[1]);

            if (targetObject == null)
            {
                Debug.LogError($"GameObject not found.");
                yield return null;
            }
            else
            {
                TextMeshProUGUI tmp = targetObject.GetComponent<TextMeshProUGUI>();
                tmp.text = objects[0];
            
            }
            yield return null;
        }



        private static IEnumerator ChangeAudio(string audioClipName, string folderPath, AudioSource audioSource)
        {

            AudioClip newClip = Resources.Load<AudioClip>($"{folderPath}{audioClipName}");
            if (newClip == null)
            {
                Debug.LogError($"AudioClip '{audioClipName}' not found in Resources/{folderPath}");
                yield return null;
            }

            // Change the audio clip and play it
            audioSource.clip = newClip;
            audioSource.Play();
            yield return null;
        }
        private static IEnumerator ChEndBut(string butName)
        {
            if (butName != null)
            {
                GameObject obj = GameObject.Find("Choose");
                Transform optEndTransform = obj.transform.Find("EndOption");
                Transform opt1Transform = obj.transform.Find(butName);

                if (optEndTransform != null && opt1Transform != null)
                {

                    Image optEndImage = optEndTransform.GetComponent<Image>();
                    Image opt1Image = opt1Transform.GetComponent<Image>();

                    optEndImage.sprite = opt1Image.sprite;


                    optEndTransform.position = opt1Transform.position;
                }
                else
                {
                    Debug.LogError("Image component is missing on one or both objects.");
                }

                Transform opt1TextTransform = opt1Transform.Find("Text (TMP)");
                Transform optEndTextTransform = optEndTransform.Find("Text (TMP)");
                if (opt1TextTransform != null && optEndTextTransform != null)
                {
                    TextMeshProUGUI opt1Text = opt1TextTransform.GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI optEndText = optEndTextTransform.GetComponent<TextMeshProUGUI>();


                    if (optEndText != null && opt1Text != null)
                    {
                        Debug.Log("HERE");
                        // Copy the text content
                        optEndText.text = opt1Text.text;
                    }
                    else
                    {
                        Debug.LogError("Text is missing");
                    }
                    
                }
                else
                {
                    Debug.LogError("TextMeshPro component is missing on one or both objects.");
                }
            }

            
            yield return null;
        }
        
        private static IEnumerator Chmus(string audioClipName)
        {
            GameObject targetObject = GameObject.Find("VNController");
            if (targetObject == null)
            {
                Debug.LogError($"VNController not found.");
                yield return null;
            }
            AudioSource audioSource = targetObject.GetComponent<AudioSource>();
            yield return ChangeAudio(audioClipName, "Audio/", audioSource);
        }

        private static IEnumerator Chsou(string audioClipName)
        {
            GameObject targetObject = GameObject.Find("SoundContoller");
            if (targetObject == null)
            {
                Debug.LogError($"VNController not found.");
                yield return null;
            }
            AudioSource audioSource = targetObject.GetComponent<AudioSource>();
            yield return ChangeAudio(audioClipName, "Audio/Sounds/", audioSource);
        }
        

         private static IEnumerator Invischar(string Character)
        {
           
            string position = Character;
            GameObject character = GameObject.Find("Character1");
            Image characterImage = character.GetComponent<Image>();

            GameObject character2 = GameObject.Find("Character2");
            Image characterImage2 = character2.GetComponent<Image>();

            Sprite newSprite = Resources.Load<Sprite>($"Characters/Empty");

            if (position == "L")
            {
                characterImage.sprite = newSprite;
            }


            if (position == "R")
            {
                characterImage2.sprite = newSprite;
            }

            yield return null;

        }

        private static IEnumerator Chchar(string[] Character)
        {

            string position = Character[0];

            GameObject character = GameObject.Find("Character1");
            Image characterImage = character.GetComponent<Image>();

            GameObject character2 = GameObject.Find("Character2");
            Image characterImage2 = character2.GetComponent<Image>();

            string Check = Character[1];
            string[] Check2 = Check.Split('.');

            if (Check2.Length > 0 && Check2[0] == "GG")
                Character[1] = TestDialogueFiles.mainCharacter + "." + Check2[1];

            if (Check2.Length > 0 && Check2[0] == "SUP")
                Character[1] = TestDialogueFiles.SupportCharacter + "." + Check2[1];


            if (Character[1] == "GG") Character[1] = TestDialogueFiles.mainCharacter;
            if (Character[1] == "SUP") Character[1] = TestDialogueFiles.SupportCharacter;

            Sprite newSprite = Resources.Load<Sprite>($"Characters/{Character[1]}");

            if (newSprite == null)
            {
                Debug.LogError($"Sprite '{Character[1]}' not found in Resources/Character.");
                yield return null;
            }


            if (position == "L")
            {
                characterImage.sprite = newSprite;
            }


            if (position == "R")
            {
                characterImage2.sprite = newSprite;
            }

          

            yield return null;
        }
     

private static IEnumerator Ñhscen(string objects)
          {
            PlayerInputManager.Instance.dynamicBool = false;
           
            GameObject ButtonMenu = GameObject.Find("ButtonsMenu");
            if (ButtonMenu != null)
            {
                ButtonMenu.SetActive(false);
            }
            string textureName = objects;
            GameObject background = GameObject.Find("Backgrund");
            if (background == null)
            {
                Debug.LogWarning($"Background not found!");
                yield break;
            }

            RawImage rawImage = background.GetComponent<RawImage>();
            if (rawImage == null)
            {
                Debug.LogWarning($"RawImage component not found on GameObject");
                yield break;
            }

            Texture newTexture = Resources.Load<Texture>($"Backgrounds/{textureName}");
            if (newTexture == null)
            {
                Debug.LogWarning($"Texture '{textureName}' not found in Resources/background folder!");
                yield break;
            }

            float fadeDuration = 1.0f;
            yield return FadeTexture(rawImage, newTexture, fadeDuration, ButtonMenu);
        }

        private static IEnumerator FadeTexture(RawImage rawImage, Texture nextTexture, float fadeDuration, GameObject ButtonMenu)
        {
            float elapsed = 0f;
          
            // Fade out to black
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                Color color = rawImage.color;
                color.a = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                rawImage.color = color;
                yield return null;
            }

            // Set the new texture
            rawImage.texture = nextTexture;

            // Fade in from black
            elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                Color color = rawImage.color;
                color.a = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
                rawImage.color = color;
                yield return null;
            }
            PlayerInputManager.Instance.dynamicBool = true;
            if (ButtonMenu != null)
            {
                ButtonMenu.SetActive(true);
            }
            yield return null;
        }

        private static IEnumerator MoveCharacter(string[] direction)
        {

            PlayerInputManager.Instance.dynamicBool = false;
            bool left = direction[0].ToLower() == "left";

            Transform character = GameObject.Find(direction[1]).transform;
            float moveSpeed = 15;
            int number = int.Parse(direction[2]);
            float targetX = left ? -number : number;

            float currentX = character.position.x;

            while (Mathf.Abs(targetX - currentX) > 0.1f)
            {
              //  Debug.Log($"Moving character to {(left ? "left" : "right")} [{currentX}/{targetX}]");
                currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
                character.position = new Vector3(currentX, character.position.y, character.position.z);
                yield return null;
            }
            PlayerInputManager.Instance.dynamicBool = true;
        }

        private static void ResetCharPos(string charpos)
        {
            string position = charpos;

            GameObject character = GameObject.Find("Character1");


            GameObject character2 = GameObject.Find("Character2");
  

            if (position == "L")
            {
              

            }


            if (position == "R")
            {
                Vector3 leftCorner = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
                float WidthOffset = Camera.main.orthographicSize * 0.05f; // 20% of screen height
                float heightOffset = Camera.main.orthographicSize * 0.25f; // 20% of screen height
                character2.transform.position = new Vector2(leftCorner.x + WidthOffset, leftCorner.y + heightOffset);
            }

        }
    }
}