using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;
using static Caius;

public class Caius : YmirComponent
{
    public struct Dialogue
    {
        public int ID;
        public string Type;
        public string Name;
        public string Text;
        public int Code;
    }

    private List<Dialogue> dialogueList = null;

    public GameObject name_gameObject;
	public GameObject line1_gameObject;
	public GameObject line2_gameObject;
	public GameObject line3_gameObject;
	public GameObject ui_gameObject;

	//private bool talked = false;
	private bool dialogue_ui = false;

	private Player csPlayer;

    private string str = "ID;Type;Name;Text;Code;Endmark<end>1;Text;Caius;Oh, good, I see they've woken you up. Just in time;;<end>;Answer;Raisen;In time for what?;goTo:2;<end>;Answer;Raisen;Woken me up? What do you mean?;goTo:2;<end>;Answer;Raisen;Up and ready for the mission.;goTo:2;<end>;Answer;Raisen;;;<end>2;Text;Caius;We've just arrived at Gliese 667. You'll have to clean up the planet. I don't have much information about the mission, it's labeled Confidential level: EXTREME, I'm not allowed to access beyond the summary annex. I hope you have been briefed on better Raisen.;;<end>;Answer;Raisen;Clear your hopes Caius.;goTo:;<end>;Answer;Raisen;I wish they did.;goTo:;<end>;Answer;Raisen;;;<end>;Answer;Raisen;;;<end>";

    public void Start()
	{
        GameObject gameObject = InternalCalls.GetGameObjectByName("Player");
        if (gameObject != null)
        {
            csPlayer = gameObject.GetComponent<Player>();
        }

        LoadDialogues(str);
    }

	public void Update()
	{
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
        if (/*other.Tag == "Player" &&*/ (Input.IsGamepadButtonAPressedCS() || Input.GetKey(YmirKeyCode.SPACE) == KeyState.KEY_DOWN))
        {
            //TODO: Lógica del diálogo
            dialogue_ui = true;

            DisplayDialogueByID(1);

            Debug.Log("[WARNING] Mostrar UI del dialogo");
        }
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
                dialogue.ID = int.Parse(dialogueParts[0]);
                dialogue.Type = dialogueParts[1];
                dialogue.Name = dialogueParts[2];
                dialogue.Text = dialogueParts[3];
                dialogue.Code = int.Parse(dialogueParts[4]);

                dialogueList.Add(dialogue);

            }
            else
            {
                Debug.Log("[ERROR] Invalid dialogue data.");
            }
        }
    }

    public void DisplayDialogueByID(int id)
    {
        UI.TextEdit(name_gameObject, GetDialogueByID(id).Name);
        UI.TextEdit(line1_gameObject, GetDialogueByID(id).Text);

        Debug.Log("[WARNING] Se ha cargado correctamente el Dialogo con id: " + id);
    }

    public Dialogue GetDialogueByID(int id)
    {
        foreach (Dialogue dialogue in dialogueList)
        {
            if (dialogue.ID == id)
            {
                return dialogue;
            }
        }

        return default(Dialogue);
    }
}