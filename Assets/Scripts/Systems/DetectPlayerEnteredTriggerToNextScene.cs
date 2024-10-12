using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerEnteredTriggerToNextScene : MonoBehaviour
{
    public string SceneToLoadName;
    public bool isFromRightToLeft;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log($"Player entered TriggerToNextScene, send {SceneToLoadName} to SceneManagerScript");
                if (SceneToLoadName != "")
                {
                    SceneManagerScript.instance.nextScene = SceneToLoadName;
                    SceneManagerScript.instance.isFromRightToLeft = isFromRightToLeft;
                }
            }  
        }
    }
}
