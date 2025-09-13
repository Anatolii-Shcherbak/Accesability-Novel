using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Character Maya = CharacterManager.instance.CreateCharacter("Mayua");
            Character Rey = CharacterManager.instance.CreateCharacter("Rey");

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}