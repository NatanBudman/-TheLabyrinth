using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConditions
{

    void Lose();

    void Win();

    bool isStatusGame(bool isWin);

    bool isWin();

}
