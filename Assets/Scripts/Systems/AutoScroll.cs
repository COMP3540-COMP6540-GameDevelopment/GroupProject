using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    public ScrollRect scrollRect;  
    public float scrollSpeed = 0.05f; 
    private bool autoScroll = false; 

    public void StartAutoScroll()
    {
        autoScroll = true;
        scrollRect.verticalNormalizedPosition = 1; 
    }

    void Update()
    {
        if (autoScroll)
        {
            // Auto scroll
            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;

            // Stop auto-scrolling if it has reached the bottom
            if (scrollRect.verticalNormalizedPosition <= 0)
            {
                scrollRect.verticalNormalizedPosition = 0;
                autoScroll = false;
            }
        }
    }
}
