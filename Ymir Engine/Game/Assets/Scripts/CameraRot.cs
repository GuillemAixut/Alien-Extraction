using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using YmirEngine;

public class CameraRot : YmirComponent
{
    //--------------------- Controller var ---------------------\\

    //position difference between camera and player
    public Vector3 difPos = new Vector3(-58, 111, 63);
    //public Vector3 difPos = new Vector3(-120, 0, 0);

    //camera velocity
    private float followStrenght;

    private GameObject target;

    //private bool scriptStart = true;

    public void Start()
    {
        target = InternalCalls.GetGameObjectByName("Player");
        if (target != null)
        {
            followStrenght = target.GetComponent<Player>().movementSpeed;
        }

        

        Audio.PlayAudio(gameObject, "LV2_Inter");
    }

    public void Update()
    {
        //gameObject.transform.localRotation = Quaternion.Euler(120, 25, -142);

        Vector3 newpos = target.transform.localPosition + difPos;

        float dis = Vector3.Distance(gameObject.transform.localPosition, newpos);

        gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, newpos, Time.deltaTime * followStrenght * dis);
    }
}

