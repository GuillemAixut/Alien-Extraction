using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Caius : YmirComponent
{
    public struct Dialogue
    {
        public int ID;
        public string Name;
        public string Text;
        public string Code;
    }
    Dictionary<int, Dialogue> dialogue = new Dictionary<int, Dialogue>();
    string juan;

    public GameObject line1_gameObject = null;

    public void Start()
    {
        line1_gameObject = InternalCalls.GetGameObjectByName("Text");
        juan = InternalCalls.CSVToString("Assets/Dialogue/asddasdf.csv");
        LoadDialogues(juan);

    }
	public void Update()
	{
        
        if(Input.GetKey(YmirKeyCode.L) == KeyState.KEY_DOWN)
        {
            line1_gameObject.SetActive(true);

            UI.TextEdit(line1_gameObject, dialogue[1].Name + ":" + dialogue[1].Text);
        }
        if (Input.GetKey(YmirKeyCode.H) == KeyState.KEY_DOWN)
        {
            UI.TextEdit(line1_gameObject, dialogue[2].Name+":" + dialogue[2].Text);
        }
        if (Input.GetKey(YmirKeyCode.Z) == KeyState.KEY_DOWN)
        {
            line1_gameObject.SetActive(false);
        }



        return;
	}
    public void LoadDialogues(string dialogueData)
    {
        string[] lines = dialogueData.Split(new string[] { "<end>" }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] dialogueParts = line.Split(';');

            if (dialogueParts.Length >= 3)
            {
                Dialogue _dialogue = new Dialogue();
                Debug.Log("[WARNING] 1");
                _dialogue.ID = int.Parse(dialogueParts[0]);
                Debug.Log("[WARNING] 1");
                _dialogue.Name = dialogueParts[1];
                Debug.Log("[WARNING] 4");
                _dialogue.Text = dialogueParts[2];
                Debug.Log("[WARNING] 5" +_dialogue.Text);
                _dialogue.Code = dialogueParts[3];
                Debug.Log("[WARNING] 6");

                dialogue.Add(_dialogue.ID, _dialogue);
                Debug.Log("[WARNING] Ended");
            }
        }

        Debug.Log("[WARNING] GG Loading dialogue data" + lines[0]);
    }
}