using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeZone : MonoBehaviour
{
    public Scene3LadderController ladder; // 引用梯子的控制脚本
    private int touchingObjectsCount = 0; // 记录接触物体的数量

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查进入的物体是否有 Tag 为 "Player" 或 "Box"
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            Debug.Log("Object Entered: " + other.name); // 输出调试信息
            touchingObjectsCount++;
            ladder.MoveUp(); // 梯子上升
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 检查离开的物体是否有 Tag 为 "Player" 或 "Box"
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            Debug.Log("Object Exited: " + other.name); // 输出调试信息
            touchingObjectsCount--;

            // 当没有物体接触时，梯子下落
            if (touchingObjectsCount <= 0)
            {
                touchingObjectsCount = 0; // 确保计数器不为负数
                ladder.MoveDown(); // 梯子下落
            }
        }
    }
}


