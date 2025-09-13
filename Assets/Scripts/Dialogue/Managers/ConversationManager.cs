using COMMANDS;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace DIALOGUE
{

    public class ConversationManager
    {
        private DialougeSystem dialougeSystem => DialougeSystem.instance;
        private Coroutine process = null;
        public bool isRunning => process != null;

        private TexktArchitekt architekt = null;
        private bool userPrompt = false;
        public ConversationManager(TexktArchitekt architekt)
        {
            this.architekt = architekt;
            dialougeSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }
        // Start is called before the first frame update
        public void StartConversation(List<string> conversation)
        {
                StopConversation();

            process = dialougeSystem.StartCoroutine(RunningConversation(conversation));
        }

        public void StopConversation() 
        {
            if (!isRunning)
            {
                return;
            }
            dialougeSystem.StopCoroutine(process);
                process = null;
           
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
           
            int startIndex = 0;
            string buttName;
            bool loadedd = false;
            DIALOGUE_LINE line1;
            UnityEngine.Debug.Log(SaveScript.Instance.Loaded);
            if (SaveScript.Instance.Loaded)
            {
                string savePath1 = Path.Combine(Application.persistentDataPath, "StaticData.json");

                string json1 = File.ReadAllText(savePath1);
                StatickSaveData dat1 = JsonUtility.FromJson<StatickSaveData>(json1);

                buttName = dat1.ButtName;
                loadedd = true;
                num();
                
            }
            void history(int index)
            {
                if (index > 0)
                {
                    for (int a = 0; a < index; a++)
                    {
                        string lines = conversation[a];
                        dialougeSystem.HistoryLog(lines);
                    }
                }
            }
            void num()
            {
                string savePath = Path.Combine(Application.persistentDataPath, "SavingData" + buttName + ".json");


                string json = File.ReadAllText(savePath);
                SavingData data = JsonUtility.FromJson<SavingData>(json);

                SaveScript.Instance.CurrLine = data.CurrLine1;

                startIndex = SaveScript.Instance.CurrLine;

                SaveScript.Instance.Loaded = false; // Prevent repetitive resets

                UnityEngine.Debug.Log($"Resuming from line {startIndex}");
                history(startIndex);
            }


            for  (int i = startIndex;  i < conversation.Count; i++)
                {
               

                if (string.IsNullOrWhiteSpace(conversation[i]))
                    continue;

                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);


                if (loadedd == true)
                {
                    loadedd = false;
                    if (!line.hasDialogue)
                    {
                        if (i - 1 >= 0)
                        {
                            line1 = DialogueParser.Parse(conversation[i - 1]);
                            yield return Line_RunDialogue(line1);
                        }
                        else if (i + 1 <= conversation.Count)
                        {
                            line1 = DialogueParser.Parse(conversation[i + 1]);
                            yield return Line_RunDialogue(line1);
                        }
                    
                    }
                }


                if (line.hasCommand)
                    yield return Line_RunCommands(line);


                if (!line.hasCommand)
                {
                    int currentLineIndex;
                    currentLineIndex = i; // записуємо індекс у змінну
                    SaveScript.Instance.CurrLine = i;
                    string lines = conversation[i];
                    dialougeSystem.HistoryLog(lines);

                }

                if (line.hasDialogue)
                    yield return Line_RunDialogue(line);

                if (line.hasDialogue)
                    yield return WaitForUserInput();


            }
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            if (line.hasSpeaker)
            {
                dialougeSystem.ShowSpeakerName(line.speakerData.displayname);
            }

           
            yield return BuildLineSegments(line.dialogueData);             
        }


        IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line)
        {
            for(int i = 0; i< line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];
               // UnityEngine.Debug.Log(line.segments[i]);
                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);

                yield return BuildDialogue(segment.dialogue, segment.appendText);
                    
            }
        }

        IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch (segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.c:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.a:
                    yield return WaitForUserInput();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.wc:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.wa:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                default:
                    break;
            }
        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            List<DL_COMMAND_DATA.Command> commands = line.commandsData.commands;

            foreach (DL_COMMAND_DATA.Command command in commands)
            {
                if (command.waitForCompletion)
                    yield return CommandManager.instance.Excute(command.name, command.arguments);
                else
                CommandManager.instance.Excute(command.name, command.arguments);
            }
            yield return null;
        }

        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            if(!append)
                architekt.Build(dialogue);
            else
                architekt.Append(dialogue);


            while (architekt.isBuilding)
            {
                if (userPrompt)
                {
                    if (!architekt.hurryUp)
                        architekt.hurryUp = true;
                    else
                        architekt.ForceComplete();

                    userPrompt = false;
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            while(!userPrompt)
                yield return null;

            userPrompt = false;
        }

    }
}
