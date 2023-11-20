using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{

    private const int nAchievements = 3;
    public enum Achievement_ID
    {
        Coin_Collector,
        Terminator,
        SomeOtherFancyAchievement
    }

    private bool[] bUnlockedAchievements = new bool[nAchievements];


    private int nCoins = 0;
    private int nEnemies = 0;

    public Button showButton;

    private void Start()
    {
        Coin.OnCoinCollected += CoinWasCollected;
        Enemy.OnEnemyDestroyed += EnemyWasDestroyed;
        showButton.onClick.AddListener(ShowAchievements);
    }

    void CoinWasCollected()
    {
        nCoins++;
        Debug.Log("Coin Collected! You have " + nCoins + " coins.");
        if (nCoins == 5)
        {
            int index = (int)Achievement_ID.Coin_Collector;
            if (!bUnlockedAchievements[index])
            {
                bUnlockedAchievements[index] = true;
                Debug.Log("You've unlocked: COIN COLLECTOR!!!");
            }
        }
    }

    void EnemyWasDestroyed()
    {
        nEnemies++;
        Debug.Log("Enemy Destroyed! You have destroyed " + nEnemies + " enemies.");
        if (nEnemies == 10)
        {
            int index = (int)Achievement_ID.Terminator;
            if (!bUnlockedAchievements[index])
            {
                bUnlockedAchievements[index] = true;
                Debug.Log("You've unlocked: TERMINATOR!!!");
            }
        }
    }

    void ShowAchievements()
    {
        Debug.Log("Your achievements are:");
        for (int i = 0; i < nAchievements; i++)
        {
            if (bUnlockedAchievements[i])
            {
                Debug.Log(((Achievement_ID)i).ToString()); 
            }
        }
    }

}
