using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyEditorInfo
    {
        private int spawNo;
        private int wave;

        public enum Mode
        {
            Circle,
            SemiCircle,
            Sector,
            Ellipse,
            Ring
        }

        private Mode specific;

        public int getSpawNo()
        {
            return this.spawNo;
        }

        public void setSpawNo(int spawNo)
        {
            this.spawNo = spawNo;
        }

        public int getWave()
        {
            return this.wave;
        }

        public void setWave(int wave)
        {
            this.wave = wave;
        }

        public Mode getSpecific()
        {
            return this.specific;
        }

        public void setSpecific(Mode specific)
        {
            this.specific = specific;
        }

    }
    public List<Transform> enemyPrefabs;
    private List<EnemyEditorInfo> enemyEditorInfos = null;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateEnemy()
    {
        if (!enemyEditorInfos.Equals(null))
            foreach (EnemyEditorInfo eInfo in enemyEditorInfos)
            {
               
            }
        else
            Debug.Log("enemyEditorInfos is null");
    }
}
