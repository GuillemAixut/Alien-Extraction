using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Caius : YmirComponent
{
    public struct Dialogue
    {
        public string ID;
        public string Type;
        public string Name;
        public string Text;
        public string Code;
    }

    private List<Dialogue> dialogueList = null;

    public GameObject name_gameObject = null;
	public GameObject line1_gameObject = null;
	public GameObject line2_gameObject = null;
	public GameObject line3_gameObject = null;
	public GameObject ui_gameObject = null;

	//private bool talked = false;
	private bool dialogue_ui = false;

	//private Player csPlayer;

    private readonly string str = "ID;Type;Name;Text;Code;Endmark<end>1;Text;Caius;Oh, good, I see they've woken you up. Just in time;;<end>;Answer;Raisen;In time for what?;goTo:2;<end>;Answer;Raisen;Woken me up? What do you mean?;goTo:2;<end>;Answer;Raisen;Up and ready for the mission.;goTo:2;<end>;Answer;Raisen;;;<end>2;Text;Caius;We've just arrived at Gliese 667. You'll have to clean up the planet. I don't have much information about the mission, it's labeled Confidential level: EXTREME, I'm not allowed to access beyond the summary annex. I hope you have been briefed on better Raisen.;;<end>;Answer;Raisen;Clear your hopes Caius.;goTo:;<end>;Answer;Raisen;I wish they did.;goTo:;<end>;Answer;Raisen;;;<end>;Answer;Raisen;;;<end>";

    public void Start()
	{
        //GameObject gameObject = InternalCalls.GetGameObjectByName("Player");
        //if (gameObject != null)
        //{
        //    csPlayer = gameObject.GetComponent<Player>();
        //}

        //LoadDialogues(str);
        Debug.Log("START Caius.cs");
        name_gameObject = InternalCalls.GetGameObjectByName("Name");
        line1_gameObject = InternalCalls.GetGameObjectByName("Text_Line1");
        line2_gameObject = InternalCalls.GetGameObjectByName("Text_Line2");
        line3_gameObject = InternalCalls.GetGameObjectByName("Text_Line3");
        ui_gameObject = InternalCalls.GetGameObjectByName("Canvas");

        if (name_gameObject != null && line1_gameObject != null && line2_gameObject != null && line3_gameObject != null && ui_gameObject != null)
        {
            Debug.Log("UI GameObjects Loaded corectly");
        }
        else
        {
            Debug.Log("[ERROR] Error Loading UI GameObjects");
        }

        //Load Dialogue Data
        try
        {
            LoadDialogues(str);
        }
        catch (Exception e)
        {
            Debug.Log("[ERROR] Error while Loading dialogue str: " + e);
        }

    }

	public void Update()
	{
        if (Input.IsGamepadButtonAPressedCS() || Input.GetKey(YmirKeyCode.SPACE) == KeyState.KEY_DOWN)
        {
            //TODO: Lógica del diálogo
            dialogue_ui = true;

            Debug.Log("[WARNING] Mostrar UI del dialogo");

            try
            {
                DisplayDialogueByID("1");
            }
            catch (Exception e)
            {
                Debug.Log("[ERROR] Error while displaying dialogue 1: " + e);
            }
            


        }

        //TODO: Show the dialogue UI when the bool is true
        if (dialogue_ui)
        {
            //UI.TextEdit(name_gameObject, "Lorem ipsum");

            ui_gameObject.SetActive(true);


            //TODO: Set de dialogue_ui = false; y csPlayer.inputsList.Add(Player.INPUT.I_IDLE); cuando acabe el dialogo

        }
    }

   // public void OnCollisionStay(GameObject other)
   // {
   //     if (other.Tag == "Player" && talked == false && Input.GetKey(YmirKeyCode.SPACE) == KeyState.KEY_DOWN)
   //     {
			////TODO: Descomentar Don't let the player move
			////csPlayer.inputsList.Add(Player.INPUT.I_STOP);

			////Show the dialogue UI
			//dialogue_ui = true;

			////Don't let repeat the dialogue
			//talked = true;
   //     }
   // }

    public void OnCollisionStay(GameObject other)
    {
        //TODO: Mostrat UI de que puede interactuar si pulsa el botón asignado
        //if (other.Tag == "Player" && (Input.IsGamepadButtonAPressedCS() || Input.GetKey(YmirKeyCode.SPACE) == KeyState.KEY_DOWN))
        //{

        //    //TODO: Lógica del diálogo
        //    dialogue_ui = true;

        //    DisplayDialogueByID("1");

        //    Debug.Log("[WARNING] Mostrar UI del dialogo");
        //}
    }

    public void LoadDialogues(string dialogueData)
    {
        string[] lines = dialogueData.Split(new string[] { "<end>" }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] dialogueParts = line.Split(';');

            if (dialogueParts.Length >= 5)
            {
                Dialogue dialogue = new Dialogue();
                Debug.Log("[WARNING] 1");
                dialogue.ID = dialogueParts[0];
                Debug.Log("[WARNING] 2");
                dialogue.Type = dialogueParts[1];
                Debug.Log("[WARNING] 3");
                dialogue.Name = dialogueParts[2];
                Debug.Log("[WARNING] 4");
                dialogue.Text = dialogueParts[3];
                Debug.Log("[WARNING] 5");
                dialogue.Code = dialogueParts[4];
                Debug.Log("[WARNING] 6");

                dialogueList.Add(dialogue);
                Debug.Log("[WARNING] Ended");
            }
            else
            {
                Debug.Log("[ERROR] Invalid dialogue data.");
            }
        }

        Debug.Log("[WARNING] GG Loading dialogue data");
    }

    public void DisplayDialogueByID(string id)
    {
        if (line1_gameObject != null && name_gameObject != null)
        {
            UI.TextEdit(name_gameObject, GetDialogueByID(id).Name);
            UI.TextEdit(line1_gameObject, GetDialogueByID(id).Text);
        }

        Debug.Log("[WARNING] Se ha mostrado correctamente el Dialogo con id: " + id);
    }

    public Dialogue GetDialogueByID(string id)
    {
        try
        {
            foreach (Dialogue dialogue in dialogueList)
            {
                if (dialogue.ID == id)
                {
                    return dialogue;
                }
            }
        }
        catch { }


        return default(Dialogue);
    }
}