using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3LadderController : MonoBehaviour
{
    
    public float moveSpeed = 2.0f; // 梯子移动速度
    public float moveDistance = 3.0f; // 梯子上升的距离
    private Vector3 originalPosition; // 梯子的初始位置

    private void Start()
    {
        // 记录梯子的原始位置
        originalPosition = transform.position;
    }

    public void MoveUp()
    {
        // 梯子上升到目标位置
        Vector3 targetPosition = originalPosition + new Vector3(0, moveDistance, 0);
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(targetPosition));
    }

    public void MoveDown()
    {
        // 梯子回到原始位置
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(originalPosition));
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 target)
    {
        // 逐步移动到目标位置，实现平滑移动
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target; // 最终确保梯子在目标位置
    }
}
