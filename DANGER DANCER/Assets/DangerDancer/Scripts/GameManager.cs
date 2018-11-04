using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingletonPersistent<GameManager>
{
    public bool didTutorial = false;
    public int CHEATStartBeat = 0;
    public bool CHEATNoReduceScore = false;
}
