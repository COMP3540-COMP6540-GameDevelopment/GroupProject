using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WomanController : MonoBehaviour
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
    "Hero, I heard the town has prepared chests for you to supply your journey!",
    "The weather is great today, perfect for exploring the town.",
    "The chests contain supplies, but I heard the kids packed them, so some broken items might be inside.",
    "Hero, be cautious with the items from the chests; not everything might be reliable.",
    "If you find a chest, don't forget to check it carefully. Broken things won't be much help.",
    "Some say there are surprises hidden in the chests, but not every item might still be usable.",
    "May your journey go smoothly, and don't let faulty items from the chests dampen your spirit.",
    "The chests are gifts from the kids. They mean well, but use them with care.",
    "The townspeople hold great respect for heroes, so they prepared some chests for your supplies.",
    "The town's sky is beautiful, just like the adventure you're about to embark on!"
};
    
    private float dialogueInterval = 2f; // �Ի����ּ��ʱ��

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
        StartCoroutine(HideDialogueAfterSeconds(5f));
    }

    IEnumerator HideDialogueAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            dialogueText.text = "";  // ��նԻ�����
        }
}
