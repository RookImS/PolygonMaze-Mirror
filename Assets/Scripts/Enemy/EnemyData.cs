using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    private int spawnNo;
    public int reward;
    public int damage;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        Stats.Tick();
    }

    public void Death()
    {
        Stats.Death();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Attack the given target. NOTE : this WON'T check if the target CAN be attacked, you should make sure before
    /// with the CanAttackTarget function.
    /// </summary>
    /// <param name="target">The CharacterData you want to attack</param>
    public void Attack()
    {
        //PlayerControl.Instance.Damage(this.damage);
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
