using COMMANDS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTesting : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            CommandManager.instance.Excute("moveChar", "left");
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            CommandManager.instance.Excute("moveChar", "right");

    }

    // Start is called before the first frame update
    void Start()
    {
    //    StartCoroutine(Running());


    }
    IEnumerator Running()
    {
      yield return  CommandManager.instance.Excute("print");
        yield return CommandManager.instance.Excute("print_1p", "Hello world!");
        yield return CommandManager.instance.Excute("print_mp", "line1", "line2", "line3");

        yield return CommandManager.instance.Excute("lambda");
        yield return CommandManager.instance.Excute("lambda_1p", "Hello lambda!");
        yield return CommandManager.instance.Excute("lambda_mp", "lambda1", "lambda2", "lambda3");

        yield return CommandManager.instance.Excute("process");
        yield return CommandManager.instance.Excute("process_1p", "3");
        yield return CommandManager.instance.Excute("process_mp", "process 1", "process 2", "process 3");
    }
   
}
