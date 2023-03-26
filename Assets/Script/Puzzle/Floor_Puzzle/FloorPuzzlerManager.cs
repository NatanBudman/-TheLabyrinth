using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloorPuzzlerManager : MonoBehaviour
{
    public PuzzleFloorPila _puzzleFloorPila = new PuzzleFloorPila();

    public GameObject player;
    public GameObject respawn;

    [SerializeField]private FloorPuzzleController[] allFloor;

    public FloorPuzzleController[] _firtRowFloors;
    
    public Material _GoodCoolor;
    public Material _FaildColor;
    public Material _NeutralColor;
    public Material _DefaultColor;


    [SerializeField] private int RowsInPuzzle = 7;
    [SerializeField] private int[] _FloorActivePerRow;
    private int indexFloorActivePerRow = 0;
    private int totalFloorActivate;

    public bool isResetPuzzle = false;

    // Start is called before the first frame update
    private void Awake()
    {
        _FloorActivePerRow = new int[RowsInPuzzle];

        allFloor = new FloorPuzzleController[GameObject.FindObjectsOfType<FloorPuzzleController>().Length];

        allFloor = GameObject.FindObjectsOfType<FloorPuzzleController>();
        

    }

    void Start()
    {
        FloorPerRow();
        
        _puzzleFloorPila.Initialization(totalFloorActivate);
        
        StartCoroutine(StartFloorPuzle());
    }
    
    private void  FloorPerRow()
    {
        totalFloorActivate = 0;
        // Defino cuanto pisos se activan por fila
        for (int i = 0; i < _FloorActivePerRow.Length; i++)
        {
            _FloorActivePerRow[i] = Random.Range(1, 3);
            totalFloorActivate += _FloorActivePerRow[i];
        }

    }

    IEnumerator ResetPuzzle(int TimeToReset)
    {
        _puzzleFloorPila = new PuzzleFloorPila();
        
        player.transform.position = respawn.transform.position;
        
        yield return new WaitForSeconds(TimeToReset);

        indexFloorActivePerRow = 0;

        FloorPerRow();
        
        _puzzleFloorPila.Initialization(totalFloorActivate);
        
        StartCoroutine(StartFloorPuzle());
        
        StopCoroutine(ResetPuzzle(TimeToReset));
    }

    public void RestarPuzzle(int TimeToReset)
    {
        StartCoroutine(ResetPuzzle(TimeToReset));
    }

    IEnumerator StartFloorPuzle()
    {
       
        for (int i = 0; i < totalFloorActivate; i++)
        {
            if (_puzzleFloorPila._floorOrder[0] != null)
            {
                //Regresa a su color original
                _puzzleFloorPila._floorOrder[_puzzleFloorPila.index() - 1]._FloorPuzzleModel.ChangeMat(_DefaultColor);
            }

            if (_puzzleFloorPila._floorOrder[0] == null)
            {
                _puzzleFloorPila.StackFloor(_firtRowFloors[Random.Range(0,7)]);
                _puzzleFloorPila._floorOrder[_puzzleFloorPila.index()]._FloorPuzzleModel.ChangeMat(_NeutralColor);
                _puzzleFloorPila._floorOrder[_puzzleFloorPila.index()]._FloorPuzzleModel.isCorreIsFloorGood = true;
            }
           
            // Aqui abajo es donde empieza la magia uwu
            yield return new WaitForSeconds(3);
            
            int h = 0;
            int nearbyFlatsLenght = _puzzleFloorPila._floorOrder[_puzzleFloorPila.index()].nearbyFlats.Length;
            
            // saco cantidad de pisos a filtrar
            for (int j = 0; j < nearbyFlatsLenght; j++)
            {
                if (_puzzleFloorPila._floorOrder[_puzzleFloorPila.index()].
                        nearbyFlats[j].row == indexFloorActivePerRow && !_puzzleFloorPila._floorOrder[_puzzleFloorPila.index()].
                        nearbyFlats[j]._FloorPuzzleModel.isHasUsed)
                {
                    h++;
                }
            }

            int[] aux = new int[h];
            int _index = 0;
            FloorPuzzleController nearbyFlat = null;
            
            //filtro pisos
            for (int j = 0; j < nearbyFlatsLenght; j++)
            {
                if (_puzzleFloorPila._floorOrder[_puzzleFloorPila.index()].nearbyFlats[j].row ==
                    indexFloorActivePerRow && !_puzzleFloorPila._floorOrder[_puzzleFloorPila.index()].nearbyFlats[j]
                        ._FloorPuzzleModel.isHasUsed)
                {
                    aux[_index] = j;
                    _index++;
                }
            }
            
            nearbyFlat = _puzzleFloorPila._floorOrder[_puzzleFloorPila.index()].nearbyFlats[aux[Random.Range(0,aux.Length)]];
            
                    
            if (nearbyFlat != null)
            {
                if (nearbyFlat.row == indexFloorActivePerRow)
                {
                       
                    _puzzleFloorPila.sumoIndex();
                    _puzzleFloorPila.StackFloor(nearbyFlat);
                    _puzzleFloorPila.SetNextFloor();
                    nearbyFlat._FloorPuzzleModel.ChangeMat(_NeutralColor);
                    _FloorActivePerRow[indexFloorActivePerRow] -= 1;
                    
                    if (_FloorActivePerRow[indexFloorActivePerRow] <= 0)
                    {
                        indexFloorActivePerRow++;
                    }

                }
            }
            // termino de construir la ruta
            if (i >= totalFloorActivate)
            {
                StopCoroutine(StartFloorPuzle());
            }
        }
            
    }

    private void Update()
    {
        if (isResetPuzzle)
        {
            RestarPuzzle(2);

            for (int i = 0; i < allFloor.Length; i++)
            {
                allFloor[i]._FloorPuzzleModel.ResetFloor();
                allFloor[i]._FloorPuzzleModel.ChangeMat(_DefaultColor);
            }

            isResetPuzzle = false;

        }
    }
}
