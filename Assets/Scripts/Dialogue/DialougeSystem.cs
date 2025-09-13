using CHARACTERS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DIALOGUE
{
    public class DialougeSystem : MonoBehaviour
    {
        [SerializeField]private DialogueSystemConfigurationSo _config;
        public DialogueSystemConfigurationSo config => _config;
        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager conversationManager;
        private TexktArchitekt architekt;

        public static DialougeSystem instance { get; private set; }
        
        public delegate void DialougeSystemEvent();
        public event DialougeSystemEvent onUserPrompt_Next;

        public bool isRunningConversation => conversationManager.isRunning;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Initialize();
            }
            else
                DestroyImmediate(gameObject);
        }

        bool _initialized = false;
        private void Initialize()
        {
            if (_initialized) return;

            architekt = new TexktArchitekt(dialogueContainer.dialogueText);
            conversationManager = new ConversationManager(architekt);
        }

        public void OnUserPrompt_Next()
        {
            onUserPrompt_Next?.Invoke();
        }

        public void ApplySpeakerDataToDialogueContainer(string speakerName)
        {
            Character character = CharacterManager.instance.GetCharacter(speakerName);
            CharacterConfigData config = character != null ? character.config : CharacterManager.instance.GetCharacterConfig(speakerName);

            ApplySpeakerDataToDialogueContainer(config);


        }

        public void HistoryLog(string line)
        {
            dialogueContainer.HistoryLog(line);
        }
        public void ApplySpeakerDataToDialogueContainer(CharacterConfigData config)
        {
            dialogueContainer.SetDialogueFont(config.dialogueFont);
            dialogueContainer.SetDialogueColor(config.dialogueColor);
            dialogueContainer.SetNameFont(config.nameFont);
            dialogueContainer.SetNameColor(config.nameColor);
        }


        public void ShowSpeakerName(string speakerName = "")
        {

            if(TestDialogueFiles.Languague == "EN")
            {
                if (speakerName == "GG")
                {
                    speakerName = TestDialogueFiles.mainCharacter;
                    dialogueContainer.Show(speakerName);
                }

                if (speakerName == "SUP")
                {
                    speakerName = TestDialogueFiles.SupportCharacter;
                    dialogueContainer.Show(speakerName);
                }
            }

           if (TestDialogueFiles.Languague == "UA")
            {
                if (speakerName == "GG")
                {
                    if (TestDialogueFiles.mainCharacter == "Rey")
                        speakerName = "Рей";
                    if (TestDialogueFiles.mainCharacter == "Mayua")
                        speakerName = "Мая";

                    dialogueContainer.Show(speakerName);
                }

                if (speakerName == "SUP")
                {
                    if (TestDialogueFiles.SupportCharacter == "Rey")
                        speakerName = "Рей";
                    if (TestDialogueFiles.SupportCharacter == "Mayua")
                        speakerName = "Мая";

                    dialogueContainer.Show(speakerName);
                }
            } 



            if (speakerName != "Narrator")
            {
                dialogueContainer.Show(speakerName);
            }
            else
                HideSpeakerName();
        }
       
        
        
        public void HideSpeakerName() => dialogueContainer.Hide();

    public void Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            Say(conversation);
        }
        public void Say(List<string> conversation)
        {
            conversationManager.StartConversation(conversation);    
        }

    }
}