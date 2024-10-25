using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3LadderController : MonoBehaviour
{
    
    public float moveSpeed = 2.0f; 
    public float moveDistance = 3.0f; 
    private Vector3 originalPosition;

    private void Start()
    {
        // Record initial position of ladder
        originalPosition = transform.position;
    }

    public void MoveUp()
    {
        // Ladder rise to desired position
        Vector3 targetPosition = originalPosition + new Vector3(0, moveDistance, 0);
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(targetPosition));
    }

    public void MoveDown()
    {
        // Ladder move back to original position
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(originalPosition));
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 target)
    {
        // Smooth move
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target; 
    }
}
