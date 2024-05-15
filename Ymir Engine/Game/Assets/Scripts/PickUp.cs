using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class PickUp : YmirComponent
{
    private bool picked = false;
    private Player player = null;

    public void Start()
    {
        picked = false;
    }

    public void Update()
    {

    }

    public void OnCollisionEnter(GameObject other)
    {
        if (other.Tag == "Player" && !picked)
        {
            player = other.GetComponent<Player>();

            if (gameObject.Tag == "Resin")
            {
                if (player.currentResinVessels < player.maxResinVessels)
                {
                    gameObject.SetActive(false);
                    Debug.Log("Pick up " + gameObject.Name);
                    player.currentResinVessels++;

                    if (player.resinText != null)
                    {
                        UI.TextEdit(player.resinText, "x" + player.currentResinVessels.ToString());
                    }

                    picked = true;
                    InternalCalls.Destroy(gameObject);
                }
            }
            else
            {
                int nonEquipped = 0;

                // TODO:
                for (int i = 0; i < player.itemsList.Count; i++)
                {
                    if (!player.itemsList[i].isEquipped)
                    {
                        nonEquipped++;
                    }
                }

                // 15 --> inventory full
                if (nonEquipped < 14)
                {
                    Audio.PlayEmbedAudio(gameObject);

                    //TODO: Hacer que el item se destruya/elimine
                    gameObject.SetActive(false);
                    Debug.Log("Pick up " + gameObject.Name);
                    player.itemsList.Add(Globals.SearchItemInDictionary(gameObject.Name));

                    //player.itemsListString.Add(gameObject.Name);

                    InternalCalls.Destroy(gameObject);
                    picked = true;
                }
                else
                {
                    // TODO: Feedback inventory full
                }
            }
        }
    }
}