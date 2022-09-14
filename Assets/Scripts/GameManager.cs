using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private string gameDataSaveDirectory = "/GameData/";
    private string gameDataFilename = "ConfigFile.sav";

    public ConfigFile configFile;
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

    public MergableItem DraggableObjectPrefab;
	public GridHandler MainGrid;

	private List<string> ActiveRecipes = new List<string>();

	private void Awake()
	{
        _instance = this;
        
		Screen.fullScreen =
			false; // https://issuetracker.unity3d.com/issues/game-is-not-built-in-windowed-mode-when-changing-the-build-settings-from-exclusive-fullscreen

		// load all item definitions
		ItemUtils.InitializeMap();
	}

	private void Start()
	{
		ReloadLevel(1);
	}

	#region Config File

	[Serializable]
	public class ConfigFile
	{
        [Range(1, 100)]
        public int itemDensity;
        public int recipeRange;
    }

    [ContextMenu ("LoadConfigFile")]
    public void LoadConfigFile()
	{
        string dir = Application.persistentDataPath + gameDataSaveDirectory;
        if (!Directory.Exists(dir))
        {
            Debug.LogWarning($"Directory {dir} was not found");
        }
        string json = File.ReadAllText(dir + gameDataFilename);
		configFile = JsonUtility.FromJson<ConfigFile>(json);

    }
    [ContextMenu("SaveConfigFile")]
    public void SaveGameData()
    {
        string dir = Application.persistentDataPath + gameDataSaveDirectory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            Debug.Log($"Directory {dir} was created");
        }
        string json = JsonUtility.ToJson(configFile, true);
        File.WriteAllText(dir + gameDataFilename, json);
    }
    #endregion


    public void ReloadLevel(int difficulty = 1)
	{
		// clear the board
		var fullCells = MainGrid.GetFullCells.ToArray();
		for (int i = fullCells.Length - 1; i >= 0; i--)
			MainGrid.ClearCell(fullCells[i]);

		// choose new recipes
		ActiveRecipes.Clear();
		difficulty = Mathf.Max(difficulty, 1);
		for (int i = 0; i < difficulty; i++)
		{
			// a 'recipe' has more than 1 ingredient, else it is just a raw ingredient.
			var recipe = ItemUtils.RecipeMap.FirstOrDefault(kvp => kvp.Value.Count > 1).Key;
			ActiveRecipes.Add(recipe);
		}

		// populate the board
		var emptyCells = MainGrid.GetEmptyCells.ToArray();
		foreach (var cell in emptyCells)
		{
			var chosenRecipe = ActiveRecipes[0];
			var ingredients = ItemUtils.RecipeMap[chosenRecipe].ToArray();
			var ingredient = ingredients[0];
			var item = ItemUtils.ItemsMap[ingredient.NodeGUID];
			cell.SpawnItem(item);
		}
	}
}
