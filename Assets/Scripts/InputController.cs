using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    private TMP_InputField inputField;
    public bool limitReached;
    private GameController gameController;
    private void Awake() {
        inputField=GetComponent<TMP_InputField>();  
        gameController=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gameController.inputController = this;
    }

    // Update is called once per frame
    void Update()
    {
        limitReached = inputField.characterLimit == inputField.text.Length ? true : false;
        
    }
}
