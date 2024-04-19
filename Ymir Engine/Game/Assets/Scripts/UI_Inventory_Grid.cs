using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using YmirEngine;


public class UI_Inventory_Grid : YmirComponent
{
    public int rows;
    public int cols;
    private bool _canTab;

    private GameObject leftGrid;
    private GameObject rightGrid;   
    private GameObject downGrid;
    private GameObject upGrid;
    public string leftGridName = " ";
    public string rightGridName = " ";
    public string downGridName = " ";
    public string upGridName = " ";
    public bool naviagteX = false;
    public bool naviagteY = false;
    public bool bounceX = false;
    public bool bounceY = false;

    private float _time;
    private float _timer;

    public void Start()
    {
        leftGrid = InternalCalls.GetGameObjectByName(leftGridName);
        rightGrid = InternalCalls.GetGameObjectByName(rightGridName);        
        downGrid = InternalCalls.GetGameObjectByName(downGridName);
        upGrid = InternalCalls.GetGameObjectByName(upGridName);
        _timer = 0.0f;
        _time = 0.3f;
    }

    public void Update()
    {
        if (!_canTab)
        {
            if (_time > _timer)
            {
                _timer += Time.deltaTime;
            }

            else
            {
                _timer = 0.0f;
                _canTab = true;
                UI.SetCanNav(true);
            }
        }

        if (Input.GetLeftAxisX() > 0 && _canTab)
        {
            _canTab = false;
            UI.NavigateGridHorizontal(gameObject, rows, cols, true, naviagteX, leftGrid, rightGrid, bounceX);
        }

        else if (Input.GetLeftAxisX() < 0 && _canTab)
        {
            _canTab = false;
            UI.NavigateGridHorizontal(gameObject, rows, cols, false, naviagteX, leftGrid, rightGrid, bounceX);
        }
        
        else if (Input.GetLeftAxisY() > 0 && _canTab)
        {
            _canTab = false;
            UI.NavigateGridVertical(gameObject, rows, cols, true, naviagteY, downGrid, upGrid, bounceY);
        }

        else if (Input.GetLeftAxisY() < 0 && _canTab)
        {
            _canTab = false;
            UI.NavigateGridVertical(gameObject, rows, cols, false, naviagteY, downGrid, upGrid, bounceY);
        }

        return;
    }
}
