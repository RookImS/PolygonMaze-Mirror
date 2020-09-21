using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    private int spawnNo;
    private int reward;
    private int damage;

    public string desc;
    public EnemyStatSystem Stats;
    public EColorSystem EColorSys = new EColorSystem();
    public WaveSystem WaveSys = new WaveSystem();


    public void Init()
    {
        Stats.Init(this);
    }

    void Awake()
    {
        //empty
    }

    // Update is called once per frame
    void Update()
    {
        Stats.Tick();
    }

    public void Death()
    {
        Stats.Death();
    }

    /// <summary>
    /// Attack the given target. NOTE : this WON'T check if the target CAN be attacked, you should make sure before
    /// with the CanAttackTarget function.
    /// </summary>
    /// <param name="target">The CharacterData you want to attack</param>
    public void Attack(PlayerData target)
    {
        //enemy decrease life of player
    }

    /// <summary>
    /// Damage the Character by the AttackData given as parameter. See the documentation for that class for how to
    /// add damage to that attackData. (this will be done automatically by weapons, but you may need to fill it
    /// manually when writing special elemental effect)
    /// </summary>
    /// <param name="attackData"></param>
    /*
    public void Damage(BulletData.AttackData attackData)
    {
        Stats.Damage(attackData);

        OnDamage.Invoke();
    }
    */
}
