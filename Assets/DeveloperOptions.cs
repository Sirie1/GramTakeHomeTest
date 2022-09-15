using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DeveloperOptions : MonoBehaviour
{
    [SerializeField] GameObject configScreen;
    [SerializeField] Slider densitySlider;
    [SerializeField] TextMeshProUGUI densityText;
    [SerializeField] Slider ing2Slider;
    [SerializeField] TextMeshProUGUI ing2Text;
    [SerializeField] Slider ing3Slider;
    [SerializeField] TextMeshProUGUI ing3Text;
    [SerializeField] Slider ing4Slider;
    [SerializeField] TextMeshProUGUI ing4Text;
    private int[] configData;

   

    // Start is called before the first frame update
    void Start()
    {
        SliderSetup();
    }
    private void Update()
    {
        Updatetext();
    }
    void SliderSetup()
    {
        densitySlider.minValue = 0;
        densitySlider.maxValue = 100;
        densitySlider.wholeNumbers = true;
        densitySlider.value = GameManager.Instance.configFile.itemDensity;
        

        ing2Slider.minValue = 0;
        ing2Slider.maxValue = 6;
        ing2Slider.wholeNumbers = true;
        ing2Slider.value = GameManager.Instance.configFile.recipe2IngRange;

        ing3Slider.minValue = 0;
        ing3Slider.maxValue = 6;
        ing3Slider.wholeNumbers =true;
        ing3Slider.value = GameManager.Instance.configFile.recipe3IngRange;

        ing4Slider.minValue = 0;
        ing4Slider.maxValue = 5;
        ing4Slider.wholeNumbers = true;
        ing4Slider.value = GameManager.Instance.configFile.recipe4IngRange;
    }
    [ContextMenu("Update Text")]
    void Updatetext()
    {
        densityText.text = densitySlider.value.ToString();
        ing2Text.text = ing2Slider.value.ToString();
        ing3Text.text = ing3Slider.value.ToString();
        ing4Text.text = ing4Slider.value.ToString();

    }
    public void OnSaveAndCloseClick()
    {
        configData = new int[4];
        configData[0] = (int) densitySlider.value;
        configData[1] = (int) ing2Slider.value;
        configData[2] = (int)ing3Slider.value;
        configData[3] = (int)ing4Slider.value;

        GameManager.Instance.SetConfigData(configData);
        GameManager.Instance.SaveConfigData();
        configScreen.SetActive(false);
    }
    public void OnDebugEnter()
    {
        configScreen.gameObject.SetActive(true);
        SliderSetup();
    }
    private void OnEnable()
    {
      // SliderSetup();
    }
}
