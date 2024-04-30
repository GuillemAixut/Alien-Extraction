using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

enum XenoState
{
    IDLE,
    IDLE_AGGRO,
    MOVE,
    MOVE_AGGRO,
    MOVE_BACKWARDS,
    CRY,
    ACID_SPIT,
    ACID_REBOUND,
    KNOCKBACK,
    DEAD
}

public class Spitter : Enemy
{
    public GameObject thisReference = null;

    protected Vector3 targetPosition = null;

    private XenoState xenoState;

    //Attacks variables

    //If aggressive or not
    private bool aggro;
    //private float backwardsCooldownTime;

    //Acid spit
    //private float clawDamage;
    private float acidSpitCooldown;
    private float acidSpitCooldownTime;
    private float acidSpitRange;

    private float tooCloseRange;

    //Acid rebound
    //private float tailDamage;
    private float acidSpitReboundCooldown;
    private float acidSpitReboundCooldownTime;

    //FOR GENERAL TIME MANAGEMENT
    public float timeCounter;
    private float timeLimit;

    private float outOfRangeTimer;

    public void Start()
    {
        //Base stuff
        xenoState = XenoState.IDLE;
        player = InternalCalls.GetGameObjectByName("Player");
        healthScript = player.GetComponent<Health>();
        agent = gameObject.GetComponent<PathFinding>();

        //Agent
        agent.stoppingDistance = 2f;
        agent.speed = 800f;
        agent.angularSpeed = 10f;

        //ATTACKS

        aggro = false;
        detectionRadius = 120f;
        wanderRange = 100f;
        //backwardsCooldownTime = 0f;

        //Acid spit
        acidSpitCooldown = 4f;
        acidSpitCooldownTime = 0f;
        acidSpitRange = 100f;

        tooCloseRange = 60f;

        //Tail
        acidSpitReboundCooldown = 10f;
        acidSpitReboundCooldownTime = 0f;

        //Time
        timeCounter = 0f;
        timeLimit = 1f;

        //Out of range timer
        outOfRangeTimer = 0f;

        //Life
        life = 450f;

    }

    public void Update()
    {
        //backwardsCooldownTime += Time.deltaTime;

        if (xenoState != XenoState.DEAD) { isDeath(); }

        switch (xenoState)
        {
            case XenoState.IDLE:

                timeCounter += Time.deltaTime;

                //Do wander if enough time has passed
                if (timeCounter >= timeLimit)
                {
                    //Animation.PlayAnimation(gameObject, "Drone_Walk");
                    timeCounter = 0f;
                    agent.CalculateRandomPath(gameObject.transform.globalPosition, wanderRange);
                    targetPosition = agent.GetPointAt(agent.GetPathSize() - 1);
                    xenoState = XenoState.MOVE;
                }

                //Check if player in radius and if so go to cry state
                if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, detectionRadius))
                {
                    timeLimit = 0.5f;
                    aggro = true;
                    xenoState = XenoState.CRY;
                }

                break;
            case XenoState.MOVE:

                LookAt(agent.GetDestination());

                MoveToCalculatedPos(agent.speed);

                IsReached(gameObject.transform.globalPosition, targetPosition);

                //Check if player in radius and if so go to cry state
                if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, detectionRadius))
                {
                    timeLimit = 0.5f;
                    aggro = true;
                    xenoState = XenoState.CRY;
                }

                break;
            case XenoState.CRY:

                timeCounter += Time.deltaTime;

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);

                //If done with animation, go to move aggro
                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    xenoState = XenoState.MOVE_AGGRO;
                }

                break;
            case XenoState.IDLE_AGGRO:

                agent.CalculatePath(gameObject.transform.globalPosition, player.transform.globalPosition);
                LookAt(agent.GetDestination());

                if (!CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, acidSpitRange))
                {
                    xenoState = XenoState.MOVE_AGGRO;
                }

                break;
            case XenoState.MOVE_AGGRO:

                agent.CalculatePath(gameObject.transform.globalPosition, player.transform.globalPosition);
                LookAt(agent.GetDestination());

                MoveToCalculatedPos(agent.speed);

                break;
            case XenoState.MOVE_BACKWARDS:

                timeCounter += Time.deltaTime;

                agent.CalculatePath(gameObject.transform.globalPosition, player.transform.globalPosition);
                LookAt(agent.GetDestination());

                MoveToCalculatedPos(-agent.speed);

                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    timeLimit = 1f;
                    xenoState = XenoState.IDLE_AGGRO;
                }

                break;
            case XenoState.KNOCKBACK:

                KnockBack(knockBackSpeed);

                timePassed += Time.deltaTime;

                if (timePassed >= knockBackTimer)
                {
                    //Debug.Log("[ERROR] End KnockBack");
                    xenoState = XenoState.IDLE;
                    timePassed = 0f;
                }

                return;
            case XenoState.ACID_SPIT:

                timeCounter += Time.deltaTime;

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);

                //If done with animation, go to idle aggro
                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    xenoState = XenoState.IDLE_AGGRO;
                    //To fix tail to claw attack bug
                    //acidSpitReboundCooldownTime = 1f;

                }

                break;
            case XenoState.ACID_REBOUND:
                timeCounter += Time.deltaTime;

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);

                //If done with animation, go to idle aggro
                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    xenoState = XenoState.IDLE_AGGRO;
                    //To fix tail to claw attack bug
                    //acidSpitReboundCooldownTime = 1f;

                }

                break;
            case XenoState.DEAD:

                timePassed += Time.deltaTime;

                if (timePassed >= 1.4f)
                {
                    InternalCalls.Destroy(gameObject);
                }

                break;
        }

        //Check attacks
        CheckAttacks();

        //Walk backwards
        if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, tooCloseRange) && aggro == true)
        {
            timeCounter = 0f;
            timeLimit = 0.8f;
            xenoState = XenoState.MOVE_BACKWARDS;
        }

        //If player too far away, go back to wander
        if (!CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, detectionRadius) && aggro == true)
        {
            outOfRangeTimer += Time.deltaTime;

            if (outOfRangeTimer >= 3f)
            {
                outOfRangeTimer = 0f;
                timeCounter = 0f;
                timeLimit = 5f;
                aggro = false;
                gameObject.SetVelocity(new Vector3(0, 0, 0));
                xenoState = XenoState.IDLE;
            }
        }
        else
        {
            //So that it resets if it is again in range
            outOfRangeTimer = 0f;
        }

    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Tail" && xenoState != XenoState.KNOCKBACK && xenoState != XenoState.DEAD)
        {
            life -= 80;

            xenoState = XenoState.KNOCKBACK;
        }
    }


    public new void IsReached(Vector3 position, Vector3 destintion)
    {
        Vector3 roundedPosition = new Vector3(Mathf.Round(position.x),
                                      0,
                                      Mathf.Round(position.z));

        Vector3 roundedDestination = new Vector3(Mathf.Round(destintion.x),
                                                 0,
                                                 Mathf.Round(destintion.z));

        if ((roundedPosition.x == roundedDestination.x) && (roundedPosition.y == roundedDestination.y) && (roundedPosition.z == roundedDestination.z))
        {
            gameObject.SetVelocity(new Vector3(0, 0, 0));
            xenoState = XenoState.IDLE;
        }
    }

    private void isDeath()
    {
        if (life <= 0)
        {
            gameObject.SetVelocity(new Vector3(0, 0, 0));
            xenoState = XenoState.DEAD;
            timePassed = 0;
        }
    }

    private void CheckAttacks()
    {
        acidSpitCooldownTime += Time.deltaTime;
        acidSpitReboundCooldownTime += Time.deltaTime;

        if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, acidSpitRange) && acidSpitCooldownTime >= acidSpitCooldown)
        {
            if (xenoState != XenoState.ACID_REBOUND)
            {
                acidSpitCooldownTime = 0f;
                timeCounter = 0f;
                //ANIMATION DURATION HERE!!!
                timeLimit = 0.8f;
                xenoState = XenoState.ACID_SPIT;
                LookAt(player.transform.globalPosition);
            }
        }
        else if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, acidSpitRange) && acidSpitReboundCooldownTime >= acidSpitReboundCooldown)
        {
            if (xenoState != XenoState.ACID_SPIT)
            {
                acidSpitReboundCooldownTime = 0f;
                timeCounter = 0f;
                //ANIMATION DURATION HERE!!!
                timeLimit = 0.8f;
                xenoState = XenoState.ACID_REBOUND;
                LookAt(player.transform.globalPosition);
            }
        }

    }
}