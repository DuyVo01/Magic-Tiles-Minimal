using System;
using UnityEngine;
using UnityEngine.UI;

public class StartingTileBehavior : TileBehavior
{
    [SerializeField]
    private Button startingButton;

    void OnEnable()
    {
        startingButton.onClick.AddListener(StartGame);
    }

    void OnDisable()
    {
        startingButton.onClick.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        //
    }
}
