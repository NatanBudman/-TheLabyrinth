using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PuzzleFloorPilaInterface
{
    public void Initialization(int AmountPila);
    
    public void StackFloor(FloorPuzzleController floor);
    
    public void UnstackBeforeFloor();

    public void CompleteFloor();
    
    public bool StackEmpty();
    
}
