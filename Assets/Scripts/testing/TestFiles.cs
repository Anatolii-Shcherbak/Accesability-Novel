using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFiles : MonoBehaviour
{
    [SerializeField] private TextAsset fileName;

    private void Start()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        List<string> Lines = FileManager.ReadTextAsset(fileName, false);

        foreach (string line in Lines)
        {
            Debug.Log(line);

            yield return line;
        }
    }
}
