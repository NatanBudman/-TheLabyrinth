using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Color[] titleColors;
    [SerializeField] private Image title;
     private int _currentColor = 0;
     [SerializeField] private float CooldownNextColor;
     private float _currentNextColorm;

     private void Start()
     {
         title.color = titleColors[_currentColor];
     }

     private void Update()
     {
         _currentNextColorm += Time.deltaTime;

         if (_currentNextColorm > CooldownNextColor)
         {
             _currentColor++;
             if (_currentColor >= titleColors.Length) _currentColor = 0;
             title.color = titleColors[_currentColor];
             _currentNextColorm = 0;
         }
     }
}
