using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistorySetting : MonoBehaviour
{
    public ScrollRect scrollRect;
      void Awake()
    {
        //scrollRect = GetComponent<ScrollRect>();
        ScrollToBottomInstant();

    }
    public void ScrollToBottomInstant()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
       }
    // Start is called before the first frame update
 
}
