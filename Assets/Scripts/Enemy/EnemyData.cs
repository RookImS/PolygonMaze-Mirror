using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public HpBar hpBar;

    private int spawnNo;
    private int waveNum;

    public int reward;
    public int damage;

    public EnemyStatSystem Stats;
    public EColorSystem EColorSys = new EColorSystem();
    public WaveSystem WaveSys = new WaveSystem();

    private void Update()
    {
        Stats.Tick();
    }

    public void Init()
    {
        Stats.Init(this);
        hpBar.Init();
    }
    

    public int GetWaveNum()
    {
        return this.waveNum;
    }

    public void SetWaveNum(int waveNum)
    {
        this.waveNum = waveNum;
    }

    public void ApplyWaveSystem()
    {
        WaveSys.ApplyWaveSystem(this);
    }

    public void Death()
    {
        Stats.Death();
        PlayerControl.Instance.GainCost(this.reward);

        hpBar.DestroyHpBar();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Attack the given target. NOTE : this WON'T check if the target CAN be attacked, you should make sure before
    /// with the CanAttackTarget function.
    /// </summary>
    /// <param name="target">The CharacterData you want to attack</param>
    public void Attack()
    {
        PlayerControl.Instance.Damage(this.damage);
        hpBar.DestroyHpBar();
    }

    /// <summary>
    /// Damage the Character by the AttackData given as parameter. See the documentation for that class for how to
    /// add damage to that attackData. (this will be done automatically by weapons, but you may need to fill it
    /// manually when writing special elemental effect)
    /// </summary>
    /// <param name="attackData"></param>
 
    public void Damage(int damage)
    {
        Stats.Damage(damage);
    }
}
