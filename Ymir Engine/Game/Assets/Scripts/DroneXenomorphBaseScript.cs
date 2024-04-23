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
	DEAD
}

public class DroneXenomorphBaseScript : YmirComponent
{
    public GameObject thisReference = null;

    private GameObject player;

    protected PathFinding agent;
    protected Vector3 targetPosition = null;

    private Health healthScript;

    private DroneState droneState;

	//If aggressive or not
	private bool aggro;

	//Attacks variables

	private float detectionRadius;
	private float wanderRange;

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

		//AGENT
		aggro = false;
        agent.stoppingDistance = 2f;
        agent.speed = 25f;

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
		timeLimit = 5f;

        //Out of range timer
        outOfRangeTimer = 0f;
	}

    public void Update()
	{
		switch (droneState)
		{
			case DroneState.IDLE_NO_AGGRO:
                //Stay idle and call a function to switch to moving from time to time, if detects player, go to cry state

				timeCounter += Time.deltaTime;

				//Do wander if enough time has passed
				if (timeCounter >= timeLimit)
				{
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
                }

                break;
			case DroneState.IDLE_AGGRO:
                //When attacking, stay idle when everything is on cooldown and sometimes switch to move aggro

                if (!CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, clawRange))
                {
                    droneState = DroneState.MOVE_AGGRO;
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
                    droneState = DroneState.CRY;
				}

                break;
			case DroneState.MOVE_AGGRO:
                //Move either to player or to a destination, perform attack when possible

                LookAt(agent.GetDestination());

                agent.CalculatePath(gameObject.transform.globalPosition, player.transform.globalPosition);

                MoveToCalculatedPos(agent.speed);

                break;
			case DroneState.CRY :
				//Do cry animation while not moving, then go to move aggro state

				timeCounter += Time.deltaTime;

                //If done with animation, go to move aggro
                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    droneState = DroneState.MOVE_AGGRO;
                }

				break;
			case DroneState.CLAW :
                //Do claw attack, then go to idle aggro state

                timeCounter += Time.deltaTime;

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);

                //If done with animation, go to idle aggro
                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    droneState = DroneState.IDLE_AGGRO;
                }

                break;
			case DroneState.TAIL :
                //Do tail attack, then go to idle aggro state

                timeCounter += Time.deltaTime;

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);

                //If done with animation, go to idle aggro
                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    droneState = DroneState.IDLE_AGGRO;
                }

                break;
			case DroneState.DEAD :
				//Do death animation and then destroy this game object

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
                droneState = DroneState.IDLE_NO_AGGRO;
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
    public bool CheckDistance(Vector3 first, Vector3 second, float checkRadius)
    {
        float deltaX = Math.Abs(first.x - second.x);
        float deltaY = Math.Abs(first.y - second.y);
        float deltaZ = Math.Abs(first.z - second.z);

        return deltaX <= checkRadius && deltaY <= checkRadius && deltaZ <= checkRadius;
    }
    public void LookAt(Vector3 pointToLook)
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

    public void MoveToCalculatedPos(float speed)
    {
        Vector3 pos = gameObject.transform.globalPosition;
        Vector3 destination = agent.GetDestination();
        Vector3 direction = destination - pos;

        gameObject.SetVelocity(direction.normalized * speed);
    }
    public void DestroyEnemy()
    {
        Audio.PlayAudio(gameObject, "FH_Death");
        InternalCalls.Destroy(gameObject);
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
            droneState = DroneState.IDLE_NO_AGGRO;
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
            clawCooldownTime = 0f;
            timeCounter = 0f;
            //ANIMATION DURATION HERE!!!
            timeLimit = 1f;
            droneState = DroneState.CLAW;
        }
        else if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, tailRange) && tailCooldownTime >= tailCooldown)
        {
            tailCooldownTime = 0f;
            timeCounter = 0f;
            //ANIMATION DURATION HERE!!!
            timeLimit = 0.8f;
            droneState = DroneState.TAIL;
        }

    }

    //State getter
    public DroneState GetState()
    {
        return droneState;
    }


}