using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{ 
    bool initOnce = false;
    private void Update()
    {
        if(!initOnce)
        {
            initOnce = true;
            DialogController d = (DialogController)FindObjectOfType<DialogController>();
            d.ShowNextDialog();
        }
    }
}
