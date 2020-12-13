using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerStatSystem
{
    [System.Serializable]
    public class Stats
    { 
    
        public int damage;
        public float aheadDelay;
        public float attackRate;
        public float recogRange;
        public float attackRange;
        public float splashRange;


        public void Copy(Stats other)
        {
            damage = other.damage;
            aheadDelay = other.aheadDelay;
            attackRate = other.attackRate;
            recogRange = other.recogRange;
            attackRange = other.attackRange;
            splashRange = other.splashRange;
        }

        public void Modify(StatModifier modifier)
        {
            if (modifier.ModifierMode == StatModifier.Mode.Percentage)
            {
                damage += Mathf.FloorToInt(damage * ((float)modifier.Stats.damage / 100.0f));
                aheadDelay += aheadDelay * (modifier.Stats.aheadDelay / 100.0f);
                attackRate += attackRate * (modifier.Stats.attackRate / 100.0f);
                recogRange += recogRange * (modifier.Stats.recogRange / 100.0f);
                attackRange += attackRange * (modifier.Stats.attackRange / 100.0f);
                splashRange += splashRange * (modifier.Stats.splashRange / 100.0f);
            }
            else
            {
                damage += modifier.Stats.damage;
                aheadDelay += modifier.Stats.aheadDelay;
                attackRate += modifier.Stats.attackRate;
                recogRange += modifier.Stats.recogRange;
                attackRange += modifier.Stats.attackRange;
                splashRange += modifier.Stats.splashRange;
            }
        }
    }

    [System.Serializable]
    public class StatModifier
    {
        public enum Mode
        {
            Percentage,
            Absolute
        }

        public Mode ModifierMode = Mode.Absolute;
        public Stats Stats = new Stats();
    }

    [System.Serializable]
    public class TimedStatModifier
    {
        public string Id;
        public StatModifier Modifier;

        public Sprite EffectSprite;

        public float Duration;
        public float Timer;

        public void Reset()
        {
            Timer = Duration;
        }
    }

    public Stats baseStats;
    public Stats stats { get; set; } = new Stats();

    public List<TimedStatModifier> TimedModifierStack => m_TimedModifierStack;

    private TowerData m_Owner;

    List<StatModifier> m_ModifiersStack = new List<StatModifier>();
    List<TimedStatModifier> m_TimedModifierStack = new List<TimedStatModifier>();

    public void Init(TowerData owner)
    {
        stats.Copy(baseStats);
        m_Owner = owner;
    }

    public void AddModifier(StatModifier modifier)
    {
        m_ModifiersStack.Add(modifier);
        UpdateFinalStats();
    }

    public void RemoveModifier(StatModifier modifier)
    {
        m_ModifiersStack.Remove(modifier);
        UpdateFinalStats();
    }

    public void AddTimedModifier(StatModifier modifier, float duration, string id, Sprite sprite,
        SoundManager.SkillSoundSpecific skillSoundSpecific, string apply_sound_name)
    {
        bool found = false;
        int index = m_TimedModifierStack.Count;
        for (int i = 0; i < m_TimedModifierStack.Count; ++i)
        {
            if (m_TimedModifierStack[i].Id == id)
            {
                found = true;
                index = i;
            }
        }

        if (!found)
        {
            m_TimedModifierStack.Add(new TimedStatModifier() { Id = id });
            if (SoundManager.Instance != null)
                SoundManager.instance.PlaySkillSound(skillSoundSpecific, apply_sound_name);
        }

        m_TimedModifierStack[index].EffectSprite = sprite;
        m_TimedModifierStack[index].Duration = duration;
        m_TimedModifierStack[index].Modifier = modifier;
        m_TimedModifierStack[index].Reset();

        UpdateFinalStats();
    }

    public void Tick()
    {
        bool needUpdate = false;

        for (int i = 0; i < m_TimedModifierStack.Count; ++i)
        {
            //permanent modifier will have a timer == -1.0f, so jump over them
            if (m_TimedModifierStack[i].Timer > 0.0f)
            {
                m_TimedModifierStack[i].Timer -= Time.deltaTime;
                if (m_TimedModifierStack[i].Timer <= 0.0f)
                {//modifier finished, so we remove it from the stack
                    m_TimedModifierStack.RemoveAt(i);
                    i--;
                    needUpdate = true;
                }
            }
        }

        if (needUpdate)
        {
            UpdateFinalStats();
        }
    }

    void UpdateFinalStats()
    {
        stats.Copy(baseStats);

        foreach (var modifier in m_ModifiersStack)
        {
            stats.Modify(modifier);
        }

        foreach (var timedModifier in m_TimedModifierStack)
        {
            stats.Modify(timedModifier.Modifier);
        }
    }
}
