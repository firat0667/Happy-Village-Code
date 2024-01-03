using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerToolSelector : MonoBehaviour
{
    public enum Tool { None, Sow, Water, Harvest};
    private Tool _activeTool;

    [Header("Elements")]
    [SerializeField] private Image[] _toolImages;

    [Header("Settings")]
    [SerializeField] private Color _selectedToolColor;

    [Header("Actions")]
    public Action<Tool> _selectedToolAction;
    // Start is called before the first frame update
    void Start()
    {
        SelectTool(0);
    }

    public void SelectTool(int toolIndex)
    {
        _activeTool = (Tool)toolIndex;
        for (int i = 0; i < _toolImages.Length; i++)
            /*
            if (i == toolIndex)
            _toolImages[i].color = _selectedToolColor;
            else
            {
                _toolImages[i].color = Color.white;
            }
            */
            // usteký satýlarýn gorevýný yapýyor 
            _toolImages[i].color = i == toolIndex ? _selectedToolColor : Color.white;
           _selectedToolAction?.Invoke(_activeTool);
    }
    public bool CanSow()
    {
        return _activeTool == Tool.Sow;
        // üsteki ile alttaki ayný þeyi ifade ediyor

        /*
        if (_activeTool == Tool.Sow)
            return true;
        else
            return false;
        */
    }
    public bool CanWater()
    {
        return _activeTool == Tool.Water;
    }
    public bool CanHarvest()
    {
        return _activeTool == Tool.Harvest;
    }
    public bool CanNone()
    {
        return _activeTool == Tool.None;
    }
}
