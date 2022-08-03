using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Minigame1EventHandler : MonoBehaviour
{
    public static Minigame1EventHandler instance;
    public event Action onEatCharacter;
    public event Action onGameEnd;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void EatCharacterTrigger()
    {
        if (onEatCharacter != null)
        {
            onEatCharacter();
        }
    }

    public void GameEndTrigger()
    {
        if (onGameEnd != null)
        {
            onGameEnd();
        }
    }
}
