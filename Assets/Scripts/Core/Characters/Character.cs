using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public abstract class Character
    {
        public string name = "";
        public RectTransform root = null;
        public CharacterConfigData config;
        public Character(string name, CharacterConfigData config)
        {
            this.name = name;
            this.config = config;
            
        }
        public enum CharacterType
        {
            Text,
            Sprite,
            SpriteSheet,
            Live2D
        }
    }
}