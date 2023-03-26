using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PuzzleFloorPila : PuzzleFloorPilaInterface
{
    public FloorPuzzleController[] _floorOrder;
    public int _index = 0;
    private int _indexCompleteFloors;
    
    public void Initialization(int AmountPila)
    {
        _floorOrder = new FloorPuzzleController[AmountPila];
    }

    public void StackFloor(FloorPuzzleController floor)
    {
        _floorOrder[_index] = floor;
        //_floorOrder[_index].LinkNearbyFlats();
        _indexCompleteFloors++;
    }

    public void sumoIndex()
    {
        _floorOrder[_index]._FloorPuzzleModel.HasUsedFloor(true);
        _index++;
    }

    public void SetNextFloor()
    {
        _floorOrder[_index - 1]._nextFloorInOrder = _floorOrder[_index];
    }

    public void UnstackBeforeFloor()
    {
        _index--;
    }
    public void CompleteFloor()
    {
        _indexCompleteFloors--;
    }
    public bool StackEmpty()
    {
        return _indexCompleteFloors == 0;
    }

    public int index()
    {
        return _index;
    }

}
