using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public class Character_text : Character
    {
        public Character_text(string name, CharacterConfigData config) : base(name, config)
        {
            Debug.Log($"Created Text Character : '{name}'");
        }
    }
}