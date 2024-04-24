using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsScreen : MonoBehaviour
{
    [SerializeField] private List<ScreenType> screenDisplayTypes = new List<ScreenType>();

    public TMP_Text screenLabel;
    private int _selectedScreenType;
    // Start is called before the first frame update
    void Start()
    {
       
    }


    public void UpdateGraphics()
    {
        if(_selectedScreenType == 0)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else if(_selectedScreenType == 1)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed);
        }
        else if(_selectedScreenType == 2)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
    }

    public void ResButtonLeft()
    {
        _selectedScreenType--;
        if(_selectedScreenType < 0)
        {
            _selectedScreenType = 0;
        }

        UpdateScreen();
    }

    public void ResButtonRight()
    {
        _selectedScreenType++;
        if(_selectedScreenType > screenDisplayTypes.Count  - 1)
        {
            _selectedScreenType = screenDisplayTypes.Count - 1;
        }

        UpdateScreen();
    }

    public void UpdateScreen()
    {
        screenLabel.text = screenDisplayTypes[_selectedScreenType].screenTypes;
    }
}

[System.Serializable]
public class ScreenType
{
    public string screenTypes;
}
