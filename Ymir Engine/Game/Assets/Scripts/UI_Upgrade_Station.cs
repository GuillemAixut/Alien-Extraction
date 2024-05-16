using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using YmirEngine;

public class UI_Upgrade_Station : YmirComponent
{
    private GameObject _focusedGO;
    public int currentScore;// Put in player

    public GameObject description, cost, coins;
    private Player _player = null;

    public void Start()
    {
        _focusedGO = UI.GetFocused();
        description = InternalCalls.GetChildrenByName(gameObject, "Description");
        cost = InternalCalls.GetChildrenByName(gameObject, "Cost");        
        coins = InternalCalls.GetChildrenByName(gameObject, "Coins");

        UI.TextEdit(coins, currentScore.ToString());
        GetPlayerScript();
    }

    public void Update()
    {
        if (_player == null)
        {
            GetPlayerScript();
        }

        return;
    }

    public void UpdateCoins()
    {
        UI.TextEdit(coins, currentScore.ToString());
    }

    private void GetPlayerScript()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Player");

        if (gameObject != null)
        {
            _player = gameObject.GetComponent<Player>();
        }
    }
}