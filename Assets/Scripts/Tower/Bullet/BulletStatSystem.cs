using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletStatSystem
{
    [System.Serializable]
    public class Stats
    {
        public float speed;

        public int damage;
        public float splashRange;
        public float attackRange;
        

        public void Copy(Stats other)
        {
            damage = other.damage;
            speed = other.speed;
            splashRange = other.splashRange;
            attackRange = other.attackRange;
        }

        //public void Modify(StatModifier modifier)
        //{
        //    if (modifier.ModifierMode == StatModifier.Mode.Percentage)
        //    {
        //        damage += damage * (modifier.Stats.damage / 100);
        //        attackDelay += Mathf.Floor(attackDelay * (modifier.Stats.attackDelay / 100.0f));
        //        attackRate += Mathf.Floor(attackRate * (modifier.Stats.attackRate / 100.0f));
        //        recogRange += Mathf.Floor(recogRange * (modifier.Stats.recogRange / 100.0f));
        //        attackRange += Mathf.Floor(attackRange * (modifier.Stats.attackRange / 100.0f));
        //        splashRange += Mathf.Floor(splashRange * (modifier.Stats.splashRange / 100.0f));
        //    }
        //    else
        //    {
        //        damage += modifier.Stats.damage;
        //        attackDelay += modifier.Stats.attackDelay;
        //        attackRate += modifier.Stats.attackRate;
        //        recogRange += modifier.Stats.recogRange;
        //        attackRange += modifier.Stats.attackRange;
        //        splashRange += modifier.Stats.splashRange;
        //    }
        //}
    }

    //[System.Serializable]
    //public class StatModifier
    //{
    //    public enum Mode
    //    {
    //        Percentage,
    //        Absolute
    //    }

    //    public Mode ModifierMode = Mode.Absolute;
    //    public Stats Stats = new Stats();
    //}

    public Stats baseStats;
    public Stats stats { get; set; } = new Stats();

    BulletData m_Owner;

    // List<StatModifier> m_ModifiersStack = new List<StatModifier>();

    public void Init(BulletData owner)
    {
        stats.Copy(baseStats);
        m_Owner = owner;
    }

    public void SetBaseStat(TowerStatSystem t_stat)
    {
        baseStats.damage = t_stat.stats.damage;
        baseStats.splashRange = t_stat.stats.splashRange;
        baseStats.attackRange = t_stat.stats.attackRange;
    }
    //public void AddModifier(StatModifier modifier)
    //{
    //    m_ModifiersStack.Add(modifier);
    //    UpdateFinalStats();
    //}

    //public void RemoveModifier(StatModifier modifier)
    //{
    //    m_ModifiersStack.Remove(modifier);
    //    UpdateFinalStats();
    //}

    //void UpdateFinalStats()
    //{
    //    bool maxdamageChange = false;
    //    int previousdamage = stats.damage;

    //    stats.Copy(baseStats);

    //    foreach (var modifier in m_ModifiersStack)
    //    {
    //        if (modifier.Stats.damage != 0)
    //            maxdamageChange = true;

    //        stats.Modify(modifier);
    //    }

    //    foreach (var timedModifier in m_TimedModifierStack)
    //    {
    //        if (timedModifier.Modifier.Stats.damage != 0)
    //            maxdamageChange = true;

    //        stats.Modify(timedModifier.Modifier);
    //    }
    //     }

}
