using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class HUDManager : MonoBehaviour
{
    public VisualElement operatingElement;
    public static HUDManager instance;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        operatingElement = root.Q<VisualElement>("Operating");

        if (operatingElement == null)
        {
            Debug.LogError("Operating element not found in HUD.uxml!");
        }
    }
    public void showHUD(){
        if (operatingElement != null)
            {
                operatingElement.style.display =  DisplayStyle.Flex;
            }
    }

    public void hideHUD(){
        if (operatingElement != null)
            {
                operatingElement.style.display = DisplayStyle.None;
            }
    }
}
