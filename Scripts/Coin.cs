using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour {
    public static event Action OnCoinCollected;

    private void OnDisable() {
        OnCoinCollected?.Invoke();
    }
}
