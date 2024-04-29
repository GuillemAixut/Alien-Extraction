using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public enum DroneState
{
	IDLE_NO_AGGRO,
	IDLE_AGGRO,
	MOVE_NO_AGGRO,
	MOVE_AGGRO,
	CRY,
	CLAW,
	TAIL,
    KNOCKBACK,
	DEAD
}

public class DroneXenomorphBaseScript : Enemy
{
    public GameObject thisReference = null;

    protected Vector3 targetPosition = null;

    

    public DroneState droneState;

	//If aggressive or not
	private bool aggro;

	//Attacks variables

	//Claw
	//private float clawDamage;
	private float clawCooldown;
    private float clawCooldownTime;
    private float clawRange;

	//Tail
	//private float tailDamage;
	private float tailCooldown;
    private float tailCooldownTime;
    private float tailRange;

	//FOR GENERAL TIME MANAGEMENT
	public float timeCounter;
	private float timeLimit;

    private float outOfRangeTimer;

	public void Start()
	{
		//MAIN STUFF
		droneState = DroneState.IDLE_NO_AGGRO;
        player = InternalCalls.GetGameObjectByName("Player");
        healthScript = player.GetComponent<Health>();
        agent = gameObject.GetComponent<PathFinding>();


        knockBackSpeed = 200;
        knockBackTimer = 0.5f;

        //AGENT
        aggro = false;
        agent.stoppingDistance = 2f;
        agent.speed = 800f;
        agent.angularSpeed = 10f;
        Debug.Log("AngularSpeed" + agent.angularSpeed);

		//ATTACKS

		detectionRadius = 60f;
        wanderRange = 100f;

        //Claw
        //clawDamage = 150f;
		clawCooldown = 2f;
        clawCooldownTime = 0f;
        clawRange = 20f;

		//Tail
		//tailDamage = 200f;
		tailCooldown = 6f;
        tailCooldownTime = 0f;
        tailRange = 30f;

		//Time
		timeCounter = 0f;
		timeLimit = 1f;

        //Out of range timer
        outOfRangeTimer = 0f;

        life = 300f;


        Animation.SetLoop(gameObject, "Combat_Idle", true);
        Animation.SetLoop(gameObject, "Drone_Walk", true);

        Animation.SetSpeed(gameObject, "Claw_Attack", 2f);
        Animation.SetSpeed(gameObject, "Drone_Tail_Attack", 2f);

        Animation.AddBlendOption(gameObject, "", "Drone_Walk", 5);
        Animation.AddBlendOption(gameObject, "", "Claw_Attack", 5);
        Animation.AddBlendOption(gameObject, "", "Drone_Tail_Attack", 5);
        Animation.AddBlendOption(gameObject, "", "Cry", 5);
        Animation.AddBlendOption(gameObject, "", "Combat_Idle", 10);
        Animation.AddBlendOption(gameObject, "", "Death", 5);

        Animation.SetResetToZero(gameObject, "Death", false);

        Animation.PlayAnimation(gameObject, "Combat_Idle");

    }

    public void Update()
	{
        Debug.Log("[ERROR] CURRENTSTATE: " + droneState);
        if (droneState != DroneState.DEAD) { isDeath(); }
        switch (droneState)
		{

            case DroneState.DEAD:
                timePassed += Time.deltaTime;

                if (timePassed >= 1.4f)
                {
                    //Debug.Log("[ERROR] DEATH");
                    InternalCalls.Destroy(gameObject);
                }


                return;
            case DroneState.KNOCKBACK:

                KnockBack(knockBackSpeed);

                timePassed += Time.deltaTime;

                if (timePassed >= knockBackTimer)
                {
                    //Debug.Log("[ERROR] End KnockBack");
                    droneState = DroneState.IDLE_NO_AGGRO;
                    Animation.PlayAnimation(gameObject, "Combat_Idle");
                    timePassed = 0f;
                }
                return;
            case DroneState.IDLE_NO_AGGRO:
                //Stay idle and call a function to switch to moving from time to time, if detects player, go to cry state

				timeCounter += Time.deltaTime;

				//Do wander if enough time has passed
				if (timeCounter >= timeLimit)
				{
                    Animation.PlayAnimation(gameObject, "Drone_Walk");
                    timeCounter = 0f;
                    agent.CalculateRandomPath(gameObject.transform.globalPosition, wanderRange);
                    targetPosition = agent.GetPointAt(agent.GetPathSize() - 1);
                    droneState = DroneState.MOVE_NO_AGGRO;
				}

				//Check if player in radius and if so go to cry state
                if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, detectionRadius))
                {
					timeLimit = 0.5f;
                    aggro = true;
                    droneState = DroneState.CRY;
                    Animation.PlayAnimation(gameObject, "Cry");
                }

                break;
			case DroneState.IDLE_AGGRO:
                //When attacking, stay idle when everything is on cooldown and sometimes switch to move aggro
                agent.CalculatePath(gameObject.transform.globalPosition,player.transform.globalPosition);
                LookAt(agent.GetDestination());

                if (!CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, clawRange))
                {
                    droneState = DroneState.MOVE_AGGRO;
                    Animation.PlayAnimation(gameObject, "Drone_Walk");

                }

                break;
			case DroneState.MOVE_NO_AGGRO :
                //Move to destination, if detects player, go to cry state

                LookAt(agent.GetDestination());

                MoveToCalculatedPos(agent.speed);

                IsReached(gameObject.transform.globalPosition, targetPosition);

                //Check if player in radius and if so go to cry state
                if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, detectionRadius))
				{
                    timeLimit = 0.5f;
                    aggro = true;
                    Audio.PlayAudio(gameObject, "DX_Cry");
                    Animation.PlayAnimation(gameObject, "Cry");
                    droneState = DroneState.CRY;
				}

                break;
			case DroneState.MOVE_AGGRO:
                //Move either to player or to a destination, perform attack when possible

                agent.CalculatePath(gameObject.transform.globalPosition, player.transform.globalPosition);
                LookAt(agent.GetDestination());

                MoveToCalculatedPos(agent.speed);

                break;
			case DroneState.CRY :
				//Do cry animation while not moving, then go to move aggro state

				timeCounter += Time.deltaTime;

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);
                //gameObject.ClearForces();

                //If done with animation, go to move aggro
                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    droneState = DroneState.MOVE_AGGRO;
                    Animation.PlayAnimation(gameObject, "Drone_Walk");
                }

				break;
			case DroneState.CLAW :
                //Do claw attack, then go to idle aggro state

                timeCounter += Time.deltaTime;

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);
                //gameObject.ClearForces();

                //LookAt(agent.GetDestination());

                //If done with animation, go to idle aggro
                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    Audio.PlayAudio(gameObject, "DX_Claw");
                    droneState = DroneState.IDLE_AGGRO;
                    Animation.PlayAnimation(gameObject, "Combat_Idle");
                    //To fix claw to tail attack bug
                    tailCooldownTime = 5.5f;
                }

                break;
			case DroneState.TAIL :
                //Do tail attack, then go to idle aggro state

                timeCounter += Time.deltaTime;

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);
                //gameObject.ClearForces();
                

                //If done with animation, go to idle aggro
                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    Audio.PlayAudio(gameObject, "DX_Tail");
                    Animation.PlayAnimation(gameObject, "Combat_Idle");
                    droneState = DroneState.IDLE_AGGRO;
                    //To fix tail to claw attack bug
                    clawCooldownTime = 1f;

                }

                break;




		}

        //Check attack posiblilities and count cooldowns
        CheckAttack();

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
                droneState = DroneState.IDLE_NO_AGGRO;
                Animation.PlayAnimation(gameObject, "Combat_Idle");
            }

        }
        else
        {
            //So that it resets if it is again in range
            outOfRangeTimer = 0f;
        }

    }

    //SHOULD BE ON ENEMY BASE SCRIPT!!!!!
    //LookAt, CheckDistance, MoveToPosition, DestroyEnemy, IsReached
    //Check distance between two gameobjects world position

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Tail" && droneState != DroneState.KNOCKBACK && droneState != DroneState.DEAD)
        {
            //Debug.Log("[ERROR] CURRENTSTATE2: " + droneState);
            //Debug.Log("[ERROR] HIT!!");
            life -= 80;

            droneState = DroneState.KNOCKBACK;
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
            droneState = DroneState.IDLE_NO_AGGRO;
        }
    }

    private void isDeath()
    {
        if (life <= 0)
        {
            Audio.PlayAudio(gameObject, "DX_Death");
            Animation.PlayAnimation(gameObject, "Death");
            //Debug.Log("[ERROR] CURRENTSTATE2: " + droneState);
            gameObject.SetVelocity(new Vector3(0, 0, 0));
            droneState = DroneState.DEAD;
            timePassed = 0;
        }
    }
    //REMOVE functions on top------------------------------------------------------------------------

    //CHECK ATTACKS
    private void CheckAttack()
	{
        clawCooldownTime += Time.deltaTime;
        tailCooldownTime += Time.deltaTime;

        if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, clawRange) && clawCooldownTime >= clawCooldown)
        {
            if (droneState != DroneState.TAIL) 
            {
                clawCooldownTime = 0f;
                timeCounter = 0f;
                //ANIMATION DURATION HERE!!!
                timeLimit = 1f;
                droneState = DroneState.CLAW;
                LookAt(player.transform.globalPosition);
                Animation.PlayAnimation(gameObject, "Claw_Attack");
            }
        }
        else if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, tailRange) && tailCooldownTime >= tailCooldown)
        {
            if (droneState != DroneState.CLAW)
            {
                tailCooldownTime = 0f;
                timeCounter = 0f;
                //ANIMATION DURATION HERE!!!
                timeLimit = 0.8f;
                droneState = DroneState.TAIL;
                LookAt(player.transform.globalPosition);
                Animation.PlayAnimation(gameObject, "Drone_Tail_Attack");
            }
        }

    }

    //State getter
    public DroneState GetState()
    {
        return droneState;
    }


}