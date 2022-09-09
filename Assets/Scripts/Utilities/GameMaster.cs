using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gameMaster;

    public float currentScore = 0f;
    public float difficultyMultiplier = 0f;

    private void Awake()
    {
        if (gameMaster is not null && gameMaster != this) 
        {
            Destroy(this);
            return;
        }
        if (gameMaster is null)
        {
            gameMaster = this;
        }
    }


}
