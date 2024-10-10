using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public bool isGameActive;
    private float time = 999;
    public TextMeshProUGUI timeText;
    // Start is called before the first frame update
    void Start()
    {
        isGameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0 && isGameActive)
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                time = 0;
            }
            UpdateTime();
        }
        else if (time <= 0)
        {
            Debug.Log("Time's up!");
        }
    }

    public void UpdateTime(){
        int seconds = Mathf.FloorToInt(time);
        timeText.text = "Time: " + seconds;
    }
}
