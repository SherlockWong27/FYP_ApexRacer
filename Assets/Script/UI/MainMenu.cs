using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button singlePlayerButton = null;
    [SerializeField] private Button twoPlayerButton = null;
    [SerializeField] private Button instructionButton = null;
    [SerializeField] public GameObject elements = null;
    //Getter function to get the instance of the class
    private static MainMenu instance = null;
    public static MainMenu instanceClass
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        elements.SetActive(true);
    }
    public void SetStatus(bool status)
    {
        elements.SetActive(status);
    }
    // Start is called before the first frame update
    void Start()
    {
        singlePlayerButton.onClick.AddListener(PlayOnePlayer);
        twoPlayerButton.onClick.AddListener(PlayTwoPlayer);
        instructionButton.onClick.AddListener(OpenInstruction);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlayOnePlayer()
    {
        SetStatus(false);
        OnePlayer.instanceClass.SetStatus(true);
    }

    private void PlayTwoPlayer()
    {
        SetStatus(false);
        TwoPlayer.instanceClass.SetStatus(true);
    }

    private void OpenInstruction()
    {
        SetStatus(false);
        Instruction.instanceClass.SetStatus(true);
    }
}
