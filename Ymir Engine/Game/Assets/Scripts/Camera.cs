using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using YmirEngine;

public struct Cinematic
{
    public List<TravelKey> keys;
    public bool isPlaying;
}
public struct TravelKey
{
    public Vector3 position;
    public Vector3 rotation;

    public float travelTime;
}
public class Camera : YmirComponent
{

    enum FOLLOW : int
    {
        NULL = -1,

        PLAYER,
        OTHER
    }
    //--------------------- Controller var ---------------------\\

    //position difference between camera and player
    public Vector3 difPos = new Vector3(-58, 111, 63);

    private GameObject target;
    private Player player;

    // Timers
    private float idleTimer;
    private float delayTimer;

    private float idleTimerMax = 2.0f;
    private float delayTimerMax = 5.6f;

    private bool delay;

    public const float constDelay = 0.3f;

    FOLLOW followState = FOLLOW.PLAYER;

    public Cinematic cinematicTest;

    public void Start()
    {
        // Cinematics

        cinematicTest = new Cinematic();

        //cinematicTest.keys.Add(SetKey(new Vector3(0f,0f,0f), new Vector3(0f,0f,0f), 3f));
        //cinematicTest.keys.Add(SetKey(new Vector3(100f,0f,0f), new Vector3(90f,0f,0f), 3f));

        idleTimer = 0.0f;
        delayTimer = 0.0f;

        delay = false;

        target = InternalCalls.GetGameObjectByName("Player");
        if (target != null)
        {
            player = target.GetComponent<Player>();
        }

        Audio.PlayEmbedAudio(gameObject);
    }

    public void Update()
    {
        // Follow Player (If unrelated stuff gets added, write above this comment)

        Vector3 newpos = target.transform.localPosition + difPos;

        float distance = Vector3.Distance(gameObject.transform.localPosition, newpos);

        if (player.currentState == Player.STATE.IDLE)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer > idleTimerMax)
            {

                delay = true;
                followState = FOLLOW.OTHER;
            }
        }
        else
        {
            idleTimer = 0.0f;

        }

        if (delay)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer < delayTimerMax)
            {
                return;
            }
            else
            {
                delay = false;
                delayTimer = 0.0f;
                followState = FOLLOW.PLAYER;
            }
        }

        switch (followState)
        {
            case FOLLOW.PLAYER:

                gameObject.transform.localPosition = (Vector3.Lerp(gameObject.transform.localPosition, newpos, Time.deltaTime * distance * constDelay));

                break;
            case FOLLOW.OTHER:

                gameObject.transform.localPosition = (Vector3.Lerp(gameObject.transform.localPosition, new Vector3(0f, 0f, 0f), Time.deltaTime * delayTimerMax));

                break;
        }
    }

    private TravelKey SetKey(Vector3 position, Vector3 rotation, float time)
    {
        TravelKey key = new TravelKey();

        key.position = position;
        key.rotation = rotation;
        key.travelTime = time;

        return key;
    }
}

