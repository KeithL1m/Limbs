using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ScreenType
{
    public string screenTypes;
}

[System.Serializable]
public class Frames
{
    public string frames;
}

public class OptionsScreen : MonoBehaviour
{
    // Sliders
    [SerializeField] private Slider _masterVolume;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Slider _sfxVolume;
    [SerializeField] private Slider _announcerVolume;
    // Screen types
    [SerializeField] private List<ScreenType> screenDisplayTypes = new List<ScreenType>();
    [SerializeField] private TMP_Text screenLabel;
    private int _selectedScreenType;

    // Toggles
    [SerializeField] private Toggle _vsyncToggle;
    [SerializeField] private Toggle _particleToggle;
    [SerializeField] private Toggle _screenshakeToggle;

    //frames
    public enum frameLimits
    {
        fps120 = 120,
        fps30 = 30,
        fps60 = 60,
        noLimit = 0
    }

    [SerializeField] private List<Frames> frameDisplay = new List<Frames>();
    private int _selectedFPS;
    [SerializeField] private TMP_Text framesLabel;
    private frameLimits _limits;

    // Start is called before the first frame update
    void Start()
    {
        _masterVolume.value = _masterVolume.maxValue;
        _musicVolume.value = _musicVolume.maxValue;
        _sfxVolume.value = _sfxVolume.maxValue;
        _announcerVolume.value = _announcerVolume.maxValue;

        //Application.targetFrameRate = (int)limits;

        if (QualitySettings.vSyncCount == 0)
        {
            _vsyncToggle.isOn = false;
        }
        else
        {
            _vsyncToggle.isOn = true;
        }
    }


    public void UpdateGraphics()
    {
        if(_selectedScreenType == 0)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.MaximizedWindow);
        }
        else if(_selectedScreenType == 1)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed);
        }
        else if(_selectedScreenType == 2)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }

        if (_vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        if(_selectedFPS == 0)
        {
            _limits = frameLimits.fps30;
        }
        else if(_selectedFPS == 1)
        {
            _limits = frameLimits.fps60;
        }
        else if (_selectedFPS == 2)
        {
            _limits = frameLimits.fps120;
        }
        else if (_selectedFPS == 3)
        {
            _limits = frameLimits.noLimit;
        }

        Application.targetFrameRate = (int)_limits;
    }

    public void ResButtonLeft()
    {
        _selectedScreenType--;
        if(_selectedScreenType < 0)
        {
            _selectedScreenType = 0;
        }

        ResolutionLabelChange();
    }

    public void ResButtonRight()
    {
        _selectedScreenType++;
        if(_selectedScreenType > screenDisplayTypes.Count  - 1)
        {
            _selectedScreenType = screenDisplayTypes.Count - 1;
        }

        ResolutionLabelChange();
    }

    public void FramesButtonLeft()
    {
        _selectedFPS--;
        if(_selectedFPS < 0)
        {
            _selectedFPS = 0;
        }

        FramesLabelChange();
    }

    public void FramesButtonRight()
    {
        _selectedFPS++;
        if (_selectedFPS > frameDisplay.Count - 1)
        {
            _selectedFPS = frameDisplay.Count - 1;
        }

        FramesLabelChange();
    }

    public void ResolutionLabelChange()
    {
        screenLabel.text = screenDisplayTypes[_selectedScreenType].screenTypes;
    }

    public void FramesLabelChange()
    {
        framesLabel.text = frameDisplay[_selectedFPS].frames;
    }
}

