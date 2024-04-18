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
	DEATH
}

public class DroneXenomorphBaseScript : YmirComponent
{
    public GameObject thisReference = null;

    private GameObject player;

    protected PathFinding agent;

    private Health healthScript;

    private DroneState droneState;

	//If aggressive or not
	//private bool aggro;

	//Attacks variables

	//Claw
	//private float clawDamage;
	//private float clawDuration;
	//private float clawTimer;
	//private float clawCooldown;

	//Tail
	//private float tailDamage;
	//private float tailDuration;
	//private float tailTimer;
	//private float tailCooldown;

	//FOR GENERAL TIME MANAGEMENT
	//private float timeCounter;
	//private float timeLimit;

	public void Start()
	{
		//MAIN STUFF
		droneState = DroneState.IDLE_NO_AGGRO;
        player = InternalCalls.GetGameObjectByName("Player");
        healthScript = player.GetComponent<Health>();
        agent = gameObject.GetComponent<PathFinding>();

		//AGENT
		//aggro = false;
        agent.stoppingDistance = 2f;
        agent.speed = 15f;

		//ATTACKS
		////Claw
		//clawDamage = 150f;
		//clawDuration = 1f;
		//clawTimer = 0.0f;
		//clawCooldown = 2f;

		////Tail
		//tailDamage = 200f;
		//tailDuration = 0.8f;
		//tailTimer = 0.0f;
		//tailCooldown = 6f;

		////Time
		//timeCounter = 0.0f;
		////CHANGE THIS LIMIT AND REMOVE THE COMMENT WHEN NEEDED!
		//timeLimit = 0.0f;
    }

    public void Update()
	{
		switch (droneState)
		{
			case DroneState.IDLE_NO_AGGRO:
				//Stay idle and call a function to switch to moving from time to time, if detects player, go to cry state

				break;
			case DroneState.IDLE_AGGRO: 
				//When attacking, stay idle when everything is on cooldown and sometimes switch to move aggro

				break;
			case DroneState.MOVE_NO_AGGRO : 
				//Move to destination, if detects player, go to cry state

				break;
			case DroneState.MOVE_AGGRO: 
				//Move either to player or to a destination, perform attack when possible

				break;
			case DroneState.CRY :
				//Do cry animation while not moving, then go to move aggro state

				break;
			case DroneState.CLAW : 
				//Do claw attack, then go to idle aggro state

				break;
			case DroneState.TAIL : 
				//Do tail attack, then go to idle aggro state

				break;
			case DroneState.DEATH :
				//Do death animation and then destroy this game object

				break;
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

	//CHECK ATTACKS
	private void CheckAttack()
	{
		//Do checks
	}

    //State getter
    public DroneState GetState()
    {
        return droneState;
    }


}