using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class EnemyStatSystem
{
    /// <summary>
    /// Store the stats, which are composed of 4 values : hp, speed, agility and def.
    /// It also contains elemental protections and boost (1 for each elements defined by the DamageType enum)
    /// </summary>
    [System.Serializable]
    public class Stats
    {
        //Integer for simplicity, may switch to float later on. For now everything is integer
        public int hp;
        public int speed;
        public int def;

        public void Copy(Stats other)
        {
            hp = other.hp;
            speed = other.speed;
            def = other.def;
        }

        /// <summary>
        /// Will modify that Stat by the given StatModifier (see StatModifier documentation for how to use them)
        /// </summary>
        /// <param name="modifier"></param>
        public void Modify(StatModifier modifier)
        {
            //bit convoluted, but allow to reuse the normal int stat system for percentage change
            if (modifier.ModifierMode == StatModifier.Mode.Percentage)
            {
                hp += Mathf.FloorToInt(hp * (modifier.Stats.hp / 100.0f));
                speed += Mathf.FloorToInt(speed * (modifier.Stats.speed / 100.0f));
                def += Mathf.FloorToInt(def * (modifier.Stats.def / 100.0f));
            }
            else
            {
                hp += modifier.Stats.hp;
                speed += modifier.Stats.speed;
                def += modifier.Stats.def;
            }
        }
    }

    /// <summary>
    /// Can be added to a stack of modifiers on the StatSystem to modify the value of the base stats
    /// e.g. a weapon adding +2 speed will push a modifier on the top of the stack.
    ///
    /// They have 2 modes : Absolute, where values are added as is, and Percentage, where values are converted to
    /// percentage (e.g. a value of 50 in speed in a Percentage modifier will increase the speed by 50%).
    /// </summary>
    [System.Serializable]
    public class StatModifier
    {
        /// <summary>
        /// The mode of the modifier : Percentage will divide the value by 100 to get a percentage, absolute use the
        /// value as is.
        /// </summary>
        public enum Mode
        {
            Percentage,
            Absolute
        }

        public Mode ModifierMode = Mode.Absolute;
        public Stats Stats = new Stats();
    }

    /// <summary>
    /// This is a special StatModifier, that gets added to the TimedStatModifier stack, that will be automatically
    /// removed when its timer reaches 0. Contains a StatModifier that controls the actual modification.
    /// </summary>
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


    public int Currenthp { get; private set; }
    //public List<BaseElementalEffect> ElementalEffects => m_ElementalEffects;
    public List<TimedStatModifier> TimedModifierStack => m_TimedModifierStack;

    EnemyData m_Owner;

    List<StatModifier> m_ModifiersStack = new List<StatModifier>();
    List<TimedStatModifier> m_TimedModifierStack = new List<TimedStatModifier>();
    //List<BaseElementalEffect> m_ElementalEffects = new List<BaseElementalEffect>();

    public void Init(EnemyData owner)
    {
        stats.Copy(baseStats);
        Currenthp = stats.hp;
        m_Owner = owner;
    }

    /// <summary>
    /// Add a modifier to the end of the stack. This will recompute the Stats so it now include the new modifier.
    /// </summary>
    /// <param name="modifier"></param>
    public void AddModifier(StatModifier modifier)
    {
        m_ModifiersStack.Add(modifier);
        UpdateFinalStats();
    }

    /// <summary>
    /// Remove a modifier from the stack. This modifier need to already be on the stack. e.g. used by the equipment
    /// effect that store the modifier they add on equip and remove it when unequipped.
    /// </summary>
    /// <param name="modifier"></param>
    public void RemoveModifier(StatModifier modifier)
    {
        m_ModifiersStack.Remove(modifier);
        UpdateFinalStats();
    }

    /// <summary>
    /// Add a Timed modifier. Timed modifier does not stack and instead re-adding the same type of modifier will just
    /// reset the already existing one timer to the given duration. That the use of the id parameter : it need to be
    /// shared by all timed effect that are the "same type". i.e. an effect that add speed can use "speedTimed"
    /// as id, so if 2 object try to add that effect, they won't stack but instead just refresh the timer.
    /// </summary>
    /// <param name="modifier">A StatModifier container the wanted modification</param>
    /// <param name="duration">The time during which that modification will be active.</param>
    /// <param name="id">A name that identify that type of modification. Adding a timed modification with an id that already exist reset the timer instead of adding a new one to the stack</param>
    /// <param name="sprite">The sprite used to display the time modification above the player UI</param>
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

    /// <summary>
    /// Add an elemental effect to the StatSystem. Elemental Effect does not stack, adding the same type (the Equals
    /// return true) will instead replace the old one with the new one.
    /// </summary>
    /// <param name="effect"></param>
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

    /// <summary>
    /// Change the hp by the given amount : negative amount damage, positive amount heal. The function will
    /// take care of clamping the value in the range [0...Maxhp]
    /// </summary>
    /// <param name="amount"></param>
    public void Changehp(int amount)
    {
        Currenthp = Mathf.Clamp(Currenthp + amount, 0, stats.hp);
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
            float percentage = Currenthp / (float)previoushp;
            Currenthp = Mathf.RoundToInt(percentage * stats.hp);
        }
    }

    /// <summary>
    /// Will damage (change negatively hp) of the amount of damage stored in the attackData. If the damage are
    /// negative, this heal instead.
    ///
    /// This will also notify the DamageUI so a damage number is displayed.
    /// </summary>
    /// <param name="attackData"></param>
    /*
    public void Damage(BulletData.AttackData attackData)
    {
        int totalDamage = attackData.GetFullDamage();

        Changehp(-totalDamage);
        DamageUI.Instance.NewDamage(totalDamage, m_Owner.transform.position);
    }
    */
}