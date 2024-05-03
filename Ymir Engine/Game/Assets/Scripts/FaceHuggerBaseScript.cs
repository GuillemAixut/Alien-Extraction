using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using YmirEngine;



public class FaceHuggerBaseScript : Enemy
{
    public GameObject thisReference = null;

    public GameObject canvas;


    protected Vector3 targetPosition = null;

    public bool PlayerDetected = false;

    
    private float AttackDistance = 15f;

    //private EnemyState state = EnemyState.Idle;
   
    
    private float wanderTimer;
    public float wanderDuration = 5f;

    private float stopedTimer;
    public float stopedDuration = 1f;

    private float cumTimer;
    public float cumDuration = 2f;

    private float cumTimer2;
    public float cumDuration2 = 5f;

    public float attackTimer;
    private float attackDuration = 0.8f;
    public bool attackSensor = false;

    

    //Audio
    private float CryTimer = 10f;

    public void Start()
    {
        wanderState = WanderState.REACHED;
        wanderDuration = 5f;
        wanderTimer = wanderDuration;
        player = InternalCalls.GetGameObjectByName("Player");
        healthScript = player.GetComponent<Health>();
        agent = gameObject.GetComponent<PathFinding>();
        movementSpeed = 25f;
        knockBackSpeed = 200;
        knockBackTimer = 0.2f;
        stopedDuration = 1f;
        detectionRadius = 80f;
        wanderRange = 100f;
        cumDuration = 2f;
        cumDuration2 = 5f;

        //Drop items
        //keys = "Nombre:,Probabilidad:";
        //path = "Assets/Loot Tables/facehugger_loot.csv";
        //numFields = 2;

        attackTimer = attackDuration;


        cumTimer = cumDuration2;

        //Enemy rarity stats
        if (rarity == 1)
        {
            life = 350;
            armor = 0.1f;
            movementSpeed = 21.5f;
        }
        else if (rarity == 2)
        {
            life = 600;
            armor = 0.2f;
            movementSpeed = 23f;

        }

        agent.stoppingDistance = 2f;
        agent.speed = 1500f;
        agent.angularSpeed = 10f;


        // Animations

        Animation.SetLoop(gameObject, "Idle_Facehugger");
        Animation.SetLoop(gameObject, "IdleCombat_Facehugger");
        Animation.SetLoop(gameObject, "Walk_Facehugger");

        Animation.SetResetToZero(gameObject, "Death_Facehugger", false);

        Animation.AddBlendOption(gameObject, "", "Idle_Facehugger", 10f);
        Animation.AddBlendOption(gameObject, "", "IdleCombat_Facehugger", 10f);
        Animation.AddBlendOption(gameObject, "", "IdleCombat_Facehugger", 10f);
        Animation.AddBlendOption(gameObject, "", "Walk_Facehugger", 10f);
        Animation.AddBlendOption(gameObject, "", "TailAttack_Facehugger", 10f);
        Animation.AddBlendOption(gameObject, "", "Death_Facehugger", 10f);

        Animation.PlayAnimation(gameObject, "Idle_Facehugger");
    }

    public void Update()
    {
        Debug.Log("[ERROR] CurrentaState: " + wanderState);
       
        if(wanderState != WanderState.DEATH) { isDeath(); }
        
        CryTimer += Time.deltaTime;
        cumTimer2 -= Time.deltaTime;
        if (cumTimer2 <= 0)
        {
            switch (wanderState)
            {
                case WanderState.DEATH:

                    timePassed += Time.deltaTime;

                    if (timePassed >= 1.2f)
                    {
                        Debug.Log("[ERROR] DEATH");
                        //DropItem();
                        InternalCalls.Destroy(gameObject);
                    }

                    return;
                case WanderState.REACHED:
                    agent.CalculateRandomPath(gameObject.transform.globalPosition, wanderRange);
                    wanderTimer = wanderDuration;
                    //Debug.Log("[ERROR] Current State: REACHED");
                    targetPosition = agent.GetPointAt(agent.GetPathSize() - 1);

                    Audio.PlayAudio(gameObject, "FH_Move");
                    wanderState = WanderState.GOING;
                    break;

                case WanderState.GOING:
                    LookAt(agent.GetDestination());
                    
                    MoveToCalculatedPos(agent.speed);
                    //Debug.Log("[ERROR] Current State: GOING");

                    IsReached(gameObject.transform.globalPosition, targetPosition);
                    break;


                case WanderState.CHASING:

                    LookAt(agent.GetDestination());
                    //Debug.Log("[ERROR] Current State: CHASING");
                    agent.CalculatePath(gameObject.transform.globalPosition, player.transform.globalPosition);

                    MoveToCalculatedPos(agent.speed);

                   
                    break;

                case WanderState.STOPED:
                    //Debug.Log("[ERROR] Current State: STOPED");
                    ProcessStopped();
                    break;

                case WanderState.HIT:


                    Proccescumdown();

                    break;

                case WanderState.KNOCKBACK:

                    KnockBack(knockBackSpeed);
                    timePassed += Time.deltaTime;

                    if(timePassed >= knockBackTimer)
                    {
                        Debug.Log("[ERROR] End KnockBack"); 
                        wanderState = WanderState.REACHED;
                        timePassed = 0f;
                    }
                    break;

                case WanderState.ATTACK:
                    LookAt(player.transform.globalPosition);
                    Attack();
                    break;


                   
            }

            ////Check if player is alive before chasing
            if (wanderState != WanderState.ATTACK && healthScript.GetCurrentHealth() > 0)
            {

                if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, detectionRadius))
                {

                    if (wanderState != WanderState.KNOCKBACK && wanderState != WanderState.HIT)
                    {
                        
                        if (CryTimer >= 10) 
                        {
                            Audio.PlayAudio(gameObject, "FH_Cry");
                            CryTimer = 0;
                        }
                        wanderState = WanderState.CHASING;
                        
                    }
                    //Attack if in range
                    if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, AttackDistance))
                    {

                        if (wanderState == WanderState.CHASING && wanderState != WanderState.ATTACK && wanderState != WanderState.KNOCKBACK)
                        {
                            //Debug.Log("[ERROR] ATTACKING");
                            attackTimer = attackDuration;
                            gameObject.SetVelocity(gameObject.transform.GetForward() * 0);
                            Audio.PlayAudio(gameObject, "FH_Tail");
                            wanderState = WanderState.ATTACK;
                        }
                    }

                }
            }


        }



    }



    private void Proccescumdown()
    {
        if (cumTimer > 0)
        {
            cumTimer -= Time.deltaTime;
            if (cumTimer <= 0)
            {
                //Debug.Log("[ERROR] Reached");
                wanderState = WanderState.REACHED;
            }
        }
    }   

    private void ProcessStopped()
    {
        if (stopedTimer > 0)
        {
            stopedTimer -= Time.deltaTime;
            if (stopedTimer <= 0)
            {
                wanderState = WanderState.REACHED;
            }
        }
    }

    public WanderState GetState()
    {
        return wanderState;
    }
    private void Attack()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0)
            {
                ////IF HIT, Do damage
                //healthScript.TakeDmg(3);
                //Debug.Log("[ERROR] DID DAMAGE");
                attackSensor = true;
                attackTimer = attackDuration;
                
                stopedTimer = stopedDuration;
                wanderState = WanderState.STOPED;

            }
        }
    }


    private void isDeath()
    {
        if(life <= 0)
        {
            Debug.Log("[ERROR] DEATH");
            gameObject.SetVelocity(new Vector3(0, 0, 0));
            Audio.PlayAudio(gameObject, "FH_Death");
            wanderState = WanderState.DEATH;
            timePassed = 0;
        }
    }


    public void OnCollisionStay(GameObject other)
    {
       if(other.Tag == "Tail" && wanderState != WanderState.KNOCKBACK && wanderState != WanderState.DEATH)
        {
            Debug.Log("[ERROR] HIT!!");
            life -= 80;
           
            wanderState = WanderState.KNOCKBACK;
        }
    }


    public void OnCollisionExit(GameObject other)
    {
        if (other.Tag == "Tail" )
        {
             
        }
    }
}