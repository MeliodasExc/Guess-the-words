using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [Header("gameObjects references")]
    [Space]
    public string[] wordsToGuess;
    public GameObject panelGuess;
    public GameObject panelRow;
    public GameObject inputRow;
    public GameObject guessLetter;
    public GameObject inputLetter;
    public TextMeshProUGUI correctLetter;
    [SerializeField]
    private GameObject[] guessLetterArray;
    [SerializeField]
    private GameObject[] inputRowArray;
    [SerializeField]
    private GameObject[] inputLetterArray;
    [SerializeField]
    private List<string> lastInputLetterList;
    [SerializeField]
    private char[] word;
    [SerializeField]
    private List<string> letterList;
    [HideInInspector]
    public InputController inputController;
    [Space]
    [Header("Attributes")]
    [SerializeField]
    private int difficulty = 1;
    private int numberOfRows;
    [SerializeField]
    private int index;
    [SerializeField]
    private int counter;
    public int numberOfRowsEasy = 6;
    public int numberOfRowsMedium = 4;
    public int numberOfRowsHard = 3;
    private void Start() {
        NewWord();
    }
    public void NewWord() {
        word = new char[0];
        var randomWord = wordsToGuess[Random.Range(0, wordsToGuess.Length)].ToUpper();
        word = randomWord.ToCharArray();
        lastInputLetterList = new List<string>(word.Length);
        foreach(var letter in word) {
            letterList.Add(letter.ToString());
        }
        guessLetterArray = new GameObject[word.Length];
        inputLetterArray = new GameObject[word.Length];
        for (int i = 0; i < word.Length; i++) {
            var newLetter = Instantiate(guessLetter, panelGuess.transform);
            guessLetterArray[i] = newLetter;
        }

        switch (difficulty) {
            case 1:
                numberOfRows = numberOfRowsEasy;
                inputRowArray = new GameObject[numberOfRowsEasy];

                break;
            case 2:
                numberOfRows = numberOfRowsMedium;
                inputRowArray = new GameObject[numberOfRowsMedium];

                break;
            case 3:
                numberOfRows = numberOfRowsHard;
                inputRowArray = new GameObject[numberOfRowsHard];

                break;
        }
        var newRow = Instantiate(inputRow, panelRow.transform);
        inputRowArray[index] = newRow;
        for (int j = 0; j < word.Length; j++) {
            var newInput = Instantiate(inputLetter, newRow.transform);
            inputLetterArray[j] = newInput;
        }
    }
    public void ChangeRow() {
        index++;
       
        if (index < numberOfRows) {
            inputLetterArray = new GameObject[word.Length];
            var newRow = Instantiate(inputRow, panelRow.transform);
            inputRowArray[index] = newRow;
            for (int j = 0; j < word.Length; j++) {
                Debug.Log($"index {j}");
                var newInput = Instantiate(inputLetter, newRow.transform);
                if (lastInputLetterList[j] == letterList[j]) {
                    inputLetterArray[j] = newInput;
                    inputLetterArray[j].GetComponent<TMP_InputField>().text = lastInputLetterList[j];
                    inputLetterArray[j].GetComponent<TMP_InputField>().interactable = false;
                    inputLetterArray[j].GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                    guessLetterArray[j].GetComponentInChildren<TextMeshProUGUI>().text = lastInputLetterList[j];

                }
                else {
                    inputLetterArray[j] = newInput;
                }
            }
        }
        else {
            Debug.LogWarning("End");
        }
    }
    public void ValidateWord() {
        
        CheckLetter();
        ValidateRow();
        ChangeRow();
        counter = 0;
    }
    private void CheckLetter() {
        lastInputLetterList = new List<string>();
        foreach (var input in inputLetterArray) {
            var inputField = input.GetComponent<TMP_InputField>();
            if (inputField != null) {
                lastInputLetterList.Add(inputField.text);
                if (inputField.text == letterList[counter]) {
                    inputField.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                }
                else {
                    inputField.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                    Debug.Log("false");
                }
                Debug.Log(counter);
                counter++;
            }
            else {
                break;
            }
        }
    }
    private void ValidateRow() {
        foreach(var input in inputLetterArray) {
            input.GetComponent<TMP_InputField>().interactable = false;
        }
    }
}
