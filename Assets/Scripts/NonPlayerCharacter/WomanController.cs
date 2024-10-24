using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WomanController : MonoBehaviour
{
    public float speed = 2f;           // 移动速度
    public Transform pointA;           // 巡逻的起点
    public Transform pointB;           // 巡逻的终点
    private Animator animator;         // 动画控制器
    private Vector3 target;            // 当前移动目标
    private bool isWalking = true;     // 是否在行走
    public float waitTime = 2f;        // 停留时间
    private float waitCounter;         // 停留计时器

    public TextMeshProUGUI dialogueText;          // 绑定的对话Text组件
    
    public string[] dialogues = {// 对话内容数组
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
    
    private float dialogueInterval = 2f; // 对话出现间隔时间

    void Start()
    {
        animator = GetComponent<Animator>();
        target = pointB.position;       // 初始目标设为B点
        waitCounter = waitTime;         // 初始化停留计时器
        UpdateFacingDirection();        // 初始化方向
        UpdateDialogue();               // 初始化对话内容
        StartCoroutine(DialogueLoop()); // 启动对话循环
    }

    void Update()
    {
        if (isWalking)
        {
            MoveTowardsTarget();
        }
        else
        {
            // 等待计时，倒计时结束后继续行走
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0f)
            {
                isWalking = true;
                waitCounter = waitTime;  // 重置计时器
                SwitchTarget();          // 切换目标
                UpdateFacingDirection(); // 更新朝向                         
            }
        }

        // 更新动画状态
        animator.SetBool("isWalking", isWalking);
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // 到达目标点后停止并等待
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            isWalking = false;
        }
    }

    void SwitchTarget()
    {
        // 在A点和B点之间切换目标
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
        // 根据目标位置与当前角色位置，调整角色的朝向
        Vector3 scale = transform.localScale;

        // 如果目标在右侧，面向右（正向），如果在左侧，面向左（负向）
        if (target.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);  // 确保角色面向右侧
            RotateText(0);  // 文字保持默认方向
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x); // 确保角色面向左侧
            RotateText(180);  // 文字旋转180度
        }

        transform.localScale = scale; // 更新角色的缩放
    }

    // 控制文字的旋转
    void RotateText(float rotationAngle)
    {
        if (dialogueText != null)
        {
            // 旋转文字，使其在角色翻转时旋转180度
            dialogueText.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
        }
    }

    IEnumerator DialogueLoop()
    {
        while (true)
        {
            UpdateDialogue(); // 更新对话内容
            yield return new WaitForSeconds(dialogueInterval); // 等待间隔
        }
    }

    void UpdateDialogue()
    {
        // 随机选择一个对话内容
        int randomIndex = Random.Range(0, dialogues.Length);
        dialogueText.text = dialogues[randomIndex];

        // 启动协程来隐藏对话
        StartCoroutine(HideDialogueAfterSeconds(5f));
    }

    IEnumerator HideDialogueAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            dialogueText.text = "";  // 清空对话内容
        }
}
