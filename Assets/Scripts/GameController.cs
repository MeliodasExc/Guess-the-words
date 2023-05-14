using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private BoardController board;
    public GameObject settings_panel;
    [HideInInspector]
    public string language="it";
    [HideInInspector]
    public int difficulty=0; 
    [HideInInspector] 
    public ButtonsController bController;
    private void Start() {
        board = bController.board;
        board.SetDifficulty(difficulty);
        board.SetLanguage(language);
        settings_panel.SetActive(false);
    }
    public void OnSettingsOpen() {
        if (settings_panel.activeInHierarchy) {
            settings_panel.SetActive(false);
        }
        else {
            settings_panel.SetActive(true);
        }
    }
    public void OnLanguageChange(string lang) {
        language = lang;
        board.SetLanguage(language);
        if(lang=="it") {
            
            bController.langen_b.GetComponent<Outline>().effectColor = Color.black;
            bController.langit_b.GetComponent<Outline>().effectColor = Color.yellow;
            bController.langen_b.enabled = false;
            bController.langit_b.enabled = true;
        }
        else {

            bController.langen_b.GetComponent<Outline>().effectColor = Color.yellow;
            bController.langit_b.GetComponent<Outline>().effectColor = Color.black;
            bController.langen_b.enabled = true;
            bController.langit_b.enabled = false;
        }
    }
    public void OnDifficultyChange(int diff) {
        difficulty = diff;
        board.SetDifficulty(difficulty);
        if (diff==0) {
            bController.hard_b.GetComponent<Outline>().effectColor = Color.yellow;
            bController.easy_b.GetComponent<Outline>().effectColor = Color.black;  
            bController.hard_b.enabled = false;
            bController.easy_b.enabled = true;
            
        }
        else {
            bController.hard_b.GetComponent<Outline>().effectColor = Color.black;
            bController.easy_b.GetComponent<Outline>().effectColor = Color.yellow;
            bController.hard_b.enabled = true;
            bController.easy_b.enabled = false;
        }
    }

    
}
