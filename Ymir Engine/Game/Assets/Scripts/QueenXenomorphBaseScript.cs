using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public enum QueenState
{
	IDLE_PHASE_1,
	IDLE_PHASE_2,
	WALKING_TO_PLAYER,
    WALK_BACKWARDS,
	WALKING_SIDEWAYS,
    DEAD,
    PAUSED,

    //ATTACKS

    ACID_SPIT,
	CLAW,
	AXE_TAIL,
	PREPARE_DASH,
	DASH
}

public class QueenXenomorphBaseScript : YmirComponent
{
    public GameObject thisReference = null;

    private QueenState queenState;

    private QueenState pausedState;

	public float life;

	public float speed = 1500f;

    public float armor = 0;

    public float xSpeed = 0, ySpeed = 0;

	private float queenRotationSpeed;

    public float DetectionRadius = 200f;

    private GameObject player;

    //For attacks
    //Random attack
    private Random random = new Random();
    private bool randomSelected = false;
    private float selectedAttack = 0f;
    private float randomCounter = 0f;

    //Random movement
    private bool randomMovSelected = false;
    private float selectedMovement = 0f;
    private float sidewaysDuration = 0f;
    private float sidewaysTimer = 0f;

    private Vector3 vectorToPlayer = null;

    private int baseAttacks = 0;

    private float clawRadius = 30f;
    private float axeRadius = 50f;
    private float dashRadius = 50f;
    private float acidSpitRadius = 80f;

    private float clawAttackCooldown = 4f;
    private float clawTimer;
    private bool clawReady;
    private bool clawDone = false;
    public float clawAniCounter = 0f;
    private float clawAniDuration = 2.5f;

    private float acidSpitAttackCooldown = 20f;
    private float acidSpitTimer;
    private bool acidSpitReady;
    //private bool acidSpitDone = false;
    public float acidSpitAniCounter = 0f;
    private float acidSpitAniDuration = 2f;

    private float axeAttackCooldown = 18f;
	private float axeTimer;
	private bool axeReady;
    //private bool axeDone = false;
    public float axeAniCounter = 0f;
    private float axeAniDuration = 1.5f;

    private float dashAttackCooldown = 22f;
    private float dashTimer;
	private bool dashReady;
    private float dashAniCounter1 = 0f;
    private float dashAniDuration1 = 1.3f;
    private float dashAniCounter2 = 0f;
    private float dashAniDuration2 = 2f;
    private bool dashDone = false;
    private float dashNum = 0f;

    private float backwardsTimer;
    private float backwardsDuration = 1f;

    private float timePassed = 0f;

    private bool tailColdown = false;



    public void Start()
	{
		//Temporary until we know for sure
		queenState = QueenState.IDLE_PHASE_1;
		
		life = 500f;
		queenRotationSpeed = 5f;
        player = InternalCalls.GetGameObjectByName("Player");
		axeTimer = axeAttackCooldown;
		dashTimer = dashAttackCooldown;
        clawTimer = clawAttackCooldown;
        acidSpitTimer = 5f;
        clawReady = true;
        axeReady = false;
        dashReady = false;
        acidSpitReady = false;

        //Make sure it starts by walking towards the player
        selectedMovement = 1;
        randomMovSelected = true;

        //Animations
        Animation.SetLoop(gameObject, "Boss_Idle", true);
        Animation.SetLoop(gameObject, "Boss_Walk", true);

        Animation.SetResetToZero(gameObject, "Boss_Die", false);

        Animation.AddBlendOption(gameObject, "", "Boss_Idle", 10f);
        Animation.AddBlendOption(gameObject, "", "Boss_Walk", 10f);
        Animation.AddBlendOption(gameObject, "", "Boss_Cry", 10f);
        Animation.AddBlendOption(gameObject, "", "Boss_Dash", 10f);
        Animation.AddBlendOption(gameObject, "", "Boss_Claw_Attack", 10f);
        Animation.AddBlendOption(gameObject, "", "Boss_Die", 10f);

        Animation.PlayAnimation(gameObject, "Boss_Idle");
    }

    public void Update()
	{

        if (queenState != QueenState.DEAD) { isDeath(); }
        //Dont rotate while doing dash
        if (queenState != QueenState.DASH)
        {
            RotateQueen();
        }

        //Each attack has cooldown so its not spammed
        ClawCooldown();

        AcidSpitCooldown();

		//Once 2 attacks done, can use other attacks
		if (baseAttacks >= 2)
		{
			AxeCooldown();
			DashCooldown();
		}

		switch (queenState)
		{
            case QueenState.PAUSED:
                //Do nothing
                break;
            case QueenState.DEAD:

                timePassed += Time.deltaTime;

                if (timePassed >= 1.2f)
                {
                    Debug.Log("[ERROR] DEATH");

                    InternalCalls.Destroy(gameObject);
                }
                return;
			case QueenState.IDLE_PHASE_1:

                if (!randomMovSelected)
                {
                    selectedMovement = random.Next(0, 4);
                    randomMovSelected = true;
                }

                if ((CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, DetectionRadius)) && (!CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, clawRadius)))
                {
                    if (selectedMovement < 2)
                    {
                        Debug.Log("[ERROR] BOSS STATE WALKING TO PLAYER");
                        randomMovSelected = false;
                        Animation.PlayAnimation(gameObject, "Boss_Walk");
                        Audio.PlayAudio(gameObject, "QX_Move");
                        queenState = QueenState.WALKING_TO_PLAYER;
                    }
                    else
                    {
                        Debug.Log("[ERROR] BOSS STATE WALKING SIDEWAYS");
                        randomMovSelected = false;
                        sidewaysDuration = random.Next(1, 4);
                        sidewaysTimer = 0f;
                        Animation.PlayAnimation(gameObject, "Boss_Walk");
                        Audio.PlayAudio(gameObject, "QX_Move");
                        queenState = QueenState.WALKING_SIDEWAYS;
                    }
                }
                else
                {
                    CheckAttackDistance();
                }

                break;
			case QueenState.WALKING_TO_PLAYER:

                vectorToPlayer = player.transform.globalPosition - gameObject.transform.globalPosition;
                vectorToPlayer = Vector3.Normalize(vectorToPlayer);

                xSpeed = vectorToPlayer.x;
                ySpeed = vectorToPlayer.z;

                gameObject.SetVelocity(gameObject.transform.GetForward() * speed * 2 * Time.deltaTime);

                CheckAttackDistance();

                break;
			case QueenState.WALKING_SIDEWAYS:

                if (selectedMovement == 2)
                {
                    //Walk to the right side
                    gameObject.SetVelocity(new Vector3(-(gameObject.transform.GetForward().z * speed * 2 * Time.deltaTime), 0, (gameObject.transform.GetForward().x * speed * 2 * Time.deltaTime)));
                }
                else
                {
                    //Walk to the left side
                    gameObject.SetVelocity(new Vector3((gameObject.transform.GetForward().z * speed * 2 * Time.deltaTime), 0, -(gameObject.transform.GetForward().x * speed * 2 * Time.deltaTime)));
                }

                sidewaysTimer += Time.deltaTime;

                if (sidewaysTimer >= sidewaysDuration)
                {
                    CheckAttackDistance();

                    if (sidewaysTimer >= sidewaysDuration+2)
                    {
                        Debug.Log("[ERROR] BOSS STATE WALKING TO PLAYER");
                        Audio.PlayAudio(gameObject, "QX_Move");
                        queenState = QueenState.WALKING_TO_PLAYER;
                    }
                }

                break;
            case QueenState.WALK_BACKWARDS:

                gameObject.SetVelocity(gameObject.transform.GetForward() * -speed * 2 * Time.deltaTime);

                backwardsTimer += Time.deltaTime;

                if (backwardsTimer >= backwardsDuration)
                {
                    Debug.Log("[ERROR] BOSS STATE ACID SPIT");
                    backwardsTimer = 0f;
                    acidSpitReady = false;
                    acidSpitTimer = acidSpitAttackCooldown;
                    Animation.SetBackward(gameObject, "Boss_Walk", false);
                    Animation.PlayAnimation(gameObject, "Boss_Cry");
                    Audio.PlayAudio(gameObject, "QX_Acid");
                    Vector3 pos = gameObject.transform.globalPosition;
                    pos.y += 10;
                    pos.z -= 6;
                    InternalCalls.CreateQueenSpitAttack(pos, gameObject.transform.globalRotation);
                    queenState = QueenState.ACID_SPIT;
                }

                break;
			case QueenState.CLAW:

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);

                clawAniCounter += Time.deltaTime;

                if (clawAniCounter >= clawAniDuration)
                {
                    Debug.Log("[ERROR] BOSS STATE IDLE");
                    clawAniCounter = 0f;
                    Animation.PlayAnimation(gameObject, "Boss_Idle");
                    queenState = QueenState.IDLE_PHASE_1;
                }
                else if (clawAniCounter >= 1f && clawDone == false)
                {
                    Vector3 pos = gameObject.transform.globalPosition;
                    pos.y += 10;
                    pos.z -= 6;
                    InternalCalls.CreateQueenClawAttack(pos, gameObject.transform.globalRotation);
                    clawDone = true;
                }

                break;
            case QueenState.ACID_SPIT:

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);

                acidSpitAniCounter += Time.deltaTime;

                if (acidSpitAniCounter >= acidSpitAniDuration)
                {
                    Debug.Log("[ERROR] BOSS STATE IDLE");
                    acidSpitAniCounter = 0f;
                    baseAttacks++;
                    Animation.PlayAnimation(gameObject, "Boss_Idle");
                    queenState = QueenState.IDLE_PHASE_1;
                }

                break;
			case QueenState.AXE_TAIL:

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);

                axeAniCounter += Time.deltaTime;

                if (axeAniCounter >= axeAniDuration)
                {
                    Debug.Log("[ERROR] BOSS STATE IDLE");
                    axeAniCounter = 0f;
                    Animation.PlayAnimation(gameObject, "Boss_Idle");
                    Audio.PlayAudio(gameObject, "QX_TailHit");
                    queenState = QueenState.IDLE_PHASE_1;
                }

            break;
			case QueenState.PREPARE_DASH:

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);

                dashAniCounter1 += Time.deltaTime;

                if (dashAniCounter1 >= dashAniDuration1)
                {
                    Debug.Log("[ERROR] BOSS STATE DASH");
                    dashAniCounter1 = 0f;
                    queenState = QueenState.DASH;
                }

            break;
			case QueenState.DASH: 

				if (!dashDone)
                {
                    dashDone = true;
                    dashNum++;
                }
                gameObject.SetVelocity(gameObject.transform.GetForward() * speed * 8 * Time.deltaTime);

                dashAniCounter2 += Time.deltaTime;

                if (dashAniCounter2 >= dashAniDuration2)
                {
                    if (dashNum == 2)
                    {
                        Debug.Log("[ERROR] BOSS STATE IDLE");
                        dashAniCounter2 = 0f;
                        dashDone = false;
                        dashNum = 0;
                        Animation.PlayAnimation(gameObject, "Boss_Idle");
                        queenState = QueenState.IDLE_PHASE_1;

                    }
                    else
                    {
                        Debug.Log("[ERROR] BOSS STATE PREPARE SECOND DASH");
                        dashAniCounter2 = 0f;
                        dashDone = false;
                        Animation.PlayAnimation(gameObject, "Boss_Dash");
                        queenState = QueenState.PREPARE_DASH;
                    }
                }
            break;
		}

        if (queenState != QueenState.PAUSED)
        {
            //If player too far away stay idle
            if (!CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, DetectionRadius))
            {
                Debug.Log("[ERROR] BOSS STATE IDLE");
                Animation.PlayAnimation(gameObject, "Boss_Idle");
                queenState = QueenState.IDLE_PHASE_1;
            }

            if (tailColdown)
            {
                timePassed += Time.deltaTime;
                if (timePassed >= 0.5)
                {
                    tailColdown = false;
                }
            }
        }
    }

    //GENERATE RANDOM NUMBER AMB PICK A ATTACK IF ITS COOLDOWN IS READY AND DISTANCE IS ENOUGH
    private void CheckAttackDistance()
    {
        if (!randomSelected)
        {
            selectedAttack = random.Next(0, 100);
            randomSelected = true;
        }

        randomCounter += Time.deltaTime;

        switch (selectedAttack)
        {
            case float i when (i <= 60):
                if ((CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, clawRadius)) && clawReady == true)
                {
                    Debug.Log("[ERROR] BOSS STATE CLAW");
                    clawReady = false;
                    clawTimer = clawAttackCooldown;
                    baseAttacks++;
                    Animation.PlayAnimation(gameObject, "Boss_Claw_Attack");
                    Audio.PlayAudio(gameObject, "QX_Claw");
                    queenState = QueenState.CLAW;
                    clawDone = false;
                }
                else
                {
                    //Reset counter
                    randomCounter = 0;
                    randomSelected = false;
                }

                break;
            case float i when (i > 60 && i <= 80):
                if ((CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, axeRadius)) && axeReady == true)
                {
                    Debug.Log("[ERROR] BOSS STATE AXE");
                    axeReady = false;
                    axeTimer = axeAttackCooldown;
                    //TAIL ANIMATION HERE!!!!!!!!-----------------------------------------------------------------------------------------------------------------
                    Audio.PlayAudio(gameObject, "QX_TailMove");
                    Vector3 pos = gameObject.transform.globalPosition;
                    pos.y += 10;
                    pos.z -= 6;
                    InternalCalls.CreateQueenTailAttack(pos, gameObject.transform.globalRotation);
                    queenState = QueenState.AXE_TAIL;
                }
                else
                {
                    //Reset counter
                    randomCounter = 0;
                    randomSelected = false;
                }

                break;
            case float i when (i > 80 && i <= 100):
                if ((CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, dashRadius)) && dashReady == true)
                {
                    Debug.Log("[ERROR] BOSS STATE PREPARE DASH");
                    dashReady = false;
                    dashTimer = dashAttackCooldown;
                    Animation.PlayAnimation(gameObject, "Boss_Dash");
                    queenState = QueenState.PREPARE_DASH;
                }
                else
                {
                    //Reset counter
                    randomCounter = 0;
                    randomSelected = false;
                }

                break;
        }

        //Acid spit
        if ((CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, acidSpitRadius)) && (!CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, clawRadius)) && randomCounter == 0 && acidSpitReady == true)
        {
            Debug.Log("[ERROR] BOSS STATE ACID SPIT");
            acidSpitReady = false;
            acidSpitTimer = acidSpitAttackCooldown;
            Animation.PlayAnimation(gameObject, "Boss_Cry");
            Audio.PlayAudio(gameObject, "QX_Acid");
            Vector3 pos = gameObject.transform.globalPosition;
            pos.y += 10;
            pos.z -= 6;
            InternalCalls.CreateQueenSpitAttack(pos, gameObject.transform.globalRotation);
            queenState = QueenState.ACID_SPIT;

        }
        else if ((CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, clawRadius)) && acidSpitReady == true)
        {
            Debug.Log("[ERROR] BOSS STATE WALK BACKWARDS");
            Animation.PlayAnimation(gameObject, "Boss_Walk");
            Animation.SetBackward(gameObject, "Boss_Walk", true);
            Audio.PlayAudio(gameObject, "QX_Move");
            queenState = QueenState.WALK_BACKWARDS;
        }
    }

    private void ClawCooldown()
    {
        if (clawTimer > 0 && !clawReady)
        {
            clawTimer -= Time.deltaTime;
            if (clawTimer <= 0)
            {
                clawReady = true;
            }
        }

    }

    private void AcidSpitCooldown()
    {
        if (acidSpitTimer > 0 && !acidSpitReady)
        {
            acidSpitTimer -= Time.deltaTime;
            if (acidSpitTimer <= 0)
            {
                acidSpitReady = true;
            }
        }

    }

    private void AxeCooldown()
	{
        if (axeTimer > 0 && !axeReady)
        {
            axeTimer -= Time.deltaTime;
            if (axeTimer <= 0)
            {
				axeReady = true;
            }
        }

    }

    private void DashCooldown()
	{
        if (dashTimer > 0 && !dashReady)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                dashReady = true;
            }
        }

    }

    private void RotateQueen()
    {
        Vector3 direction = player.transform.globalPosition - gameObject.transform.globalPosition;
        direction = direction.normalized;
        float angle = (float)Math.Atan2(direction.x, direction.z);

        //Debug.Log("Desired angle: " + (angle * Mathf.Rad2Deg).ToString());

        if (Math.Abs(angle * Mathf.Rad2Deg) < 1.0f)
            return;

        Quaternion dir = Quaternion.RotateAroundAxis(Vector3.up, angle);

        float rotationSpeed = Time.deltaTime * queenRotationSpeed;
        //Debug.Log("CS: Rotation speed: " + rotationSpeed.ToString());
        //Debug.Log("CS: Time: " + Time.deltaTime);

        Quaternion desiredRotation = Quaternion.Slerp(gameObject.transform.localRotation, dir, rotationSpeed);

        gameObject.SetRotation(desiredRotation);

        //Debug.Log("[ERROR] rotation:  " + gameObject.transform.localRotation);
    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Tail"  && queenState != QueenState.DEAD && !tailColdown)
        {
            Debug.Log("[ERROR] HIT!!");
            life -= 80;

            tailColdown = true;
        }
    }

    public bool CheckDistance(Vector3 first, Vector3 second, float checkRadius)
    {
        float deltaX = Math.Abs(first.x - second.x);
        float deltaY = Math.Abs(first.y - second.y);
        float deltaZ = Math.Abs(first.z - second.z);

        return deltaX <= checkRadius && deltaY <= checkRadius && deltaZ <= checkRadius;
    }
    public QueenState GetState()
    {
        return queenState;
    }

    private void isDeath()
    {
        if (life <= 0)
        {
            Animation.PlayAnimation(gameObject, "Boss_Die");
            Debug.Log("[ERROR] DEATH");
            gameObject.SetVelocity(new Vector3(0, 0, 0));
            Audio.PlayAudio(gameObject, "QX_Death");
            queenState = QueenState.DEAD;
        }
    }

    public void SetPause(bool pause)
    {
        if (pause)
        {
            pausedState = queenState;
            queenState = QueenState.PAUSED;
            Animation.PauseAnimation(gameObject);
        }
        else if (queenState == QueenState.PAUSED)
        {
            //If bool set to false when it was never paused, it will do nothing
            queenState = pausedState;
            Animation.ResumeAnimation(gameObject);
        }
    }
}