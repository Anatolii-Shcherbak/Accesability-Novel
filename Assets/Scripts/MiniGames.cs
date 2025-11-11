using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MiniGames : MonoBehaviour
{
    [Header("Left Sprites")]
    public SpriteRenderer[] leftSprites;

    [Header("Right Sprites")]
    public SpriteRenderer[] rightSprites; // "buttons", but actually sprites

    [Header("Sounds")]
    public AudioClip[] sounds;

    [Header("Line Prefab")]
    public LineRenderer linePrefab; // small width, white material, useWorldSpace = true

    [Header("Highlight Color")]
    public Color highlightColor = Color.yellow;

    private AudioSource audioSource;
    private LineRenderer[] spriteLines;
    private int[] connectedRight; // index of right sprite (-1 = none)
    private int draggingIndex = -1;
    private LineRenderer currentLine;
    private int currentlyHighlighted = -1;
    private Color[] originalRightColors;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        spriteLines = new LineRenderer[leftSprites.Length];
        connectedRight = new int[leftSprites.Length];
        for (int i = 0; i < connectedRight.Length; i++)
            connectedRight[i] = -1;

        // Save original colors
        originalRightColors = new Color[rightSprites.Length];
        for (int i = 0; i < rightSprites.Length; i++)
            originalRightColors[i] = rightSprites[i].color;
    }

    void Update()
    {
        HandleMouseInput();
        UpdateDraggingLine();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;

            // Click on left sprite → start dragging or remove connection
            for (int i = 0; i < leftSprites.Length; i++)
            {
                if (Vector2.Distance(mouseWorld, leftSprites[i].transform.position) < 0.5f)
                {
                    OnLeftSpriteClicked(i);
                    return;
                }
            }

            // Click on connected right sprite → remove connection
            for (int i = 0; i < connectedRight.Length; i++)
            {
                int rightIndex = connectedRight[i];
                if (rightIndex != -1)
                {
                    if (rightSprites[rightIndex].bounds.Contains(mouseWorld))
                    {
                        RemoveConnection(i);
                        return;
                    }
                }
            }
        }
    }

    void UpdateDraggingLine()
    {
        if (draggingIndex == -1 || currentLine == null) return;

        currentLine.startColor = Color.red;
        currentLine.endColor = Color.red;
        if (currentLine.material != null)
            currentLine.material.color = Color.red;

        Vector3 startPos = leftSprites[draggingIndex].transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Reset previous highlight
        if (currentlyHighlighted != -1)
        {
            rightSprites[currentlyHighlighted].color = originalRightColors[currentlyHighlighted];
            currentlyHighlighted = -1;
        }

        int snapIndex = -1;

        for (int i = 0; i < rightSprites.Length; i++)
        {
            // Already connected by other left sprite → skip
            bool alreadyConnected = false;
            for (int j = 0; j < connectedRight.Length; j++)
            {
                if (connectedRight[j] == i)
                {
                    alreadyConnected = true;
                    break;
                }
            }
            if (alreadyConnected) continue;

            // Use sprite bounds to check hover
            if (rightSprites[i].bounds.Contains(mousePos))
            {
                snapIndex = i;
                rightSprites[i].color = highlightColor;
                currentlyHighlighted = i;
                break;
            }
        }

        Vector3 endPos = (snapIndex != -1) ? rightSprites[snapIndex].transform.position : mousePos;

        currentLine.SetPosition(0, startPos);
        currentLine.SetPosition(1, endPos);

        currentLine.startColor = Color.red;
        currentLine.endColor = Color.red;
        if (currentLine.material != null)
            currentLine.material.color = Color.red;

        if (Input.GetMouseButtonUp(0))
        {
            if (snapIndex != -1)
            {
                connectedRight[draggingIndex] = snapIndex;
                Debug.Log($"Node {draggingIndex} connected to Right Sprite {snapIndex}");

                draggingIndex = -1;
                currentLine = null;

                rightSprites[snapIndex].color = originalRightColors[snapIndex];
                currentlyHighlighted = -1;
            }
            else
            {
                RemoveConnection(draggingIndex);
            }
        }
    }

    void OnLeftSpriteClicked(int index)
    {
        if (spriteLines[index] != null)
        {
            RemoveConnection(index);
            return;
        }


        draggingIndex = index;
        Vector3 startPos = leftSprites[index].transform.position;

        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        currentLine.useWorldSpace = true;
        currentLine.positionCount = 2;
        currentLine.SetPosition(0, startPos);
        currentLine.SetPosition(1, startPos);

        currentLine.startColor = Color.red;
        currentLine.endColor = Color.red;
        if (currentLine.material != null)
            currentLine.material.color = Color.red;

        spriteLines[index] = currentLine;

        if (sounds.Length > index)
        {
            audioSource.clip = sounds[index];
            audioSource.Play();
        }
    }

    void RemoveConnection(int index)
    {
        if (spriteLines[index] != null)
        {
            Destroy(spriteLines[index].gameObject);
            spriteLines[index] = null;
        }

        connectedRight[index] = -1;
        draggingIndex = -1;
        currentLine = null;

        Debug.Log($"Node {index} disconnected");
    }
}
