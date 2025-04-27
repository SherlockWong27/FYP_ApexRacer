using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TyresMenu : MonoBehaviour
{
    [SerializeField] public GameObject elements = null;
    //Getter function to get the instance of the class
    private static TyresMenu instance = null;
    public static TyresMenu instanceClass
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

    }

    // Start the rain effect
    public void PlayGame()
    {
        elements.SetActive(false);
        OnePlayer.instanceClass.SetStatus(true);
    }
}
