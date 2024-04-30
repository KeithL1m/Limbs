using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private enum frameLimits
    {
        noLimit = 0,
        fps30 = 30,
        fps60 = 60,
        fps120 = 120
    }

    [SerializeField] private List<Frames> frameDisplay = new List<Frames>();
    private int _selectedLimitDisplay;
    private int _selectedFPS;
    [SerializeField] private TMP_Text framesLabel;

    // Start is called before the first frame update
    void Start()
    {
        _masterVolume.value = _masterVolume.maxValue;
        _musicVolume.value = _musicVolume.maxValue;
        _sfxVolume.value = _sfxVolume.maxValue;
        _announcerVolume.value = _announcerVolume.maxValue;

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

        //Application.targetFrameRate = frameDisplay[_selectedLimit].frameLimits;
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
        _selectedLimitDisplay--;
        if(_selectedLimitDisplay < 0)
        {
            _selectedLimitDisplay = 0;
        }

        FramesLabelChange();
    }

    public void FramesButtonRight()
    {
        _selectedLimitDisplay++;
        if (_selectedLimitDisplay > frameDisplay.Count - 1)
        {
            _selectedLimitDisplay = frameDisplay.Count - 1;
        }

        FramesLabelChange();
    }

    public void ResolutionLabelChange()
    {
        screenLabel.text = screenDisplayTypes[_selectedScreenType].screenTypes;
    }

    public void FramesLabelChange()
    {
        framesLabel.text = frameDisplay[_selectedLimitDisplay].frames;
    }
}

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
