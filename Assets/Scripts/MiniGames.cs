using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGames : MonoBehaviour
{
    [Header("Left Sprites")]
    public SpriteRenderer[] leftSprites;

    [Header("Right Sprites")]
    public SpriteRenderer[] rightSprites;

    [Header("Sounds")]
    public AudioClip[] sounds;

    [Header("Line Prefab")]
    public LineRenderer linePrefab;

    [Header("Highlight Color")]
    public Color highlightColor = Color.yellow;

    private AudioSource audioSource;
    private LineRenderer[] spriteLines;
    private int[] connectedRight;
    private int draggingIndex = -1;
    private LineRenderer currentLine;
    private int currentlyHighlighted = -1;

    // >>> ADDED – Correct mapping
    private int[] correctMapping = new int[] { 5, 0, 3, 4, 2, 1 };


    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        spriteLines = new LineRenderer[leftSprites.Length];
        connectedRight = new int[leftSprites.Length];

        for (int i = 0; i < connectedRight.Length; i++)
            connectedRight[i] = -1;
    }

    void Update()
    {
        HandleMouseInput();
        UpdateDraggingLine();

        // >>> ADDED – Check automatically when all are connected
        CheckIfAllConnected();
    }

    // -------------------------------
    //   WHITE GRADIENT
    // -------------------------------
    Gradient CreateWhiteGradient()
    {
        Gradient g = new Gradient();
        g.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0f),
                new GradientColorKey(Color.white, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );
        return g;
    }

    // -------------------------------
    //       INPUT HANDLING
    // -------------------------------
    void HandleMouseInput()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        // Click left sprite
        for (int i = 0; i < leftSprites.Length; i++)
        {
            if (Vector2.Distance(mouseWorld, leftSprites[i].transform.position) < 0.5f)
            {
                OnLeftSpriteClicked(i);
                return;
            }
        }

        // Click to remove connection from right side
        for (int i = 0; i < connectedRight.Length; i++)
        {
            int r = connectedRight[i];
            if (r != -1 && rightSprites[r].bounds.Contains(mouseWorld))
            {
                RemoveConnection(i);
                return;
            }
        }
    }

    // -------------------------------
    //        DRAGGING UPDATE
    // -------------------------------
    void UpdateDraggingLine()
    {
        if (draggingIndex == -1 || currentLine == null) return;

        Vector3 startPos = leftSprites[draggingIndex].transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Remove previous highlight
        if (currentlyHighlighted != -1)
        {
            rightSprites[currentlyHighlighted].color = Color.white;
            currentlyHighlighted = -1;
        }

        int snapIndex = -1;

        // Check right sprites
        for (int i = 0; i < rightSprites.Length; i++)
        {
            // Skip already used right sprites
            if (System.Array.IndexOf(connectedRight, i) != -1)
                continue;

            if (rightSprites[i].bounds.Contains(mousePos))
            {
                snapIndex = i;
                rightSprites[i].color = highlightColor;
                currentlyHighlighted = i;
                break;
            }
        }

        Vector3 endPos = (snapIndex != -1)
            ? rightSprites[snapIndex].transform.position
            : mousePos;

        currentLine.SetPosition(0, startPos);
        currentLine.SetPosition(1, endPos);

        // Release mouse
        if (Input.GetMouseButtonUp(0))
        {
            if (snapIndex != -1)
            {
                connectedRight[draggingIndex] = snapIndex;
                rightSprites[snapIndex].color = Color.white;
            }
            else
            {
                RemoveConnection(draggingIndex);
            }

            draggingIndex = -1;
            currentLine = null;
        }
    }

    // -------------------------------
    //      START DRAGGING
    // -------------------------------
    void OnLeftSpriteClicked(int index)
    {
        if (spriteLines[index] != null)
        {
            RemoveConnection(index);
            return;
        }

        draggingIndex = index;
        Vector3 startPos = leftSprites[index].transform.position;

        // Create Line
        currentLine = Instantiate(linePrefab);

        // Setup as white
        currentLine.colorGradient = CreateWhiteGradient();

        // >>> CLEANUP – only assign once
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = Color.white;
        currentLine.sharedMaterial = mat;

        currentLine.useWorldSpace = true;
        currentLine.positionCount = 2;
        currentLine.SetPosition(0, startPos);
        currentLine.SetPosition(1, startPos);

        spriteLines[index] = currentLine;

        // Play sound
        if (sounds.Length > index)
        {
            audioSource.clip = sounds[index];
            audioSource.Play();
        }
    }

    // -------------------------------
    //     REMOVE CONNECTION
    // -------------------------------
    void RemoveConnection(int index)
    {
        if (spriteLines[index] != null)
        {
            Destroy(spriteLines[index].gameObject);
            spriteLines[index] = null;
        }

        connectedRight[index] = -1;
    }

    // -------------------------------
    //     CHECK ALL CONNECTED
    // -------------------------------
    void CheckIfAllConnected()
    {
        for (int i = 0; i < connectedRight.Length; i++)
            if (connectedRight[i] == -1)
                return; // Not finished yet

        // All connected → evaluate
        EvaluateConnections();
    }

    // -------------------------------
    //     EVALUATE RESULT
    // -------------------------------
    void EvaluateConnections()
    {
        // 1. Destroy all lines FIRST
        for (int i = 0; i < spriteLines.Length; i++)
        {
            if (spriteLines[i] != null)
            {
                Destroy(spriteLines[i].gameObject);
                spriteLines[i] = null;
            }
        }

        Debug.Log("Lines destroyed. Checking connections:");

        // 2. Now evaluate the connections
        int correctCount = 0;

        for (int left = 0; left < connectedRight.Length; left++)
        {
            int right = connectedRight[left];

            string leftName = leftSprites[left].name;
            string rightName = rightSprites[right].name;

            Debug.Log($"Left {left} ({leftName}) → Right {right} ({rightName})");

            if (right == correctMapping[left])
                correctCount++;
        }

        Debug.Log($"Correct: {correctCount}/{connectedRight.Length}");

        // 3. Clear connection data after checking
        for (int i = 0; i < connectedRight.Length; i++)
            connectedRight[i] = -1;
    }
}
