using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class EnemyStatSystem
{
    [System.Serializable]
    public class Stats
    {
        public int hp;
        public float speed;
        public int def;

        public void Copy(Stats other)
        {
            hp = other.hp;
            speed = other.speed;
            def = other.def;
        }


        public void Modify(StatModifier modifier)
        {
            if (modifier.ModifierMode == StatModifier.Mode.Percentage)
            {
                hp += Mathf.FloorToInt(hp * ((float)modifier.Stats.hp / 100.0f));
                speed += Mathf.FloorToInt(speed * (modifier.Stats.speed / 100.0f));
                def += Mathf.FloorToInt(def * ((float)modifier.Stats.def / 100.0f));
            }
            else
            {
                hp += modifier.Stats.hp;
                speed += modifier.Stats.speed;
                def += modifier.Stats.def;
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
    public StatModifier baseStatModifier;
    public Stats stats { get; set; } = new Stats();

    public int currentHp { get; private set; }
    //public List<BaseElementalEffect> ElementalEffects => m_ElementalEffects;
    public List<TimedStatModifier> TimedModifierStack => m_TimedModifierStack;

    EnemyData m_Owner;

    List<StatModifier> m_ModifiersStack = new List<StatModifier>();
    List<TimedStatModifier> m_TimedModifierStack = new List<TimedStatModifier>();
    //List<BaseElementalEffect> m_ElementalEffects = new List<BaseElementalEffect>();

    public void Init(EnemyData owner)
    {
        stats.Copy(baseStats);
        currentHp = stats.hp;
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


    public void AddTimedModifier(StatModifier modifier, float duration, string id, Sprite sprite)
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
        }

        m_TimedModifierStack[index].EffectSprite = sprite;
        m_TimedModifierStack[index].Duration = duration;
        m_TimedModifierStack[index].Modifier = modifier;
        m_TimedModifierStack[index].Reset();

        UpdateFinalStats();
    }


    /*
    public void AddElementalEffect(BaseElementalEffect effect)
    {
        effect.Applied(m_Owner);

        bool replaced = false;
        for (int i = 0; i < m_ElementalEffects.Count; ++i)
        {
            if (effect.Equals(m_ElementalEffects[i]))
            {
                replaced = true;
                m_ElementalEffects[i].Removed();
                m_ElementalEffects[i] = effect;
            }
        }

        if (!replaced)
            m_ElementalEffects.Add(effect);
    }
    */


    public void Death()
    {
        /*
        foreach (var e in ElementalEffects)
            e.Removed();

        ElementalEffects.Clear();
        */
        TimedModifierStack.Clear();

        UpdateFinalStats();
    }


    public void Tick()
    {
        Debug.Log("check");
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
            UpdateFinalStats();
        /*
        for (int i = 0; i < m_ElementalEffects.Count; ++i)
        {
            var effect = m_ElementalEffects[i];
            effect.Update(this);

            if (effect.Done)
            {
                m_ElementalEffects[i].Removed();
                m_ElementalEffects.RemoveAt(i);
                i--;
            }
        }
        */
    }

    
    public void Changehp(int amount)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, stats.hp);
        if (currentHp <= 0)
            m_Owner.Death();
    }

    void UpdateFinalStats()
    {
        bool maxhpChange = false;
        int previoushp = stats.hp;

        stats.Copy(baseStats);

        foreach (var modifier in m_ModifiersStack)
        {
            if (modifier.Stats.hp != 0)
                maxhpChange = true;

            stats.Modify(modifier);
        }

        foreach (var timedModifier in m_TimedModifierStack)
        {
            if (timedModifier.Modifier.Stats.hp != 0)
                maxhpChange = true;

            stats.Modify(timedModifier.Modifier);
        }

        //if we change the max hp we update the current hp to it's new value
        if (maxhpChange)
        {
            float percentage = currentHp / (float)previoushp;
            currentHp = Mathf.RoundToInt(percentage * stats.hp);
        }
    }

    public void Damage(int damage)
    {
        int totalDamage = damage - stats.def;

        Changehp(-totalDamage);
        //DamageUI.Instance.NewDamage(totalDamage, m_Owner.transform.position);
    }
}