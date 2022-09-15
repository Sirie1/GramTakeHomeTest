using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private string gameDataSaveDirectory = "/GameData/";
    private string gameDataFilename = "ConfigFile.sav";
    public ConfigFile configFile;

    private string gameSettingsFilename = "Settings.sav";
    public SettingsFile settingsFile;
    [SerializeField] SettingsMenu settingsMenu;

    public MergableItem DraggableObjectPrefab;
    public GridHandler MainGrid;
    [SerializeField] private List<string> ActiveRecipes = new List<string>();


    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }

            return _instance;
        }
    }
	private void Awake()
	{

     //   DontDestroyOnLoad(this);
       if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Screen.fullScreen =
			false; // https://issuetracker.unity3d.com/issues/game-is-not-built-in-windowed-mode-when-changing-the-build-settings-from-exclusive-fullscreen

		// load all item definitions
		//ItemUtils.InitializeMap();
       
	}

	private void Start()
	{



        ReloadLevel(1);
        LoadSettingFile();
        LoadConfigData();
        Debug.Log ($"density:{configFile.itemDensity}");
        Debug.Log($"2:{configFile.recipe2IngRange}");
        Debug.Log($"3:{configFile.recipe3IngRange}");
        Debug.Log($"4:{configFile.recipe4IngRange}");
    }

    #region Settings Manage

    [Serializable]
    public class SettingsFile
    {
        [HideInInspector]
        public float volumeSetting;
    }

    private void LoadSettingFile()
    {
        string dir = Application.persistentDataPath + gameDataSaveDirectory;
        if (!Directory.Exists(dir))
        {
            Debug.LogWarning($"Directory {dir} was not found");
            return;
        }
        if (!File.Exists(dir + gameSettingsFilename))
        {
            Debug.LogWarning($"File {gameSettingsFilename} was not found");
            return;
        }
        string json = File.ReadAllText(dir + gameSettingsFilename);
        settingsFile = JsonUtility.FromJson<SettingsFile>(json);

    }

    public void SaveSettingData()
    {
        string dir = Application.persistentDataPath + gameDataSaveDirectory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            Debug.Log($"Directory {dir} was created");
        }
        string json = JsonUtility.ToJson(settingsFile, true);
        File.WriteAllText(dir + gameSettingsFilename, json);
    }

    public void OnSettingEnter()
    {
        settingsMenu.gameObject.SetActive(true);

    }

    public void OnSettingClose()
    {
        settingsMenu.gameObject.SetActive(false);
    }

    public void SetSettingsFile(float value)
    {
        settingsFile.volumeSetting = value;
    }

    public float GetSettingsFile()
    {
        return settingsFile.volumeSetting;
    }

    #endregion

    #region Config File

    [Serializable]
	public class ConfigFile
	{
        [Range(1, 100)]
        public int itemDensity;

        [Range(0, 6)]
        public int recipe2IngRange;

        [Range(0, 6)]
        public int recipe3IngRange;

        [Range(0, 5)]
        public int recipe4IngRange;
    }

    public void SetConfigData(int[] newData)
    {
        configFile.itemDensity = newData[0];
        configFile.recipe2IngRange = newData[1];
        configFile.recipe3IngRange = newData[2];
        configFile.recipe4IngRange = newData[3];
    }

    [ContextMenu ("LoadConfigFile")]
    public void LoadConfigData()
	{
        string dir = Application.persistentDataPath + gameDataSaveDirectory;
        if (!Directory.Exists(dir))
        {
            Debug.LogWarning($"Directory {dir} was not found");
        }
        if (File.Exists(dir+gameDataFilename))
        {
            string json = File.ReadAllText(dir + gameDataFilename);
            configFile = JsonUtility.FromJson<ConfigFile>(json);
            Debug.Log ($"ConfigFile {dir + gameDataFilename} loaded");
        }
        else
            return;

    }
    [ContextMenu("SaveConfigFile")]
    public void SaveConfigData()
    {
        string dir = Application.persistentDataPath + gameDataSaveDirectory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            Debug.Log($"Directory {dir} was created");
        }
        string json = JsonUtility.ToJson(configFile, true);
        File.WriteAllText(dir + gameDataFilename, json);
        Debug.Log($"GameData saved in {dir}");
    }
    #endregion
    
    public void GoToMainScene()
    {
        ScenePersist.Instance.ToMainMenuScene();
    }
    public void ReloadScene()
    {
        ScenePersist.Instance.ReloadScene();
    }
    public void ReloadLevel(int difficulty = 1)
	{

        ItemUtils.InitializeMap();
        // clear the board
        var fullCells = MainGrid.GetFullCells.ToArray();
		for (int i = fullCells.Length - 1; i >= 0; i--)
			MainGrid.ClearCell(fullCells[i]);

		// choose new recipes
		ActiveRecipes.Clear();
        // Debug.Log ($"Clearing Active recipes to{ActiveRecipes.Count}");
        //difficulty = Mathf.Max(difficulty, 1);

        LoadConfigData();
        if (configFile.recipe2IngRange == 0 && configFile.recipe3IngRange == 0 && configFile.recipe4IngRange == 0)
            Debug.LogError("Designer must set at least one recipe");
        Debug.Log($"2range: {configFile.recipe2IngRange} 3range: {configFile.recipe3IngRange} 4range:{configFile.recipe4IngRange}");
        if (configFile.recipe2IngRange != 0)
            for (int i=0; i< configFile.recipe2IngRange ;i++)
            {
                var recipe = ItemUtils.RecipeMap2Ing.ElementAt(i).Key;
                ActiveRecipes.Add(recipe);

            }
        if(configFile.recipe3IngRange != 0)
            for (int i = 0; i < configFile.recipe3IngRange; i++)
            {
                var recipe = ItemUtils.RecipeMap3Ing.ElementAt(i).Key;
                ActiveRecipes.Add(recipe);
            }
        if (configFile.recipe4IngRange != 0)
            for (int i = 0; i < configFile.recipe4IngRange; i++)
            {
                var recipe = ItemUtils.RecipeMap4Ing.ElementAt(i).Key;
                ActiveRecipes.Add(recipe);
            }
        // populate the board
        var emptyCells = MainGrid.GetEmptyCells.ToArray();
		foreach (var cell in emptyCells)
		{
            
            if (Random.Range(0, 100)< configFile.itemDensity)
            {
                var chosenRecipe = ActiveRecipes[Random.Range(0, ActiveRecipes.Count)];
                var ingredients = ItemUtils.RecipeMap[chosenRecipe].ToArray();			
                var ingredient = ingredients[Random.Range(0,ingredients.Count())];
                var item = ItemUtils.ItemsMap[ingredient.NodeGUID];
			    cell.SpawnItem(item);
            }
        }
	}
}
