using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TyreMenu2Player : MonoBehaviour
{
    [SerializeField] public GameObject elements = null;
    [SerializeField] public GameObject player1Select = null;
    [SerializeField] public GameObject player2Select = null;
    //Getter function to get the instance of the class
    private static TyreMenu2Player instance = null;
    public static TyreMenu2Player instanceClass
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
        player2Select.SetActive(false);
        player1Select.SetActive(true);
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
        TwoPlayer.instanceClass.SetStatus(true);
    }

    public void Player2Select()
    {
        player2Select.SetActive(true);
        player1Select.SetActive(false);
    }
}
