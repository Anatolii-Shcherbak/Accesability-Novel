using System.Collections;
using UnityEngine;
using TMPro;
public class TexktArchitekt
{
    private TextMeshProUGUI tmpro_ui;
    private TextMeshPro tmpro_world;
    public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui : tmpro_world;

    public string currentText => tmpro.text;
    public string targetText { get; private set; } = "";
    public string preText { get; private set; } = "";
    private int preTextLength = 0;

    public string fullTargetText => preText + targetText;

    public enum BuildMethod { instant, typewriter, fade }
    public BuildMethod buildMethod = BuildMethod.typewriter;

    public Color textColor { get { return tmpro.color; } set { tmpro.color = value; } }

    public float speed { get { return baseSpeed * speedMultiplier; } set { speedMultiplier = value; } }
    private const float baseSpeed = 1;
    private float speedMultiplier = 1;
    private int characterMultiplier = 1;
    public bool hurryUp = false;
    public int charactersPerCycle { get { return speed <= 2f ? characterMultiplier : speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3; } }

    public TexktArchitekt(TextMeshProUGUI tmpro_ui)
    {
        this.tmpro_ui = null;
        this.tmpro_ui = tmpro_ui;

    }

    public TexktArchitekt(TextMeshPro tmpro_world)
    {
        this.tmpro_world = null;
        this.tmpro_world = tmpro_world;
    }

    public Coroutine Build(string text)
    {
        preText = "";
        targetText = "";
        targetText = text;

        Stop();

        buildProccess = tmpro.StartCoroutine(Building());
        return buildProccess;
    }

    public Coroutine Append(string text)
    {
        preText = "";
        targetText = "";
        preText = tmpro.text;
        targetText = text;

        Stop();

        buildProccess = tmpro.StartCoroutine(Building());
        return buildProccess;
    }

    private Coroutine buildProccess = null;
    public bool isBuilding => buildProccess != null;

    public void Stop()
    {
        if (!isBuilding)
            return;
        tmpro.StopCoroutine(buildProccess);
        buildProccess = null;
    }

    IEnumerator Building()
    {
        Prepare();
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                yield return Build_Typewriter();
                break;
            case BuildMethod.fade:
                yield return Build_Fade();
                break;
        }
        OnComplete();
    }

    private void OnComplete()
    {
        buildProccess = null;
        hurryUp = false;
    }

    public void ForceComplete()
    {
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
            case BuildMethod.fade:
                tmpro.ForceMeshUpdate();
                break;
        }
        Stop();
        OnComplete();
    }

    private void Prepare()
    {
        switch (buildMethod)
        {
            case BuildMethod.instant:
                Prepare_Instant();
                break;
            case BuildMethod.typewriter:
                Prepare_Typerwriter();
                break;
            case BuildMethod.fade:
                Prepare_Fade();
                break;
        }
    }

    private void Prepare_Instant()
    {
        tmpro.color = tmpro.color;
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate();
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
    }

    private void Prepare_Typerwriter()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;

        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }
        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }

    private void Prepare_Fade()
    {
        tmpro.text = preText;
        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            preTextLength = tmpro.textInfo.characterCount;
        }
        else
        {
            preTextLength = 0;
        }

        tmpro.text += targetText;
        tmpro.maxVisibleCharacters = int.MaxValue;
        tmpro.ForceMeshUpdate();

        TMP_TextInfo textInfo = tmpro.textInfo;

        Color colorVisable = new Color(textColor.r, textColor.g, textColor.b, 1);
        Color colorHidden = new Color(textColor.r, textColor.g, textColor.b, 0);

        Color32[] vertexColors = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo characterInfo = textInfo.characterInfo[i];

            if (!characterInfo.isVisible)
                continue;
            if (i < preTextLength)
            {
                for (int v = 0; v < 4; v++)
                    vertexColors[characterInfo.vertexIndex + v] = colorVisable;
            }
            else
            {
                for (int v = 0; v < 4; v++)
                    vertexColors[characterInfo.vertexIndex + v] = colorHidden;
            }

        }
        tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    private IEnumerator Build_Typewriter()
    {
        while (tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += hurryUp ? charactersPerCycle * 5 : charactersPerCycle;

            yield return new WaitForSeconds(0.015f / speed);
        }
    }

    private IEnumerator Build_Fade()
    {
        int minRage = preTextLength;
        int maxRage = minRage + 1;

        byte alphaTreshold = 15;

        TMP_TextInfo textInfo = tmpro.textInfo;
        Color32[] vertexColors = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;
        float[] alpas = new float[textInfo.characterCount];
        while (true)
        {
            float fadeSpeed = ((hurryUp ? charactersPerCycle * 5 : charactersPerCycle) * speed) * 4f;
            for (int i = minRage; i < maxRage; i++)
            {
                TMP_CharacterInfo characterInfo = textInfo.characterInfo[i];

                if (!characterInfo.isVisible)
                    continue;

                int vertrxIndex = textInfo.characterInfo[i].vertexIndex;
                alpas[i] = Mathf.MoveTowards(alpas[i], 255, fadeSpeed);

                for (int v = 0; v < 4; v++)
                    vertexColors[characterInfo.vertexIndex + v].a = (byte)alpas[i];

                if (alpas[i] >= 255)
                    minRage++; 
            }

            tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            bool lastCharacterIsInvisible = !textInfo.characterInfo[maxRage-1].isVisible;
            if (alpas[maxRage-1] > alphaTreshold || lastCharacterIsInvisible)
            {
                if (maxRage < textInfo.characterCount)
                    maxRage++;
                else if (alpas[maxRage - 1] >= 255 || lastCharacterIsInvisible)
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}