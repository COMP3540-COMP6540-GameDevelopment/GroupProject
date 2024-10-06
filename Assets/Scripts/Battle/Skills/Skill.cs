using System.Collections;
using UnityEngine;
public enum SkillType { DEFAULT, PHYSICAL, MAGICAL, HEAL, BUFF, DEBUFF }

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public SkillType skillType;
    public string description;
    public int potency;
    public int costMP;
    public ParticleSystem particleSystem;
}
