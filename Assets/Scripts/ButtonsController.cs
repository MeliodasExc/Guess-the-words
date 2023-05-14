using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour {
    public BoardController board;
    public GameController gController;
    [Header("Debug Buttons:")]
    [Space]
    public Button debug_b, checkCorrect_b, checkWrong_b, manual_b;

    [Space]
    [Header("UI Buttons:")]
    [Space]
    public Button retry_b, next_b, settings_b, closeSett_b, langen_b, langit_b, easy_b, hard_b;

    private void Awake() {
        gController.bController = this;
    }
    void Start() {
        #region Debug Buttons
        debug_b.onClick.AddListener(board.Debuglog);
        manual_b.onClick.AddListener(board.ManualValidate);
        checkCorrect_b.onClick.AddListener(delegate { board.CheckCorrect(board.debugRow); });
        checkWrong_b.onClick.AddListener(delegate { board.CheckWrong(board.debugRow); });
        #endregion
        retry_b.onClick.AddListener(board.Retry);
        next_b.onClick.AddListener(board.NewGame);
        settings_b.onClick.AddListener(gController.OnSettingsOpen);
        closeSett_b.onClick.AddListener(gController.OnSettingsOpen);
        langen_b.onClick.AddListener(delegate { gController.OnLanguageChange("en"); });
        langit_b.onClick.AddListener(delegate { gController.OnLanguageChange("it"); });
        easy_b.onClick.AddListener(delegate { gController.OnDifficultyChange(0); });
        hard_b.onClick.AddListener(delegate { gController.OnDifficultyChange(1); });
    }
}
