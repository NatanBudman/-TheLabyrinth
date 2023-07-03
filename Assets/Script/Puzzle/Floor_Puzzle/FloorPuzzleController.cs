using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class FloorPuzzleController : MonoBehaviour
{
    public FloorPuzzleModel _FloorPuzzleModel;
    
    public int row;

    public FloorPuzzleController[] nearbyFlats;

    private int[] angles = {0,40,90,135,180,-40,-90,-135,-180};

    public FloorPuzzlerManager _puzzlerManager;

    private int nearbyFlastCount = 0;

    
    public FloorPuzzleController _nextFloorInOrder;
    private void Awake()
    {
        _FloorPuzzleModel = GetComponent<FloorPuzzleModel>();
        LinkNearbyFlats();

    }
    
    public void LinkNearbyFlats()
    {
        nearbyFlats = new FloorPuzzleController[8];

        for (int i = 0; i < nearbyFlats.Length; i++)
        {
            nearbyFlats[i] = _FloorPuzzleModel.GetNearbyFlats(2,angles[i]);

            if (nearbyFlats[i] != null)
            {
                nearbyFlastCount++;
            }
        }
        
        // rearmo el array para que contenga solo los pisos cercanos y no haya nulls
        FloorPuzzleController[] p = nearbyFlats;
        nearbyFlats = new FloorPuzzleController[nearbyFlastCount];
        nearbyFlastCount = 0;
        for (int i = 0; i < p.Length; i++)
        {
            if (p[i] != null)
            {
                nearbyFlats[nearbyFlastCount] = p[i];
                nearbyFlastCount++;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!_FloorPuzzleModel.isFloorInPila() && !_FloorPuzzleModel.IsCorrectFloor())
            {
              //  Debug.Log("Perdistes");
                _puzzlerManager.isResetPuzzle = true;
                _FloorPuzzleModel.ChangeMat(_puzzlerManager._FaildColor);

            }

            if (_FloorPuzzleModel.IsCorrectFloor())
            {
                _FloorPuzzleModel.IsCorrectNextFloor(_nextFloorInOrder);
                _FloorPuzzleModel.ChangeMat(_puzzlerManager._GoodCoolor);
            }
        }
        
    }
}
