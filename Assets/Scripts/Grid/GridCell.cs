using System.Collections.Generic;
using System.Data;
using GramGames.CraftingSystem.DataContainers;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private GridCell _left;
    [SerializeField] private GridCell _right;
    [SerializeField] private GridCell _up;
    [SerializeField] private GridCell _down;
    [SerializeField] private GridHandler _handler;
    
    private MergableItem _item;
    public MergableItem Item => _item;

    //Item Density should be a value 0-100 representing the probability of item appear
    [SerializeField] private int itemDensity;
    [SerializeField] private int recipeRange;

    public void SpawnItem(NodeContainer item)
    {
	    _handler.ClearCell(this);
    //    itemDensity = GameManager.Instance.configFile.itemDensity;
    //    if (Random.Range(0, 100) <= itemDensity)
   //     {
            var obj = Instantiate(GameManager.Instance.DraggableObjectPrefab);
            obj.Configure(item, this);
   //     }

    }
    
    public void SetHandler(GridHandler handler)
    {
        _handler = handler;
    }

    public void SetItemAssigned(MergableItem item)
    {
	    if (_handler == null)
		    _handler = GetComponentInParent<GridHandler>();
       
        if (_item != null)
            _handler.SetCellState(_item.GetCurrentCell(), true);
        
        _item = item;
        _handler.SetCellState(this, _item != null);
    }

    public void SetNeighbor(GridCell neighbor, MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Left:
                _left = neighbor;
                break;
            case MoveDirection.Right:
                _right = neighbor;
                break;
            case MoveDirection.Up:
                _up = neighbor;
                break;
            case MoveDirection.Down:
                _down = neighbor;
                break;
      
        }
    }


    public bool HasItem()
    {
        return _item != null;
    }

    public void ClearItem()
    {
        if (_item != null)
        {
            Destroy(_item.gameObject);
            _item = null;
        }
    }
}

public enum MoveDirection
{
    Left, Right, Up, Down
}
