using CHARACTERS;
using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance { get; private set; }
    private Dictionary<string, Character> characters = new Dictionary<string, Character>();

    private CharacterConfigSo config => DialougeSystem.instance.config.characterConfigurarionAsset;

    private void Awake()
    {
        instance = this;
    }

    public CharacterConfigData GetCharacterConfig(string characterName)
    {
        return config.GetConfig(characterName);
    }
     
    public Character GetCharacter(string characterName, bool createIfDoesNotExist =false)
    {
        if (characters.ContainsKey(characterName.ToLower()))
            return characters[characterName.ToLower()];
        else if (createIfDoesNotExist)
            return CreateCharacter(characterName);

        return null;
    }

    public Character CreateCharacter(string characterName)
    {
        if (characters.ContainsKey(characterName.ToLower()))
        {
            Debug.LogWarning($"A Character called '{characterName}' already exists. Did not create the chaacter ");
            return null;

        }
        CHARACTER_INFO info = GetCharacterInfo(characterName);

        Character character = CreateCharacterFromInfo(info);

        characters.Add(characterName.ToLower(), character);

        return character;
    }
    private CHARACTER_INFO GetCharacterInfo(string characterName)
    {
        CHARACTER_INFO result = new CHARACTER_INFO();

        result.name = characterName;

        result.config = config.GetConfig(characterName);

        return result;
    }

    private Character CreateCharacterFromInfo(CHARACTER_INFO info)
    {
        CharacterConfigData config = info.config;

        switch (config.characterType)
        {
            case Character.CharacterType.Text:
                return new Character_text(info.name, config);

            case Character.CharacterType.Sprite:
            case Character.CharacterType.SpriteSheet:
                return new Character_Sprite(info.name, config);

            case Character.CharacterType.Live2D:
                return new Character_Live2D(info.name, config);

            default: return null;
        }
    }
    private class CHARACTER_INFO
    {
        public string name = "";

        public CharacterConfigData config;

    }
}
