using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    Player m_Player;
    
	// Use this for initialization
	void Start ()
    {
        m_Player = new Player();
	}

    // Update is called once per frame
    void Update ()
    {
        m_Player.Update();
	}
}
