using Miku.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public PlayerController PlayerController;
    [HideInInspector]
    public PlayerCombat PlayerCombat;

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        PlayerCombat = GetComponent<PlayerCombat>();
    }

}
