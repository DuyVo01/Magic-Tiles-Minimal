using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class GameRepository : MonoBehaviour
{
    public static GameRepository Instance { get; private set; }

    public ReactiveProperty<List<TileLineX>> tileLine = new ReactiveProperty<List<TileLineX>>(
        new List<TileLineX>()
    );

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
    }
}
