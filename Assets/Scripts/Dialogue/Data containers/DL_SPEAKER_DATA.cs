using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DIALOGUE
{
    public class DL_SPEAKER_DATA
    {
        public string name, castName;

        public string displayname => castName != string.Empty ? castName : name;

        public Vector2 castPosition;
        public List<(int layer, string expression)> CastExpressions { get; set; }

        private const string NAMECAST_ID = "as";
        private const string POSITIONCAST_ID = "at";
        private const string EXPRESSIONCAST_ID = "[";
        private const char AXISDELIMITER = ':';
        private const char EXPRESSIONLAUER_JOINER = ',';
        private const char EXPRESSIONLAUER_DELIMITER = ':';


        public DL_SPEAKER_DATA(string rawSpeaker)
        {
            string pattern = @$"{NAMECAST_ID} | {POSITIONCAST_ID} | {EXPRESSIONCAST_ID.Insert(EXPRESSIONCAST_ID.Length - 1, @"\")}";
            MatchCollection matches = Regex.Matches(rawSpeaker, pattern);

            castName = "";
            name = "";
            castPosition = Vector2.zero;
            CastExpressions = new List<(int layer, string expression)>();


            if (matches.Count == 0)
            {
                name = rawSpeaker;
                if (name == "GG")
                    name = TestDialogueFiles.mainCharacter;
                if (name == "SUP")
                    name = TestDialogueFiles.SupportCharacter;
                Lightup();
                return;
            }

            int index = matches[0].Index;
         
                
            name = rawSpeaker.Substring(0, index);

            if (name == "GG")
                name = TestDialogueFiles.mainCharacter;
            if(name == "SUP")
                name = TestDialogueFiles.SupportCharacter;

            void Lightup()
            {
                Transform character = GameObject.Find("Character1").transform;
                Image characterImage = character.GetComponent<Image>();
                string imageName = characterImage.sprite.name;
                string[] nameParts = imageName.Split('.');
    


                if (nameParts.Length > 0 && nameParts[0].Trim() == name.Trim())
                {
                    Color color = characterImage.color;
                    color.a = 1f;
                    characterImage.color = color;
                }
                else if (imageName.Trim() == name.Trim())
                {
                    Color color = characterImage.color;
                    color.a = 1f;
                    characterImage.color = color;
                }
                else
                {
                    Color color = characterImage.color;
                    color.a = 0.5f;
                    characterImage.color = color;
                }

                Transform character2 = GameObject.Find("Character2").transform;
                Image characterImage2 = character2.GetComponent<Image>();
                string imageName2 = characterImage2.sprite.name;
                string[] nameParts2 = imageName2.Split('.');

                if (nameParts2.Length > 0 && nameParts2[0].Trim() == name.Trim())
                {
                    Color color2 = characterImage2.color;
                    color2.a = 1f;
                    characterImage2.color = color2;
                }
                else if (imageName2.Trim() == name.Trim())
                {
                    Color color2 = characterImage2.color;
                    color2.a = 1f;
                    characterImage2.color = color2;
                }
                else
                {
                    Color color2 = characterImage2.color;
                    color2.a = 0.5f;
                    characterImage2.color = color2;
                }
            }

            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                int startIndex = 0, endIndex = 0;
                   
                if (match.Value.Trim() == NAMECAST_ID.Trim())
                {
                    Lightup();
                    startIndex = match.Index + NAMECAST_ID.Length;
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    castName = rawSpeaker.Substring(startIndex, endIndex - startIndex);
                }
                else if (match.Value.Trim() == POSITIONCAST_ID.Trim())
                {
                    startIndex = match.Index + POSITIONCAST_ID.Length;
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    string castPos = rawSpeaker.Substring(startIndex, endIndex - startIndex);

                    string[] axis = castPos.Split(AXISDELIMITER, System.StringSplitOptions.RemoveEmptyEntries);

                    float.TryParse(axis[0], out castPosition.x);

                    if (axis.Length > 1)
                        float.TryParse(axis[1], out castPosition.y);
                }
                else if (match.Value.Trim() == EXPRESSIONCAST_ID.Trim())
                {
                    startIndex = match.Index + EXPRESSIONCAST_ID.Length;
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    string castExp = rawSpeaker.Substring(startIndex, endIndex - (startIndex + 1));

                    CastExpressions = castExp.Split(EXPRESSIONLAUER_JOINER).Select(x =>
                    {
                        var parts = x.Trim().Split(EXPRESSIONLAUER_DELIMITER);
                        return (int.Parse(parts[0]), parts[1]);
                    }).ToList();
                }
                

            }
          
        }
    }
}