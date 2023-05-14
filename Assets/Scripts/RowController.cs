using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RowController : MonoBehaviour
{
    public TilesController[] tiles { get; private set; }
    public string word {
        get {
            string word = "";
            for(int i =0; i < tiles.Length; i++) {
                word += tiles[i].letter;
            }
            return word;
        }
    }
    private void Start() {
    tiles=GetComponentsInChildren<TilesController>();    
    }
}
