using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmirEngine;

public class Spawner: YmirComponent
{
    public bool spawn = false;

    public int maxEnemies;

    //private int enemiesCounter;

    public List<GameObject> currentEnemies = null;

    public void Start()
    {
        spawn = true;
        currentEnemies = new List<GameObject>();
        //enemiesCounter = 0;
    }

    public void Update()
    {

        if(spawn)
        {
            Spawn();
            spawn = false;
        }

    }


    private void Spawn()
    {
        InternalCalls.CreateGOFromPrefab("Assets/Prefabs", "Enemy-FaceHugger", gameObject.transform.globalPosition);

        //if( enemy != null)
        //{
        //    currentEnemies.Add(enemy);
        //    enemiesCounter++;
        //}

    }


}
