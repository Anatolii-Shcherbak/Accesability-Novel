using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace DIALOGUE
{
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject root;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;
        public TextMeshProUGUI LogText;

        public void SetDialogueColor(Color color)=> dialogueText.color = color;
        public void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;
        public void SetNameColor(Color color) => nameText.color = color;
        public void SetNameFont(TMP_FontAsset font) => nameText.font = font;


        public void Show(string nameToShow = "")
        {
            if (nameToShow != string.Empty)
            {
                nameText.text = nameToShow;
            }
        }

        public void HistoryLog(string line)
        {
            LogText.text += "\n" + line;
            LogText.text += "\n";
        }
        public void Hide()
        {
            nameText.text = "";
        }
    }
}