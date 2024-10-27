using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Introduction : MonoBehaviour
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
        GameManager.Instance.SetGameRun(true);
        gameObject.SetActive(false);
    }
}
