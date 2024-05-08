using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

using YmirEngine;

public class Player : YmirComponent
{
    public string saveName = "Player";

    #region DEFINE BASE VARS
    public enum STATE : int
    {
        NONE = -1,

        IDLE,
        MOVE,
        STOP,
        DASH,
        SHOOTING,
        RELOADING,
        SHOOT,
        DEAD,
        JUMP,
        TAILSWIPE,

        All_TYPES
    }

    enum INPUT : int
    {
        I_IDLE,
        I_MOVE,
        I_STOP,
        I_DASH,
        I_DASH_END,
        I_SHOOTING,
        I_SHOOTING_END,
        I_SHOOT,
        I_SHOOT_END,
        I_RELOAD,
        I_DEAD,
        I_JUMP,
        I_JUMP_END,
        I_ACID,
        I_ACID_END,
        I_PRED,
        I_PRED_END,
        I_SWIPE,
        I_SWIPE_END,
    }

    enum WEAPON : int
    {
        NONE = -1,

        SMG,
        SHOTGUN,
        TRACE,

        All_TYPES
    }

    //--------------------- State ---------------------\\
    public STATE currentState = STATE.NONE;   //NEVER SET THIS VARIABLE DIRECTLLY, ALLWAYS USE INPUTS
    private List<INPUT> inputsList = new List<INPUT>();

    //--------------------- Movement ---------------------\\
    //public float rotationSpeed = 2.0f;
    public float movementSpeed = 35.0f; // speed
    //private double angle = 0.0f;
    private float deathZone = 0.5f;

    //--------------------- Controller var ---------------------\\
    float x = 0;
    float y = 0;
    Vector3 gamepadInput;
    //bool isMoving = false;

    public bool deathAnimFinish;
    private float deathTimer;

    //--------------------- GOD mode ---------------------\\
    public bool godMode = false;
    #endregion

    #region DEFINE SHOOT VARS

    //--------------------- Shoot var ---------------------\\
    public float fireRate = 0; // rate of fire
    private float shootingTimer = 0.0f;
    //public float secondaryRate = 0.2f;
    private bool shootBefore = false;

    private bool isReloading = false;
    private float reloadTimer = 0.0f;
    public float reloadDuration = 1.0f; // reload speed

    public int ammo = 0;
    public int magsize = 5;

    public float smgRange = 30.0f;
    public float shootgunRange = 15.0f;
    public float traceRange = 100.0f;

    private int shootRumbleIntensity;
    private int shootRumbleDuration;

    private WEAPON weaponType = WEAPON.NONE;
    public WEAPON_TYPE weaponTypeTest = WEAPON_TYPE.NONE;
    //private Weapon currentWeapon = null;

    // Stats que no he visto implementadas, para inventario
    public float damageMultiplier = 0;
    public int resin = 10;

    // Lo usa el script de BH_Shotgun
    public Vector3 shotgunOffset = Vector3.zero;

    #endregion

    #region DEFINE SKILL VARS

    //--------------------- Dash ---------------------\\
    //public float dashforce = 1000.0f;
    private float dashTimer = 0.0f;
    private float jumpTimer = 0.0f;

    private bool hasDashed;
    private float dashCD = 3.5f;
    private float dashCDTimer;
    public float dashDuration = 0.250f;
    public float dashDistance = 200.0f;
    private float dashSpeed = 0.0f;
    //private float dashStartYPos = 0.0f;

    //--------------------- Predatory Rush ---------------------\\
    private float predatoryTimer;
    private float predatoryDuration = 6.0f;
    private float predatoryCDTimer;
    private float predatoryCD = 22.0f;
    private bool hasPred = false;

    //--------------------- Tail Swipe ---------------------\\
    public float swipeTimer;
    private float swipeDuration = 3f;
    private float swipeCDTimer;
    private float swipeCD = 13.0f;
    private bool hasSwipe = false;

    //private float angle;
    //private bool has360;
    //float initRot;

    //--------------------- Acidic Spit ------------------------\\
    public float acidicTimer;
    private float acidicDuration = 1.8f;
    private float acidicCDTimer;
    private float acidicCD = 7.0f;
    private bool hasAcidic = false;

    #endregion

    #region DEFINE MENUS

    public int currentLvl = (int)LEVEL.BASE;
    public int lastUnlockedLvl = (int)LEVEL.BASE;

    public string currentMenu = "";
    public bool setHover = false; // Guarrada temporal
    public List<string> itemsListString;
    public List<Item> itemsList;

    #endregion

    #region DEFINE EXTERNAL THINGS

    //--------------------- External GameObjects ---------------------\\
    private GameObject cameraObject;

    //--------------------- External Scripts ---------------------\\
    private UI_Bullets csBullets;
    private Health csHealth;

    private UI_Animation csUI_AnimationDash;
    private UI_Animation csUI_AnimationPredatory;
    private UI_Animation csUI_AnimationSwipe;
    private UI_Animation csUI_AnimationAcid;

    #endregion

    #region WEAPON GAMEOBJECTS

    public GameObject w_SMG_0;
    //public GameObject w_SMG_1;
    //public GameObject w_SMG_2;
    //public GameObject w_SMG_3a;
    //public GameObject w_SMG_3b;

    public GameObject w_Shotgun_0;
    //public GameObject w_Shotgun_1;
    //public GameObject w_Shotgun_2;
    //public GameObject w_Shotgun_3a;
    //public GameObject w_Shotgun_3b;

    public GameObject w_Plasma_0;
    //public GameObject w_Plasma_1;
    //public GameObject w_Plasma_2;
    //public GameObject w_Plasma_3a;
    //public GameObject w_Plasma_3b;

    //List<GameObject> weapons = new List<GameObject>();

    #endregion

    //Hay que dar valor a las variables en el start

    public void Start()
    {
        // Item map
        Globals.CreateItemDictionary();

        Audio.SetState("PlayerState", "Alive");
        Audio.SetState("CombatState", "Exploration");

        saveName = "Player";

        //angle = 0;
        //has360 = false;
        //

        deathAnimFinish = false;
        deathTimer = 3;

        weaponType = WEAPON.SMG;

        movementSpeed = 3000.0f;    //Antes 35

        //--------------------- Dash ---------------------\\
        dashDistance = 400.0f;     //Antes 2 

        dashTimer = 0f;
        dashDuration = 0.250f;
        dashCDTimer = 0;
        dashCD = 3.5f;

        jumpTimer = 0.0f;
        hasDashed = false;

        dashSpeed = dashDistance / dashDuration;

        //--------------------- Swipe ---------------------\\
        swipeTimer = 0;
        swipeDuration = 1f;
        swipeCDTimer = 0;
        swipeCD = 2.0f; //Es 13.0f
        hasSwipe = false;

        //--------------------- Predatory Rush ---------------------\\

        predatoryTimer = 0;
        predatoryDuration = 6.0f;
        predatoryCDTimer = 0;
        predatoryCD = 22.0f;
        hasPred = false;

        //--------------------- Acidic Spit ------------------------\\

        acidicTimer = 0;
        acidicDuration = 1.8f;
        acidicCDTimer = 0;
        acidicCD = 7.0f;
        hasAcidic = false;

        //--------------------- Get Player Scripts ---------------------\\
        GetPlayerScripts();

        //--------------------- Get Skills Scripts ---------------------\\
        GetSkillsScripts();

        //--------------------- Shoot ---------------------\\

        //--------- Weapons List -----------\\

        //weapons.Add(w_SMG_0);
        //weapons.Add(w_Shotgun_0);
        //weapons.Add(w_Plasma_0);

        GetWeaponVars();

        //currentWeapon = w_SMG_0.GetComponent<SMG>();

        //--------------------- Menus ---------------------\\
        itemsList = new List<Item>();
        itemsListString = new List<string>();

        if (currentLvl != (int)LEVEL.BASE)
        {
            Debug.Log("current: " + currentLvl.ToString());
            LoadPlayer();
        }

        {
            // TODO: Sara --> cosas pa probar items
            //for (int i = 0; i < 5; i++)
            //{
            //    Random random = new Random();

            //    ITEM_SLOT a = (ITEM_SLOT)random.Next((int)ITEM_SLOT.SIZE);
            //    ITEM_SLOT b = (ITEM_SLOT)random.Next((int)ITEM_SLOT.SIZE);
            //    //ITEM_SLOT b = ITEM_SLOT.ARMOR;
            //    ITEM_RARITY c = (ITEM_RARITY)random.Next((int)ITEM_RARITY.NONE);
            //    //bool d = (random.NextDouble() < 0.5 ? false : true);
            //    bool d = true;

            //    Item item = null;

            //    int x = random.Next(0, 3);
            //    switch (x)
            //    {
            //        case 0:
            //            {
            //                item = new Item(a, b, c, d, "Item " + i.ToString(), "This is: Item " + i.ToString(),
            //               "Assets/UI/Items Slots/Iconos/ResinVesselIconColor.png");
            //            }
            //            break;
            //        case 1:
            //            {
            //                item = new I_Equippable(a, b, c, d, "Item " + i.ToString(), "This is: Item " + i.ToString(),
            //        "Assets/UI/Items Slots/Iconos/ResinVesselIconColor.png",
            //        random.Next(100), random.Next(100), random.Next(100), random.Next(100), random.Next(100), random.Next(100));
            //            }
            //            break;
            //        case 2:
            //            {
            //                item = new I_Consumables(a, b, c, d, "Item " + i.ToString(), "This is: Item " + i.ToString(),
            //               "Assets/UI/Items Slots/Iconos/ResinVesselIconColor.png", random.Next(100), random.Next(100), random.Next(100));
            //            }
            //            break;
            //        default:
            //            break;
            //    }

            //    itemsList.Add(item);
            //    itemsListTest.Add(item.name);
            //}
        }

        //--------------------- Get Camera GameObject ---------------------\\
        cameraObject = InternalCalls.GetGameObjectByName("Main Camera");

        //--------------------- Set Animation Parameters ---------------------\\
        SetAnimParameters();

        currentState = STATE.IDLE;
    }

    public void Update()
    {
        //Debug.Log(currentState.ToString());
        // New Things WIP
        //Debug.Log("State: " + currentState);
        UpdateControllerInputs();

        ProcessInternalInput();
        ProcessExternalInput();
        ProcessState();

        UpdateState();

        if (Input.GetKey(YmirKeyCode.K) == KeyState.KEY_DOWN)
        {
            Audio.SetState("CombatState", "Exploration");
        }

        if (Input.GetKey(YmirKeyCode.L) == KeyState.KEY_DOWN)
        {
            Audio.SetState("CombatState", "Fight");
        }

        if (Input.GetKey(YmirKeyCode.F1) == KeyState.KEY_DOWN)
        {
            godMode = !godMode;
        }

        if (Input.GetKey(YmirKeyCode.KP_1) == KeyState.KEY_DOWN)
        {
            SwapWeapon(WEAPON.SMG);
        }

        if (Input.GetKey(YmirKeyCode.KP_2) == KeyState.KEY_DOWN)
        {
            SwapWeapon(WEAPON.SHOTGUN);
        }

        if (Input.GetKey(YmirKeyCode.KP_3) == KeyState.KEY_DOWN)
        {
            SwapWeapon(WEAPON.TRACE);
        }

        if (Input.GetKey(YmirKeyCode.F8) == KeyState.KEY_DOWN)
        {
            SavePlayer();
        }

        //Debug.Log("swipeCD = " + swipeCDTimer);

    }

    #region FSM
    private void ProcessInternalInput()
    {
        //--------------------- Dash Timers ---------------------\\
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0)
            {
                inputsList.Add(INPUT.I_DASH_END);
            }
        }

        if (dashCDTimer > 0)
        {
            dashCDTimer -= Time.deltaTime;

            if (dashCDTimer < csUI_AnimationDash.delay * csUI_AnimationDash.totalFrames && !csUI_AnimationDash.backwards)
            {
                // SARA: vuelve ui normal
                // Without ping-pong
                //csUI_AnimationDash.SetAnimationState(false);
                //csUI_AnimationDash.SetCurrentFrame(0, 0);

                // With ping-pong
                csUI_AnimationDash.Reset();
                csUI_AnimationDash.backwards = !csUI_AnimationDash.backwards;

                Debug.Log("dash cd: " + dashCDTimer.ToString() + " delay: " + (csUI_AnimationDash.delay * csUI_AnimationDash.totalFrames).ToString());
            }

            if (dashCDTimer <= 0)
            {
                hasDashed = false;
            }
        }

        //--------------------- Shoot Timer ---------------------\\
        if (currentState == STATE.SHOOTING && !shootBefore)
        {
            StartShoot();
            shootBefore = true;
        }
        else if (shootingTimer > 0)
        {
            shootingTimer -= Time.deltaTime;

            if (shootingTimer <= 0)
            {
                inputsList.Add(INPUT.I_SHOOT);
                //Debug.Log("In shoot");
            }
        }

        //--------------------- Reload Timer ---------------------\\
        if (isReloading)
        {
            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;

                if (reloadTimer <= 0)
                {
                    ammo = magsize;
                    if (csBullets != null) { csBullets.UseBullets(); }
                    isReloading = false;
                }
            }
        }

        //--------------------- Acidic Spit Timer ---------------------\\
        if (acidicTimer > 0)
        {
            acidicTimer -= Time.deltaTime;

            if (acidicTimer <= 0)
            {
                inputsList.Add(INPUT.I_ACID_END);
            }
        }

        if (acidicCDTimer > 0)
        {
            acidicCDTimer -= Time.deltaTime;

            if (acidicCDTimer < csUI_AnimationAcid.delay * csUI_AnimationAcid.totalFrames && !csUI_AnimationAcid.backwards)
            {
                // SARA: vuelve ui normal
                // Without ping-pong
                //csUI_AnimationAcid.SetAnimationState(false);
                //csUI_AnimationAcid.SetCurrentFrame(0, 0);

                // With ping-pong
                csUI_AnimationAcid.Reset();
                csUI_AnimationAcid.backwards = !csUI_AnimationAcid.backwards;

                Debug.Log("acid cd: " + acidicCDTimer.ToString() + " delay: " + (csUI_AnimationAcid.delay * csUI_AnimationAcid.totalFrames).ToString());
            }

            if (acidicCDTimer <= 0)
            {
                hasAcidic = false;
                //    // SARA: vuelve ui normal
                //    // Without ping-pong
                //    //csUI_AnimationAcid.SetAnimationState(false);
                //    //csUI_AnimationAcid.SetCurrentFrame(0, 0);

                //    // With ping-pong
                //    csUI_AnimationAcid.Reset();
                //    csUI_AnimationAcid.backwards = !csUI_AnimationAcid.backwards;
            }
        }

        //--------------------- Predatory Timer ---------------------\\
        if (predatoryTimer > 0)
        {
            predatoryTimer -= Time.deltaTime;

            if (predatoryTimer <= 0)
            {
                inputsList.Add(INPUT.I_PRED_END);
            }
        }

        if (predatoryCDTimer > 0)
        {
            predatoryCDTimer -= Time.deltaTime;

            if (predatoryCDTimer < csUI_AnimationPredatory.delay * csUI_AnimationPredatory.totalFrames && !csUI_AnimationPredatory.backwards)
            {
                // SARA: vuelve ui normal
                // Without ping-pong
                //csUI_AnimationPredatory.SetAnimationState(false);
                //csUI_AnimationPredatory.SetCurrentFrame(0, 0);

                // With ping-pong
                csUI_AnimationPredatory.Reset();
                csUI_AnimationPredatory.backwards = !csUI_AnimationPredatory.backwards;

                Debug.Log("predatory cd: " + predatoryCDTimer.ToString() + " delay: " + (csUI_AnimationPredatory.delay * csUI_AnimationPredatory.totalFrames).ToString());
            }

            if (predatoryCDTimer <= 0)
            {
                hasPred = false;
                //    // SARA: vuelve ui normal
                //    // Without ping-pong
                //    //csUI_AnimationPredatory.SetAnimationState(false);
                //    //csUI_AnimationPredatory.SetCurrentFrame(0, 0);

                //    // With ping-pong
                //    csUI_AnimationPredatory.Reset();
                //    csUI_AnimationPredatory.backwards = !csUI_AnimationPredatory.backwards;
            }
        }

        //--------------------- Tail Swipe Timer ---------------------\\
        if (swipeTimer > 0)
        {
            swipeTimer -= Time.deltaTime;

            if (swipeTimer <= 0)
            {
                inputsList.Add(INPUT.I_SWIPE_END);
            }
        }

        if (swipeCDTimer > 0)
        {
            swipeCDTimer -= Time.deltaTime;

            if (swipeCDTimer < csUI_AnimationSwipe.delay * csUI_AnimationSwipe.totalFrames && !csUI_AnimationSwipe.backwards)
            {
                // SARA: vuelve ui normal
                // Without ping-pong
                //csUI_AnimationSwipe.SetAnimationState(false);
                //csUI_AnimationSwipe.SetCurrentFrame(0, 0);

                // With ping-pong
                csUI_AnimationSwipe.Reset();
                csUI_AnimationSwipe.backwards = !csUI_AnimationSwipe.backwards;

                Debug.Log("predatory cd: " + swipeCDTimer.ToString() + " delay: " + (csUI_AnimationSwipe.delay * csUI_AnimationSwipe.totalFrames).ToString());
            }

            if (swipeCDTimer <= 0)
            {
                hasSwipe = false;
                //    // SARA: vuelve ui normal
                //    // Without ping-pong
                //    //csUI_AnimationSwipe.SetAnimationState(false);
                //    //csUI_AnimationSwipe.SetCurrentFrame(0, 0);

                //    // With ping-pong
                //    csUI_AnimationSwipe.Reset();
                //    csUI_AnimationSwipe.backwards = !csUI_AnimationSwipe.backwards;
            }
        }

        //--------------------- HP Detector-------------------- -\\
        if (csHealth != null && csHealth.currentHealth <= 0)
        {
            inputsList.Add(INPUT.I_DEAD);
        }

        //--------------------- Jump Timer (Useless) ---------------------\\
        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;

            if (jumpTimer <= 0)
            {
                inputsList.Add(INPUT.I_JUMP_END);
            }
        }
    }
    private void ProcessExternalInput()
    {
        //----------------- Debug KEY to test Die Animation -----------------\\
        if (Input.GetKey(YmirKeyCode.V) == KeyState.KEY_DOWN)
        {
            inputsList.Add(INPUT.I_DEAD);
        }

        if (currentState != STATE.STOP)
        {
            //----------------- Joystic -----------------\\
            if (JoystickMoving() == true)
            {
                inputsList.Add(INPUT.I_MOVE);
            }
            else if (currentState == STATE.MOVE && JoystickMoving() == false)
            {
                inputsList.Add(INPUT.I_IDLE);
                StopPlayer();
            }

            //----------------- Shoot -----------------\\
            if (Input.GetGamepadRightTrigger() > 0 && !isReloading && ammo > 0)
            {
                inputsList.Add(INPUT.I_SHOOTING);
            }
            else
            {
                inputsList.Add(INPUT.I_SHOOTING_END);
                shootBefore = false;
            }

            //----------------- Dash -----------------\\
            if (Input.GetGamepadLeftTrigger() > 0 && hasDashed == false && dashCDTimer <= 0)
            {
                hasDashed = true;
                inputsList.Add(INPUT.I_DASH);

                // SARA: start dash cooldown
                csUI_AnimationDash.Reset();
                csUI_AnimationDash.backwards = false;
                csUI_AnimationDash.SetAnimationState(true);
            }

            //----------------- Acidic Spit (Skill 1) -----------------\\
            if (Input.GetGamepadButton(GamePadButton.X) == KeyState.KEY_DOWN && hasAcidic == false && acidicCDTimer <= 0)
            {
                hasAcidic = true;
                inputsList.Add(INPUT.I_ACID);

                // SARA: start acidic cooldown
                csUI_AnimationAcid.Reset();
                csUI_AnimationAcid.backwards = false;
                csUI_AnimationAcid.SetAnimationState(true);
            }

            //----------------- Predatory Rush (Skill 2) -----------------\\
            if (Input.GetGamepadButton(GamePadButton.B) == KeyState.KEY_DOWN && hasPred == false && predatoryCDTimer <= 0)
            {
                hasPred = true;
                inputsList.Add(INPUT.I_PRED);

                // SARA: start predatory cooldown
                csUI_AnimationPredatory.Reset();
                csUI_AnimationPredatory.backwards = false;
                csUI_AnimationPredatory.SetAnimationState(true);
            }

            //----------------- Swipe (Skill 3) -----------------\\
            if (Input.GetGamepadButton(GamePadButton.Y) == KeyState.KEY_DOWN && hasSwipe == false && swipeCDTimer <= 0)
            {
                hasSwipe = true;
                inputsList.Add(INPUT.I_SWIPE);

                // SARA: start swipe cooldown
                csUI_AnimationSwipe.Reset();
                csUI_AnimationSwipe.backwards = false;
                csUI_AnimationSwipe.SetAnimationState(true);
            }

            //----------------- Reload -----------------\\
            if (Input.GetGamepadButton(GamePadButton.A) == KeyState.KEY_DOWN)
            {
                inputsList.Add(INPUT.I_RELOAD);
            }
        }

        else
        {
            // If player is on menu and presses B, quit menu
            if (Input.GetGamepadButton(GamePadButton.B) == KeyState.KEY_DOWN && currentMenu != "")
            {
                ToggleMenu(false);
            }
        }

        //----------------- Inventory -----------------\\
        if (Input.GetGamepadButton(GamePadButton.DPAD_RIGHT) == KeyState.KEY_DOWN && currentMenu == "")
        {
            currentMenu = "Inventory Menu";
            ToggleMenu(true);

            Debug.Log("Inventory Menu");
        }

        ////----------------- Upgrade -----------------\\
        //if (Input.GetGamepadButton(GamePadButton.DPAD_LEFT) == KeyState.KEY_DOWN && currentMenu == "") // Debug upgrade station
        //{
        //    currentMenu = "Upgrade Station";
        //    ToggleMenu(true);

        //    Debug.Log("Upgrade Station");
        //}

        ////----------------- Stash -----------------\\
        //if (Input.GetGamepadButton(GamePadButton.DPAD_UP) == KeyState.KEY_DOWN && currentMenu == "") // Debug stash 
        //{
        //    currentMenu = "Stash Canvas";
        //    ToggleMenu(true);

        //    Debug.Log("Stash Canvas");
        //}

        //----------------- Swap to SMG -----------------\\  Provisional!!!
        if (Input.GetKey(YmirKeyCode.Alpha1) == KeyState.KEY_DOWN)
        {
            SwapWeapon(WEAPON.SMG);
            Debug.Log("" + WEAPON.SMG);
        }

        //----------------- Swap to Shotgun -----------------\\  Provisional!!!
        if (Input.GetKey(YmirKeyCode.Alpha2) == KeyState.KEY_DOWN)
        {
            SwapWeapon(WEAPON.SHOTGUN);
            Debug.Log("" + WEAPON.SHOTGUN);
        }

        //----------------- Swap to Laser -----------------\\  Provisional!!!
        if (Input.GetKey(YmirKeyCode.Alpha3) == KeyState.KEY_DOWN)
        {
            SwapWeapon(WEAPON.TRACE);
            Debug.Log("" + WEAPON.TRACE);
        }

        //----------------- Desbugear -----------------\\
        //if (Input.GetGamepadButton(GamePadButton.Y) == KeyState.KEY_DOWN)
        //{
        //    //Debug.Log("aaaa");
        //    inputsList.Add(INPUT.I_JUMP);
        //    Input.Rumble_Controller(50);
        //}
    }

    private void ProcessState()
    {
        while (inputsList.Count > 0)
        {
            INPUT input = inputsList[0];

            switch (currentState)
            {
                case STATE.NONE:
                    Debug.Log("ERROR STATE");
                    break;

                case STATE.IDLE:
                    //Debug.Log("IDLE");
                    switch (input)
                    {
                        case INPUT.I_MOVE:
                            currentState = STATE.MOVE;
                            StartMove();
                            break;

                        case INPUT.I_STOP:
                            currentState = STATE.STOP;
                            StopPlayer();
                            break;

                        case INPUT.I_DASH:
                            currentState = STATE.DASH;
                            StartDash();
                            break;

                        case INPUT.I_ACID:
                            StartAcidicSpit();
                            break;

                        case INPUT.I_ACID_END:
                            EndAcidicSpit();
                            break;

                        case INPUT.I_PRED:
                            StartPredRush();
                            break;

                        case INPUT.I_PRED_END:
                            EndPredRush();
                            break;

                        case INPUT.I_SWIPE:
                            currentState = STATE.TAILSWIPE;
                            StartTailSwipe();
                            break;

                        case INPUT.I_JUMP:
                            currentState = STATE.JUMP;
                            StartJump();
                            break;

                        case INPUT.I_SHOOTING:
                            currentState = STATE.SHOOTING;
                            StartShooting();
                            break;

                        case INPUT.I_RELOAD:
                            currentState = STATE.RELOADING;
                            StartReload();
                            break;

                        case INPUT.I_DEAD:
                            currentState = STATE.DEAD;
                            StartDeath();
                            break;
                    }
                    break;

                case STATE.MOVE:
                    //Debug.Log("MOVE");
                    switch (input)
                    {
                        case INPUT.I_IDLE:
                            currentState = STATE.IDLE;
                            StartIdle();
                            break;

                        case INPUT.I_STOP:
                            currentState = STATE.STOP;
                            StopPlayer();
                            break;

                        case INPUT.I_DASH:
                            currentState = STATE.DASH;
                            StartDash();
                            break;

                        case INPUT.I_ACID:
                            StartAcidicSpit();
                            break;

                        case INPUT.I_ACID_END:
                            EndAcidicSpit();
                            break;

                        case INPUT.I_PRED:
                            StartPredRush();
                            break;

                        case INPUT.I_PRED_END:
                            EndPredRush();
                            break;

                        case INPUT.I_SWIPE:
                            currentState = STATE.TAILSWIPE;
                            StartTailSwipe();
                            break;

                        case INPUT.I_JUMP:
                            currentState = STATE.JUMP;
                            StartJump();
                            break;

                        case INPUT.I_SHOOTING:
                            currentState = STATE.SHOOTING;
                            StartShooting();
                            break;

                        case INPUT.I_RELOAD:
                            currentState = STATE.RELOADING;
                            StartReload();
                            break;

                        case INPUT.I_DEAD:
                            currentState = STATE.DEAD;
                            StartDeath();
                            break;
                    }
                    break;


                case STATE.DASH:
                    //Debug.Log("DASH");
                    switch (input)
                    {
                        case INPUT.I_STOP:
                            currentState = STATE.STOP;
                            StopPlayer();
                            break;

                        case INPUT.I_DASH_END:
                            currentState = STATE.IDLE;
                            EndDash();
                            break;

                        case INPUT.I_PRED_END:
                            EndPredRush();
                            break;

                        case INPUT.I_DEAD:
                            currentState = STATE.DEAD;
                            StartDeath();
                            break;
                    }
                    break;

                case STATE.JUMP:
                    //Debug.Log("JUMP");
                    switch (input)
                    {
                        case INPUT.I_STOP:
                            currentState = STATE.STOP;
                            StopPlayer();
                            break;

                        case INPUT.I_JUMP_END:
                            currentState = STATE.IDLE;
                            EndJump();
                            break;

                        case INPUT.I_PRED_END:
                            EndPredRush();
                            break;

                        case INPUT.I_DEAD:
                            currentState = STATE.DEAD;
                            StartDeath();
                            break;
                    }
                    break;


                case STATE.SHOOTING:
                    //Debug.Log("SHOOTING");
                    switch (input)
                    {
                        case INPUT.I_STOP:
                            currentState = STATE.STOP;
                            StopPlayer();
                            break;

                        case INPUT.I_DASH:
                            currentState = STATE.DASH;
                            StartDash();
                            break;

                        case INPUT.I_PRED_END:
                            EndPredRush();
                            break;

                        case INPUT.I_JUMP:
                            currentState = STATE.JUMP;
                            StartJump();
                            break;

                        case INPUT.I_SHOOTING_END:
                            currentState = STATE.IDLE;
                            EndShooting();
                            //StartIdle(); //Trigger de la animacion
                            break;

                        case INPUT.I_SHOOT:
                            currentState = STATE.SHOOT;
                            StartShoot();
                            break;

                        case INPUT.I_RELOAD:
                            currentState = STATE.RELOADING;
                            StartReload();
                            break;

                        case INPUT.I_DEAD:
                            currentState = STATE.DEAD;
                            StartDeath();
                            break;
                    }
                    break;

                case STATE.SHOOT:
                    //Debug.Log("SHOOT");
                    switch (input)
                    {
                        case INPUT.I_STOP:
                            currentState = STATE.STOP;
                            StopPlayer();
                            break;

                        case INPUT.I_SHOOT_END:
                            currentState = STATE.SHOOTING;
                            StartShooting();
                            break;

                        case INPUT.I_PRED_END:
                            EndPredRush();
                            break;

                        case INPUT.I_DEAD:
                            currentState = STATE.DEAD;
                            StartDeath();
                            break;
                    }
                    break;

                case STATE.RELOADING:
                    switch (input)
                    {
                        case INPUT.I_MOVE:
                            currentState = STATE.MOVE;
                            StartMove();
                            break;

                        case INPUT.I_STOP:
                            currentState = STATE.STOP;
                            StopPlayer();
                            break;

                        case INPUT.I_DASH:
                            currentState = STATE.DASH;
                            StartDash();
                            break;

                        case INPUT.I_PRED_END:
                            EndPredRush();
                            break;

                        case INPUT.I_JUMP:
                            currentState = STATE.JUMP;
                            StartJump();
                            break;

                        case INPUT.I_SHOOTING:
                            currentState = STATE.SHOOTING;
                            StartShooting();
                            break;

                        case INPUT.I_RELOAD:
                            currentState = STATE.RELOADING;
                            StartReload();
                            break;

                        case INPUT.I_DEAD:
                            currentState = STATE.DEAD;
                            StartDeath();
                            break;
                    }
                    break;

                case STATE.STOP:
                    //Debug.Log("Stop");
                    switch (input)
                    {
                        case INPUT.I_STOP:
                            currentState = STATE.STOP;
                            StopPlayer();
                            break;

                            //case INPUT.I_IDLE:
                            //    currentState = STATE.IDLE;
                            //    //StartIdle(); //Trigger de la animacion //Arreglar esto
                            //    break;
                    }
                    break;

                case STATE.TAILSWIPE:
                    //Debug.Log("Tail Swipe");
                    switch (input)
                    {
                        //case INPUT.I_STOP:
                        //    currentState = STATE.STOP;
                        //    StopPlayer();
                        //    break;

                        case INPUT.I_PRED_END:
                            EndPredRush();
                            break;

                        case INPUT.I_SWIPE_END:
                            currentState = STATE.IDLE;
                            EndTailSwipe();
                            break;

                        case INPUT.I_DEAD:
                            currentState = STATE.DEAD;
                            StartDeath();
                            break;
                    }
                    break;

                default:
                    Debug.Log("No State? :(");
                    break;
            }
            inputsList.RemoveAt(0);
        }
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case STATE.NONE:
                break;
            case STATE.IDLE:
                break;
            case STATE.MOVE:
                UpdateMove();
                break;
            case STATE.STOP:
                StopPlayer();
                break;
            case STATE.DASH:
                UpdateDash();
                break;
            case STATE.TAILSWIPE:
                UpdateTailSwipe();
                break;
            case STATE.JUMP:
                UpdateJump();
                break;
            case STATE.SHOOTING:
                UpdateShooting();
                break;
            case STATE.RELOADING:
                break;
            case STATE.SHOOT:
                break;
            case STATE.DEAD:
                UpdateDeath();
                break;
            default:
                Debug.Log("No State? :(");
                break;
        }
    }
    #endregion

    #region IDLE
    private void StartIdle()
    {
        Animation.PlayAnimation(gameObject, "Raisen_Idle");
    }
    #endregion

    #region SHOOT

    private void StartShooting()
    {
        // Trigger animacion disparar
        // Futuro autoapuntado
        shootingTimer = fireRate;
    }
    private void StartShoot()
    {
        StopPlayer();
        Animation.PlayAnimation(gameObject, "Raisen_Shooting");
        //Logica del disparo depende del arma equipada
        switch (weaponType)
        {
            case WEAPON.SMG:
                SmgShoot();
                break;
            case WEAPON.SHOTGUN:
                ShotgunShoot();
                break;
            case WEAPON.TRACE:
                TraceShoot();
                break;
            default:
                SmgShoot();
                break;
        }

        if (!godMode)
        {
            --ammo;
            if (csBullets != null) { csBullets.UseBullets(); }
        }

        inputsList.Add(INPUT.I_SHOOT_END);
    }
    private void UpdateShooting()
    {
        if (JoystickMoving() == true)
            HandleRotation();
    }

    private void EndShooting()
    {
        // Reset del futuro autoapuntado
        Animation.PlayAnimation(gameObject, "Raisen_Idle");
    }
    private void StartReload()
    {
        switch (weaponType)
        {
            case WEAPON.SMG:
                Audio.PlayAudio(gameObject, "W_FirearmReload");
                break;
            case WEAPON.SHOTGUN:
                Audio.PlayAudio(gameObject, "W_FSADReload");
                break;
            case WEAPON.TRACE:
                Audio.PlayAudio(gameObject, "W_PlasmaReload");
                break;
            default:
                Audio.PlayAudio(gameObject, "W_FirearmReload");
                break;
        }

        isReloading = true;
        reloadTimer = reloadDuration;
    }

    private void SmgShoot()
    {
        Audio.PlayAudio(gameObject, "P_Shoot");
        Input.Rumble_Controller(shootRumbleDuration, shootRumbleIntensity);

        if (!godMode)
        {
            --ammo;
            //if (csBullets!= null){ csBullets.UseBullets(); }
        }

        //Particles FX
        GameObject particles = GetParticles(gameObject, "ParticlesSmg");
        Particles.PlayParticles(particles);

        //Bullet
        GameObject bulletParticles = GetParticles(gameObject, "ParticlesSmgBullet");
        Particles.ParticleShoot(bulletParticles, gameObject.transform.GetForward().normalized);
        Particles.PlayParticles(bulletParticles);

        Debug.Log("Forward: " + gameObject.transform.GetForward().normalized.x + " " + gameObject.transform.GetForward().normalized.y + " " + gameObject.transform.GetForward().normalized.z);

        Vector3 offset = new Vector3(0, 15, 0);

        //Distancias y posicion para que la bala salga desde delante del player
        //Vector3 offsetDirection = gameObject.transform.GetForward().normalized;
        //float distance = 20.0f;
        //Vector3 pos = gameObject.transform.globalPosition + offset + (offsetDirection * distance);

        //Rotacion desde la que se crea la bala (la misma que el game object que le dispara)
        //Quaternion rot = gameObject.transform.globalRotation;

        //Tamaño de la bala
        //Vector3 scale = new Vector3(2.0f, 2.0f, 2.0f);

        //Crea la bala
        //InternalCalls.CreateBullet(pos, rot, scale);

        GameObject target;
        target = gameObject.RaycastHit(gameObject.transform.globalPosition + offset, gameObject.transform.GetForward(), 100.0f);

        if (target != null)
        {

            Debug.Log(target.Name);

            if (target.Tag != "Enemy")
            {

                Audio.PlayAudio(gameObject, "W_FirearmSurf");
            }
            else
            {
                Audio.PlayAudio(gameObject, "W_FirearmEnemy");
                //---------------Xiao: Gurrada Pendiente de Cambiar----------------------------
                FaceHuggerBaseScript aux = target.GetComponent<FaceHuggerBaseScript>();

                if (aux != null)
                {
                    aux.life -= 5;
                }

                DroneXenomorphBaseScript aux2 = target.GetComponent<DroneXenomorphBaseScript>();
                if (aux2 != null)
                {
                    aux2.life -= 5;
                }

                QueenXenomorphBaseScript aux3 = target.GetComponent<QueenXenomorphBaseScript>();
                if (aux3 != null)
                {
                    aux3.life -= 5;
                }
                Debug.Log("[ERROR] HIT ENEMy");
                //-----------------------------------------------------------------------------------
                // Sparkle particle
                // Play bullet hit wall SFX
            }
        }

        inputsList.Add(INPUT.I_SHOOT_END);
    }

    private void ShotgunShoot()
    {
        Audio.PlayAudio(gameObject, "W_FSADShot");
        Audio.PlayAudio(gameObject, "W_FSADCapRel");

        Input.Rumble_Controller(shootRumbleDuration, shootRumbleIntensity);

        if (!godMode)
        {
            --ammo;
            //if (csBullets!= null){ csBullets.UseBullets(); }
        }

        GameObject shotgunParticles = GetParticles(gameObject, "ParticlesShotgun");
        Particles.ParticleShoot(shotgunParticles, gameObject.transform.GetForward().normalized);
        Particles.PlayParticles(shotgunParticles);

        Vector3 offsetDirection = gameObject.transform.GetForward().normalized;
        float distance = 40.0f;
        Vector3 offset = new Vector3(0, 15, 0);
        shotgunOffset = gameObject.transform.globalPosition + offset + (offsetDirection * distance);

        Quaternion rot = gameObject.transform.globalRotation * new Quaternion(0.7071f, 0.0f, 0.0f, -0.7071f); // <- -90º Degree Quat

        InternalCalls.CreateShotgunSensor(shotgunOffset, rot, gameObject.transform.GetRight());

        inputsList.Add(INPUT.I_SHOOT_END);
    }

    private void TraceShoot()
    {
        Audio.PlayAudio(gameObject, "W_PlasmaShot");

        Input.Rumble_Controller(shootRumbleDuration, shootRumbleIntensity);

        GameObject traceParticles = GetParticles(gameObject, "ParticlesTraceShoot");
        Particles.ParticleShoot(traceParticles, gameObject.transform.GetForward().normalized);
        Particles.PlayParticles(traceParticles);

        if (!godMode)
        {
            --ammo;
            //if (csBullets!= null){ csBullets.UseBullets(); }
        }

        Vector3 offset = new Vector3(0, 15, 0);

        GameObject target;
        target = gameObject.RaycastHit(gameObject.transform.globalPosition + offset, gameObject.transform.GetForward(), 30.0f);

        if (target != null)
        {

            Debug.Log(target.Name);

            if (target.Tag != "Enemy")
            {
                Audio.PlayAudio(gameObject, "W_PlasmaSurf");
            }
            else
            {
                Audio.PlayAudio(gameObject, "W_PlasmaEnemy");

                //---------------Xiao: Gurrada Pendiente de Cambiar----------------------------
                FaceHuggerBaseScript aux = target.GetComponent<FaceHuggerBaseScript>();

                if (aux != null)
                {
                    aux.life -= 10;
                }

                DroneXenomorphBaseScript aux2 = target.GetComponent<DroneXenomorphBaseScript>();
                if (aux2 != null)
                {
                    aux2.life -= 10;
                }

                QueenXenomorphBaseScript aux3 = target.GetComponent<QueenXenomorphBaseScript>();
                if (aux3 != null)
                {
                    aux3.life -= 10;
                }
                Debug.Log("[ERROR] HIT ENEMy");
                //-----------------------------------------------------------------------------------
            }
        }
        // List<GameObject> targets = new List<GameObject>();

        // for (float tracerCurrentDistance = 0; tracerCurrentDistance < traceRange; tracerCurrentDistance += 0) {

        //     Debug.Log("Tracer CurrentDistance" + tracerCurrentDistance);

        //     GameObject tempTarget = gameObject.RaycastHit(gameObject.transform.globalPosition + offset + (tracerCurrentDistance * gameObject.transform.GetForward()), gameObject.transform.GetForward(), traceRange - tracerCurrentDistance);

        //     if (tempTarget != null && !targets.Contains(tempTarget)) {

        //         targets.Add(tempTarget);
        //         tracerCurrentDistance = Mathf.Distance(gameObject.transform.globalPosition, tempTarget.transform.globalPosition);
        //     }
        //     else
        //     {
        //         tracerCurrentDistance = traceRange;
        //     }
        // }

        //for (int i = 0; i < targets.Count(); i++)
        // {
        //     Debug.Log("Tracer" + i + "Hit:"+ targets[i].Name);

        //     if (targets[i].Tag != "Enemy")
        //     {
        //         // Sparkle particle
        //         // Play bullet hit wall SFX
        //     }
        //     else
        //     {
        //         // Damage enemy
        //         // Blood particle

        //     }
        // }

        inputsList.Add(INPUT.I_SHOOT_END);
    }

    private void GetWeaponVars()
    {
        switch (weaponType)
        {
            case WEAPON.SMG:

                magsize = 35;
                reloadDuration = 1.8f;
                //To do
                //dmg = ?
                fireRate = 0.1f;
                //range = ?

                shootRumbleIntensity = 5;
                shootRumbleDuration = 50;

                //for (int i = 0; i < weapons.Count(); i++)
                //{
                //    weapons[i].SetActive(false);
                //}

                //w_SMG_0.SetActive(true);

                break;

            case WEAPON.SHOTGUN:

                magsize = 8;
                reloadDuration = 2.7f;
                //To do
                //dmg = ?
                fireRate = 1.2f;
                //range = ?

                shootRumbleIntensity = 10;
                shootRumbleDuration = 200;

                //for (int i = 0; i < weapons.Count(); i++)
                //{
                //    weapons[i].SetActive(false);
                //}

                //w_Shotgun_0.SetActive(true);
                break;

            case WEAPON.TRACE:

                magsize = 200;
                reloadDuration = 3f;
                //To do
                //dmg = ?
                fireRate = 0.03f;
                //range = ?

                //for (int i = 0; i < weapons.Count(); i++)
                //{
                //    weapons[i].SetActive(false);
                //}

                //w_Plasma_0.SetActive(true);
                break;
        }

        ammo = magsize;
        reloadTimer = reloadDuration;

        csBullets.UseBullets();
    }

    private void SwapWeapon(WEAPON type)
    {
        weaponType = type;
        GetWeaponVars();
    }
    #endregion

    #region DASH
    private void StartDash()
    {
        Animation.PlayAnimation(gameObject, "Raisen_Dash");
        Audio.PlayAudio(gameObject, "P_Dash");

        //Sistema de particulas
        GameObject particles = GetParticles(gameObject, "ParticlesDash");
        Particles.PlayParticles(particles);

        Input.Rumble_Controller(100, 7);
        StopPlayer();
        dashTimer = dashDuration;
        //dashStartYPos = gameObject.transform.localPosition.y;
    }
    private void UpdateDash()
    {
        gameObject.SetImpulse(gameObject.transform.GetForward() * dashSpeed * Time.deltaTime);
    }
    private void EndDash()
    {
        StopPlayer();
        dashCDTimer = dashCD;
        //gameObject.transform.localPosition.y = dashStartYPos;
        Animation.PlayAnimation(gameObject, "Raisen_Idle"); // Chuekada para la entrega, si ves esto ponlo bien porfa no lo ignores

    }

    private void StartJump()
    {
        jumpTimer = dashDuration;
    }
    private void UpdateJump()
    {
        gameObject.SetImpulse(new Vector3(0, 1, 0) * dashSpeed);
    }
    private void EndJump()
    {
        StopPlayer();
    }
    #endregion

    #region Joystick
    private bool JoystickMoving()
    {
        //Debug.Log("Magnitude:" + gamepadInput.magnitude);
        return gamepadInput.magnitude > deathZone;
    }

    private void UpdateControllerInputs()
    {
        x = Input.GetLeftAxisX();
        y = Input.GetLeftAxisY();

        gamepadInput = new Vector3(x, y, 0f);
    }
    #endregion

    #region COLLISION
    public void OnCollisionEnter(GameObject other)
    {
        //Debug.Log("Peedrito");
    }
    #endregion

    #region PLAYER

    private void StartMove()
    {
        //Trigger de la animacion
        Animation.PlayAnimation(gameObject, "Raisen_Walk");
        //Trigger del SFX de caminar
        //Vector3 impulse = new Vector3(0.0f,0.0f,0.01f);
        //gameObject.SetImpulse(gameObject.transform.GetForward() * 0.5f);
    }
    private void UpdateMove()
    {
        //Debug.Log("Fuersa:" + gameObject.transform.GetForward());
        //Vector3 forward = gameObject.transform.GetForward();
        //forward.y = 0f;

        HandleRotation();

        Vector3 gravity = new Vector3(0, -20, 0);
        gameObject.SetVelocity((gameObject.transform.GetForward() * movementSpeed * Time.deltaTime) + gravity);

        //if (gamepadInput.x > 0)
        //{
        //    gameObject.SetVelocity(cameraObject.transform.GetRight() * movementSpeed * -1);
        //}
        //if (gamepadInput.x < 0)
        //{
        //    gameObject.SetVelocity(cameraObject.transform.GetRight() * movementSpeed);
        //}
    }

    private void StopPlayer()
    {
        //Debug.Log("Stopping");
        gameObject.SetVelocity(new Vector3(0, 0, 0));
        gameObject.ClearForces();
    }

    private void HandleRotation()
    {
        Vector3 aX = new Vector3(gamepadInput.x, 0, gamepadInput.y);
        aX = Vector3.Normalize(aX);

        Quaternion targetRotation = Quaternion.identity;

        Vector3 aY = new Vector3(0, 1, 0);

        if (aX != Vector3.zero)
        {
            float angle = 0;

            if (aX.x >= 0)
            {
                angle = (float)Math.Acos(Vector3.Dot(aX, aY) - 1);
            }
            else if (aX.x < 0)
            {
                angle = -(float)Math.Acos(Vector3.Dot(aX, aY) - 1);
            }

            // Add 45 degrees to the calculated angle
            angle -= Mathf.PI / 4f; // Rotate 45 degrees to the right

            // Construct quaternion rotation with an inverted axis for correcting left and right inversion
            targetRotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.up);
        }

        // Apply rotation
        gameObject.SetRotation(targetRotation);
    }

    private void StartDeath()
    {
        gameObject.SetVelocity(new Vector3(0, 0, 0));
        gameObject.ClearForces();
        Animation.PlayAnimation(gameObject, "Raisen_Death");
        deathTimer = 3;
    }

    private void UpdateDeath()
    {
        if (deathTimer > 0)
        {
            deathTimer -= Time.deltaTime;

            if (deathTimer <= 0)
            {
                csHealth.DeathScreen();
            }
        }
    }

    public void PlayerStopState(bool stop)
    {
        currentState = (stop) ? STATE.STOP : STATE.IDLE;
    }

    public void ToggleMenu(bool open)
    {
        GameObject canvas = InternalCalls.GetGameObjectByName(currentMenu);
        Debug.Log("" + currentMenu);

        canvas.SetActive(open);
        PlayerStopState(open);

        if (!open)
        {
            currentMenu = "";
        }
        else
        {
            setHover = true;
            UI.SetFirstFocused(canvas);
        }
    }

    // External scripts
    private void GetPlayerScripts()
    {
        Debug.Log("" + gameObject.Name);
        csHealth = gameObject.GetComponent<Health>();
        csBullets = gameObject.GetComponent<UI_Bullets>();
    }

    private void GetSkillsScripts()
    {
        GameObject gameObject1 = InternalCalls.GetGameObjectByName("Frame (1)");

        //Debug.Log(gameObject.name);
        if (gameObject1 != null)
        {
            csUI_AnimationSwipe = gameObject1.GetComponent<UI_Animation>();
        }

        GameObject gameObject2 = InternalCalls.GetGameObjectByName("Frame (2)");
        if (gameObject2 != null)
        {
            csUI_AnimationPredatory = gameObject2.GetComponent<UI_Animation>();
        }

        GameObject gameObject3 = InternalCalls.GetGameObjectByName("Frame (3)");
        if (gameObject3 != null)
        {
            csUI_AnimationAcid = gameObject3.GetComponent<UI_Animation>();
        }

        GameObject gameObject4 = InternalCalls.GetGameObjectByName("Frame (4)");
        if (gameObject4 != null)
        {
            csUI_AnimationDash = gameObject4.GetComponent<UI_Animation>();
        }
    }

    private void SetAnimParameters()
    {
        Animation.SetLoop(gameObject, "Raisen_Idle", true);
        Animation.SetLoop(gameObject, "Raisen_Walk", true);
        Animation.SetLoop(gameObject, "Raisen_Dash", true);

        Animation.SetSpeed(gameObject, "Raisen_Dash", 4);
        Animation.SetSpeed(gameObject, "Raisen_Spit", 0.1f);

        Animation.SetResetToZero(gameObject, "Raisen_Death", false);

        Animation.AddBlendOption(gameObject, "Raisen_Idle", "Raisen_Walk", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Idle", "Raisen_Death", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Idle", "Raisen_Spin", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Idle", "Raisen_Shooting", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Idle", "Raisen_Spit", 5.0f);
        //Animation.AddBlendOption(gameObject, "Raisen_Idle", "Raisen_Dash", 5.0f);


        Animation.AddBlendOption(gameObject, "Raisen_Walk", "Raisen_Idle", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Walk", "Raisen_Death", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Walk", "Raisen_Spin", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Walk", "Raisen_Shooting", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Walk", "Raisen_Spit", 5.0f);
        //Animation.AddBlendOption(gameObject, "Raisen_Walk", "Raisen_Dash", 5.0f);


        Animation.AddBlendOption(gameObject, "Raisen_Shooting", "Raisen_Idle", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Shooting", "Raisen_Walk", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Shooting", "Raisen_Death", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Shooting", "Raisen_Spin", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Shooting", "Raisen_Shooting", 2.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Shooting", "Raisen_Spit", 2.0f);
        //Animation.AddBlendOption(gameObject, "Raisen_Run", "Raisen_Dash", 5.0f);

        Animation.AddBlendOption(gameObject, "Raisen_Spin", "Raisen_Idle", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Spin", "Raisen_Walk", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Spin", "Raisen_Death", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Spin", "Raisen_Shooting", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Spin", "Raisen_Spit", 5.0f);

        Animation.AddBlendOption(gameObject, "Raisen_Dash", "Raisen_Idle", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Dash", "Raisen_Run", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Dash", "Raisen_Death", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Dash", "Raisen_Walk", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Dash", "Raisen_Spin", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Dash", "Raisen_Shooting", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Dash", "Raisen_Spit", 5.0f);

        Animation.AddBlendOption(gameObject, "Raisen_Spit", "Raisen_Idle", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Spit", "Raisen_Walk", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Spit", "Raisen_Death", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Spit", "Raisen_Spin", 5.0f);
        Animation.AddBlendOption(gameObject, "Raisen_Spit", "Raisen_Shooting", 5.0f);

        Animation.PlayAnimation(gameObject, "Raisen_Idle");
    }

    #endregion

    #region PREDATORY RUSH
    private void StartPredRush()
    {
        //trigger del sonido
        Audio.PlayAudio(gameObject, "P_PredRush");

        GameObject predRushParticles = GetParticles(gameObject, "ParticlesPredRush");
        Particles.PlayParticles(predRushParticles);

        //trigger de la animacion

        //cambio de variables
        movementSpeed = movementSpeed * 1.5f;
        //Increase armor by * 1.3
        fireRate = fireRate * 0.7f;
        reloadDuration = reloadDuration * 0.5f;
        //Reduce dash CD * 0,5

        predatoryTimer = predatoryDuration;
    }

    private void EndPredRush()
    {
        //volver las variables a su valor original

        movementSpeed = movementSpeed / 1.5f;
        //Decrease armor by / 1.3
        fireRate = fireRate / 0.7f;
        reloadDuration = reloadDuration / 0.5f;
        //Increase dash CD / 0,5

        predatoryCDTimer = predatoryCD;
    }

    #endregion

    #region TAIL SWIPE
    private void StartTailSwipe()
    {
        //trigger del sonido
        Animation.PlayAnimation(gameObject, "Raisen_Spin");
        Audio.PlayAudio(gameObject, "P_TailSweep");

        GameObject particles = GetParticles(gameObject, "Tail Particles");
        Particles.PlayParticles(particles);

        //trigger de la animacion
        //Setup de todo lo necesario

        StopPlayer();

        //Vector3 offset = new Vector3(0, 15, 0);

        //Funciona pero es rarete
        //Vector3 pos = gameObject.transform.globalPosition + (gameObject.transform.GetForward() * -2.5f);
        //Quaternion rot = gameObject.transform.globalRotation;

        Vector3 pos = gameObject.transform.globalPosition;
        Quaternion rot = gameObject.transform.globalRotation;

        InternalCalls.CreateTailSensor(pos, rot);

        //has360 = false;
        //initRot = gameObject.transform.globalRotation.y * Mathf.Rad2Deg;
        //angle = gameObject.transform.globalRotation.y * Mathf.Rad2Deg;

        swipeTimer = swipeDuration;
    }

    private void UpdateTailSwipe()
    {
        //Funciona pero es rarete
        //if (angle < (initRot + 360) && has360 == false)
        //{
        //    Debug.Log("" + angle);
        //    angle += 2;
        //    //gameObject.transform.globalRotation.y
        //    Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        //    gameObject.SetRotation(targetRotation);
        //}
        //else
        //{
        //    has360 = true;
        //}

        //angle += 3 * Time.deltaTime;
    }

    private void EndTailSwipe()
    {
        //StopPlayer();
        //Delete de la hitbox de la cola
        Animation.PlayAnimation(gameObject, "Raisen_Idle");
        swipeCDTimer = swipeCD;

    }

    public void LookAt(float angle)
    {
        if (Math.Abs(angle * Mathf.Rad2Deg) < 1.0f)
            return;

        Quaternion dir = Quaternion.RotateAroundAxis(Vector3.up, angle);

        float rotationSpeed = Time.deltaTime * 1;


        Quaternion desiredRotation = Quaternion.Slerp(gameObject.transform.localRotation, dir, rotationSpeed);

        gameObject.SetRotation(desiredRotation);

    }

    #endregion

    #region ACIDIC SPIT

    private void StartAcidicSpit()
    {
        //Trigger del sonido
        Audio.PlayAudio(gameObject, "P_AcidSpit");

        Animation.PlayAnimation(gameObject, "Raisen_Shooting");

        GameObject acidicParticles = GetParticles(gameObject, "ParticlesAcidic");
        Particles.PlayParticles(acidicParticles);

        //Trigger de la animación

        // --- Creación de la bola de acido ---

        //Offset para que la bola salga a la altura del torso del player
        Vector3 offset = new Vector3(0, 15, 0);

        //Posicion desde la que se crea la bala (la misma que el game object que le dispara)
        Vector3 pos = gameObject.transform.globalPosition + offset + (gameObject.transform.GetForward() * 2);

        //Crea la bola de acido
        InternalCalls.CreateAcidicSpit("AcidSpit", pos);

        acidicTimer = acidicDuration;
    }

    private void EndAcidicSpit()
    {
        Animation.PlayAnimation(gameObject, "Raisen_Idle");
        acidicCDTimer = acidicCD;
    }

    #endregion

    #region GetParticlesByName

    //TONI: Lo pongo aqui porque servirá para las habilidades y disparo

    /*Para hacer funcionar las particulas hay que hacerle un hijo ParticleSystem al player
    y llamarlo de la manera que quieras, luego pasas por parametro ese nombre aqui y haces
    Particle.PlayEmitter del game object con las particulas (ejemplo en el dash)*/

    private GameObject GetParticles(GameObject go, string pName)
    {
        return InternalCalls.GetChildrenByName(go, pName);
    }

    #endregion

    #region SaveLoad
    // TODO: Sara --> cambiar al arma buena cuando este hecho
    public void SavePlayer()
    {
        SaveLoad.CreateSaveGameFile(Globals.saveGameDir, saveName);

        SaveLoad.SaveInt(Globals.saveGameDir, saveName, "Last unlocked Lvl", (int)lastUnlockedLvl);

        SaveLoad.SaveInt(Globals.saveGameDir, saveName, "Current weapon", (int)weaponType);
        SaveLoad.SaveInt(Globals.saveGameDir, saveName, "Current weapon", (int)weaponTypeTest);
        //SaveLoad.SaveInt(Globals.saveGameDir, saveName, "Weapon upgrade", (int)weaponType);

        SaveLoad.SaveFloat(Globals.saveGameDir, saveName, "Health", csHealth.currentHealth);

        SaveLoad.SaveInt(Globals.saveGameDir, saveName, "Items num", itemsList.Count);

        // Items
        //for (int i = 0; i < itemsListString.Count; i++)
        //{
        //    SaveLoad.SaveString(Globals.saveGameDir, saveName, "Item " + i.ToString(), itemsListString[i]);
        //}

        for (int i = 0; i < itemsList.Count; i++)
        {
            SaveLoad.SaveString(Globals.saveGameDir, saveName, "Item " + i.ToString(), itemsList[i].name);
            SaveLoad.SaveBool(Globals.saveGameDir, saveName, "Item " + i.ToString() + ".Equipped", itemsList[i].isEquipped);
        }
    }

    public void LoadPlayer()
    {
        lastUnlockedLvl = SaveLoad.LoadInt(Globals.saveGameDir, saveName, "Last unlocked Lvl");

        //weaponType = (WEAPON)SaveLoad.LoadInt(Globals.saveGameDir, saveName, "Current weapon");
        //weaponTypeTest = (WEAPON_TYPE)SaveLoad.LoadInt(Globals.saveGameDir, saveName, "Current weapon");
        //weaponType = (WEAPON)SaveLoad.LoadInt(Globals.saveGameDir, saveName, "Weapon upgrade");

        SaveLoad.LoadFloat(Globals.saveGameDir, saveName, "Health");

        LoadItems();
    }

    public void LoadItems()
    {
        for (int i = 0; i < SaveLoad.LoadInt(Globals.saveGameDir, saveName, "Items num"); i++)
        {
            //string item = SaveLoad.LoadString(Globals.saveGameDir, saveName, "Item " + i.ToString());
            //itemsListString.Add(item);

            Debug.Log("jiji " + SaveLoad.LoadInt(Globals.saveGameDir, saveName, "Items num").ToString());

            string name = SaveLoad.LoadString(Globals.saveGameDir, saveName, "Item " + i.ToString());

            Item item = Globals.SearchItemInDictionary(name);
            item.isEquipped = SaveLoad.LoadBool(Globals.saveGameDir, saveName, "Item " + i.ToString() + " Equipped");
            item.LogStats();
            itemsList.Add(item);

            item.LogStats();
        }
    }

    #endregion

    #region AUDIO
    public void SetCombatAudioState()
    {
        Audio.SetState("CombatState", "Fight");
    }

    public void SetExplorationAudioState()
    {
        Audio.SetState("CombatState", "Exploration");
    }
    public void SetAliveAudioState()
    {
        Audio.SetState("PlayerState", "Alive");
    }

    public void SetDeadAudioState()
    {
        Audio.SetState("PlayerState", "Dead");
    }

    #endregion
}