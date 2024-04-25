using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmirEngine;

public class SpawnSensor:YmirComponent
{
    public GameObject spawner;
    private Spawner spawnScript;

    private bool spawned;

    public void Start()
    {
        spawned = false;
        spawnScript = spawner.GetComponent<Spawner>();
    }


    public void Update()
    {
        if (Input.GetKey(YmirKeyCode.X) == KeyState.KEY_DOWN)
        {
            spawnScript.spawn = true;
        }
    }


    public void OnCollisionStay(GameObject other)
    {
        Debug.Log("[ERROR] COLLISION");
        if (other.Tag == "Player" && !spawned)
        {
            Debug.Log("[ERROR] COLLISION GOOD");
            spawnScript.spawn = true;
            spawned = true;
        }

    }

}

