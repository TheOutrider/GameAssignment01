using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    [SerializeField] private Button startButton;


    void Start()
    {
        startButton.onClick.AddListener(OnStartPressed);
    }

    private void OnStartPressed()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
