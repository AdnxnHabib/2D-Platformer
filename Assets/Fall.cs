﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fall : MonoBehaviour
{

     
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PermanentUI.perm.Reset();
        }
    }
}
