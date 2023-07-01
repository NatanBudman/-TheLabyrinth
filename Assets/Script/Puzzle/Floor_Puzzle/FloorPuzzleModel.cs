using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPuzzleModel : MonoBehaviour
{
    
    private bool isHasInPila = false;
    public bool isCorreIsFloorGood = false;
    public bool isHasUsed = false;

    private int index = 0;

    private Material _material;

    public void ResetFloor()
    {
        isHasUsed = false;
        isCorreIsFloorGood = false;
        isHasInPila = false;
    }

    public bool isFloorInPila()
    {
        return isHasInPila;
    }
    public void HasUsedFloor(bool isUses)
    {
        isHasUsed = isUses;
    }
    public bool IsCorrectFloor()
    {
        return isCorreIsFloorGood;
    }

    public void IsCorrectNextFloor(FloorPuzzleController next)
    {
        next._FloorPuzzleModel.isCorreIsFloorGood = true;
    }

    public FloorPuzzleController GetNearbyFlats(int maxDist,float angle)
    {
        FloorPuzzleController floor = null;
        
        float angulo = angle;
        Vector3 direction = Quaternion.AngleAxis(angulo, Vector3.up) * transform.forward;
        
        RaycastHit hit;

        if (Physics.Raycast(transform.position,direction,out hit,maxDist,LayerMask.GetMask("Puzzle")))
        {
            hit.collider.gameObject.GetComponent<FloorPuzzleController>();
            floor = hit.collider.gameObject.GetComponent<FloorPuzzleController>();
            return floor;
        }

        return floor;
    }

    public void ChangeMat(Material material)
    {
        this.gameObject.GetComponent<Renderer>().material.color = material.color;
    }
}
