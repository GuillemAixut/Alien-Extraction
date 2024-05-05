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

public class SpitterBaseScript : YmirComponent
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

    //TO REMOVE WHEN ENEMY.CS IS FIXED?
    protected PathFinding agent;
    public GameObject player = null;
    public Health healthScript;
    public float movementSpeed;
    public float knockBackTimer;
    public float knockBackSpeed;
    public float timePassed = 0f;
    public float life = 100f;
    public float armor = 0;
    public int rarity = 0;
    public float wanderRange = 10f;
    public float detectionRadius = 60f;

    //TO REMOVE WHEN ENEMY.CS IS FIXED?

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

        //Drop items
        //keys = "Nombre:,Probabilidad:";
        //path = "Assets/Loot Tables/spitter_loot.csv";
        //numFields = 2;

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
                //Rather look at player? May cause to do weird rotations
                //LookAt(agent.GetDestination());
                LookAt(player.transform.globalPosition);

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
                LookAt(player.transform.globalPosition);

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
                LookAt(player.transform.globalPosition);

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
                   // DropItem();
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


    public void IsReached(Vector3 position, Vector3 destintion)
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
                Vector3 pos = gameObject.transform.globalPosition;
                pos.y += 15;
                InternalCalls.CreateSpitterAcidSpit(pos, gameObject.transform.globalRotation);
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
                Vector3 pos = gameObject.transform.globalPosition;
                pos.y += 15;
                pos.z -= 10;
                InternalCalls.CreateSpitterAcidRebound(pos, gameObject.transform.globalRotation);
                LookAt(player.transform.globalPosition);
            }
        }

    }
    //REMOVE WHEN ENEMY.CS IS FIXED?
    void TakeDmg(float dmg)
    {
        life -= dmg * armor;
    }
    void LookAt(Vector3 pointToLook)
    {

        Vector3 direction = pointToLook - gameObject.transform.globalPosition;
        direction = direction.normalized;
        float angle = (float)Math.Atan2(direction.x, direction.z);

        //Debug.Log("Desired angle: " + (angle * Mathf.Rad2Deg).ToString());

        if (Math.Abs(angle * Mathf.Rad2Deg) < 1.0f)
            return;

        Quaternion dir = Quaternion.RotateAroundAxis(Vector3.up, angle);

        float rotationSpeed = Time.deltaTime * agent.angularSpeed;


        Quaternion desiredRotation = Quaternion.Slerp(gameObject.transform.localRotation, dir, rotationSpeed);

        gameObject.SetRotation(desiredRotation);
    }

    public void KnockBack(float speed)
    {

        Vector3 knockbackDirection = player.transform.globalPosition - gameObject.transform.globalPosition;
        knockbackDirection = knockbackDirection.normalized;
        knockbackDirection.y = 0f;
        gameObject.SetVelocity(knockbackDirection * -speed);

    }

    public void MoveToCalculatedPos(float speed)
    {
        Vector3 pos = gameObject.transform.globalPosition;
        Vector3 destination = agent.GetDestination();
        Vector3 direction = destination - pos;

        gameObject.SetVelocity(direction.normalized * speed * Time.deltaTime);
    }

    public bool CheckDistance(Vector3 first, Vector3 second, float checkRadius)
    {
        float deltaX = Math.Abs(first.x - second.x);
        float deltaY = Math.Abs(first.y - second.y);
        float deltaZ = Math.Abs(first.z - second.z);

        return deltaX <= checkRadius && deltaY <= checkRadius && deltaZ <= checkRadius;
    }
    public void DestroyEnemy()
    {
        Audio.PlayAudio(gameObject, "FH_Death");
        InternalCalls.Destroy(gameObject);
    }
    //REMOVE WHEN ENEMY.CS IS FIXED?

}
