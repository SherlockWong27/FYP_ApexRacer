using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Instruction : MonoBehaviour
{
    [SerializeField] public GameObject elements = null;
    [SerializeField] private Button closeButton = null; // Close the elements
    //Getter function to get the instance of the class
    private static Instruction instance = null;
    public static Instruction instanceClass
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        elements.SetActive(false);
    }
    public void SetStatus(bool status)
    {
        elements.SetActive(status);
    }

    private void Start()
    {
        // Add listeners to the buttons
        closeButton.onClick.AddListener(Closed);
    }

    // Start the rain effect
    private void Closed()
    {
        elements.SetActive(false);
        MainMenu.instanceClass.SetStatus(true);
    }
}
