using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider volumeSlider; 
    // Start is called before the first frame update


    public void CloseSettingButton()
    {
        //save
        GameManager.Instance.SetSettingsFile(volumeSlider.value);
        GameManager.Instance.SaveSettingData();
        GameManager.Instance.OnSettingClose();
    }
    private void OnEnable()
    {
        float vol = GameManager.Instance.GetSettingsFile();
        volumeSlider.value = vol;
    }
}
