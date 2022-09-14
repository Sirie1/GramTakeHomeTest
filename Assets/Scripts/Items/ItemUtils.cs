using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GramGames.CraftingSystem.DataContainers;
using UnityEngine;

public static class ItemUtils
{
	public static Dictionary<string, NodeContainer> ItemsMap = new Dictionary<string, NodeContainer>();
	public static Dictionary<string, HashSet<NodeData>> RecipeMap = new Dictionary<string, HashSet<NodeData>>();

    public static Dictionary<string, HashSet<NodeData>> RecipeMap2Ing = new Dictionary<string, HashSet<NodeData>>();
    public static Dictionary<string, HashSet<NodeData>> RecipeMap3Ing = new Dictionary<string, HashSet<NodeData>>();
    public static Dictionary<string, HashSet<NodeData>> RecipeMap4Ing = new Dictionary<string, HashSet<NodeData>>();


    public static void InitializeMap()
	{
		var nodes = Resources.LoadAll<NodeContainer>("CraftingObjects");
		foreach (var node in nodes)
		{
			ItemsMap.Add(node.MainNodeData.NodeGUID, node);
			
			if (node.IsRawMaterial())
				continue;

			var ingredients = node.GetRecipe();
			
			if (RecipeMap.ContainsKey(node.MainNodeData.NodeGUID) == false)
			{
				var dt = new HashSet<NodeData>(ingredients.Keys);
				RecipeMap.Add(node.MainNodeData.NodeGUID, dt);
				if (node.NodeLinks.Count==2)
                    RecipeMap2Ing.Add(node.MainNodeData.NodeGUID, dt);
				else if(node.NodeLinks.Count == 3)
                    RecipeMap3Ing.Add(node.MainNodeData.NodeGUID, dt);
                else if (node.NodeLinks.Count == 4)
                    RecipeMap4Ing.Add(node.MainNodeData.NodeGUID, dt);

            }
			else
			{
				Debug.LogError($"Tried to add recipe for '{node.MainNodeData.Sprite.name}' more than once!");
			}
				
			// foreach (var recipe in node.GetRecipe())
			// {
			// 	string recipId = recipe.Key.NodeGUID;
			// 	if (RecipeMap.ContainsKey(recipId) == false)
			// 	{
			// 		var dt = new HashSet<NodeData>();
			// 		dt.Add(recipe.Key);
			// 		RecipeMap.Add(recipId, dt);
			// 	}
			// 	else
			// 	{
			// 		RecipeMap[recipId].Add(recipe.Key);
			// 	}
			// }
			//
		}

		Debug.Log($"recipes: {RecipeMap.Count}");
        Debug.Log($"recipes of 2 ing: {RecipeMap2Ing.Count}");
        Debug.Log($"recipes of 3 ing: {RecipeMap3Ing.Count}");
        Debug.Log($"recipes of 4 ing: {RecipeMap4Ing.Count}");
    }

	public static NodeContainer FindBestRecipe(GridCell cell, NodeContainer droppedItem)
	{
		string droppedNodeId = droppedItem.MainNodeData.NodeGUID;

		if (RecipeMap.ContainsKey(droppedNodeId))
		{
			var possibilities = RecipeMap[droppedNodeId];
			Debug.Log($"possibles recipes: {possibilities.Count}");
		}

		return null;
	}

	//return one recipe that have all its ingredient in a chain of cells

	public static NodeContainer FindBestRecipe(NodeContainer[] items)
	{
		

		foreach (var entries in RecipeMap)
		{
			if (items.Length != entries.Value.Count)
				continue;
			
			bool hasAllIngredient = true;
			foreach (var ingredient in entries.Value)
			{
				NodeContainer ingr = ItemsMap[ingredient.NodeGUID];
				if (items.Contains(ingr) == false)
				{
					hasAllIngredient = false;
					break;
				}
			}
			
			if (hasAllIngredient)
				return ItemsMap[entries.Key];
		}

		return null;
	}
}
