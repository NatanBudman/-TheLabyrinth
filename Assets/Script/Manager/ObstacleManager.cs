using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private Transform Player;

    [SerializeField] private LayerMask ObstaclesDetecLayer;
    [SerializeField] private float _angle;
    [SerializeField] private float _radius;
    [SerializeField] private int _maxObstacleDetected;

    private SenserObstacles Doors;
    // Start is called before the first frame update
    void Start()
    {
        Inicializate();
    }

    private void Inicializate()
    {
        SenserObstacles _door = new SenserObstacles(Player, ObstaclesDetecLayer, _maxObstacleDetected, _radius, _angle);
        Doors = _door;
    }

    // Update is called once per frame
    void Update()
    {
        Doors.ObstacleDetected();
    }
}
