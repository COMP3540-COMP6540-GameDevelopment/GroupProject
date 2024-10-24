using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeardedManController : MonoBehaviour
{
    public float speed = 2f;           // �ƶ��ٶ�
    public Transform pointA;           // Ѳ�ߵ����
    public Transform pointB;           // Ѳ�ߵ��յ�
    private Animator animator;         // ����������
    private Vector3 target;            // ��ǰ�ƶ�Ŀ��
    private bool isWalking = true;     // �Ƿ�������
    public float waitTime = 2f;        // ͣ��ʱ��
    private float waitCounter;         // ͣ����ʱ��

    public TextMeshProUGUI dialogueText;          // �󶨵ĶԻ�Text���
    
    public string[] dialogues = {// �Ի���������   
    "I like beer!",
    "&&@&##..."
    };
    
    private float dialogueInterval = 4f; // �Ի����ּ��ʱ��

    void Start()
    {
        animator = GetComponent<Animator>();
        target = pointB.position;       // ��ʼĿ����ΪB��
        waitCounter = waitTime;         // ��ʼ��ͣ����ʱ��
        UpdateFacingDirection();        // ��ʼ������
        UpdateDialogue();               // ��ʼ���Ի�����
        StartCoroutine(DialogueLoop()); // �����Ի�ѭ��
    }

    void Update()
    {
        if (isWalking)
        {
            MoveTowardsTarget();
        }
        else
        {
            // �ȴ���ʱ������ʱ�������������
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0f)
            {
                isWalking = true;
                waitCounter = waitTime;  // ���ü�ʱ��
                SwitchTarget();          // �л�Ŀ��
                UpdateFacingDirection(); // ���³���                         
            }
        }

        // ���¶���״̬
        animator.SetBool("isWalking", isWalking);
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // ����Ŀ����ֹͣ���ȴ�
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            isWalking = false;
        }
    }

    void SwitchTarget()
    {
        // ��A���B��֮���л�Ŀ��
        if (target == pointA.position)
        {
            target = pointB.position;
        }
        else
        {
            target = pointA.position;
        }
    }

    void UpdateFacingDirection()
    {
        // ����Ŀ��λ���뵱ǰ��ɫλ�ã�������ɫ�ĳ���
        Vector3 scale = transform.localScale;

        // ���Ŀ�����Ҳ࣬�����ң����򣩣��������࣬�����󣨸���
        if (target.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);  // ȷ����ɫ�����Ҳ�
            RotateText(0);  // ���ֱ���Ĭ�Ϸ���
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x); // ȷ����ɫ�������
            RotateText(180);  // ������ת180��
        }

        transform.localScale = scale; // ���½�ɫ������
    }

    // �������ֵ���ת
    void RotateText(float rotationAngle)
    {
        if (dialogueText != null)
        {
            // ��ת���֣�ʹ���ڽ�ɫ��תʱ��ת180��
            dialogueText.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
        }
    }

    IEnumerator DialogueLoop()
    {
        while (true)
        {
            UpdateDialogue(); // ���¶Ի�����
            yield return new WaitForSeconds(dialogueInterval); // �ȴ����
        }
    }

    void UpdateDialogue()
    {
        // ���ѡ��һ���Ի�����
        int randomIndex = Random.Range(0, dialogues.Length);
        dialogueText.text = dialogues[randomIndex];

        // ����Э�������ضԻ�
        StartCoroutine(HideDialogueAfterSeconds(8f));
    }

    IEnumerator HideDialogueAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            dialogueText.text = "";  // ��նԻ�����
        }
}
