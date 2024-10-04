using System.Collections;
using UnityEngine;
public enum SkillType { PHYSICAL , MAGICAL}

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public SkillType skillType;
    public string description;
    public int potency;
    public int costMP;
}
