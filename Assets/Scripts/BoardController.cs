
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour {
    #region AllowedKeycodes
    public static readonly KeyCode[] ALLOWED_KEYS = new KeyCode[] {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E,
        KeyCode.F, KeyCode.G,   KeyCode.H, KeyCode.I, KeyCode.J,
        KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O,
        KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
        KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y,
        KeyCode.Z,
    };
    #endregion
    [SerializeField]
    private List<RowController> rows;
    [SerializeField]
    private int rowIndex;
    private int difficulty;
    [SerializeField]
    private int columnIndex;
    private string[] solution;
    private string[] validWords;
    private string language;
    [SerializeField]
    private string word;
    [SerializeField]
    private char[] charArray;
    [SerializeField]
    private List<GameObject> tilesOnBoard;
    [SerializeField]
    private string rem;
    [HideInInspector]
    public RowController debugRow;
    [Space]
    [Header("Difficulty Settings:")]
    [Space]
    public int easyRows;
    public int hardRows;
    [Space]
    [Header("Tiles Attributes:")]
    [Space]
    public TilesController.State empty;
    public TilesController.State correct;
    public TilesController.State occupied;
    public TilesController.State wrongSpot;
    public TilesController.State incorrect;

    [Space]
    [Header("UI Objects:")]
    [Space]
    public GameObject tryAgainButton;
    public GameObject newWordButton;
    public GameObject invalidWordText;
    [Space]
    [Header("UI Board Objects for initialization:")]
    public GameObject rowPrefab;
    public GameObject tilePrefab;
    private void Awake() {
        invalidWordText.SetActive(false);
    }
    private void OnEnable() {
        tryAgainButton.SetActive(false);
        newWordButton.SetActive(false);
    }
    private void OnDisable() {
        tryAgainButton.SetActive(true);
        newWordButton.SetActive(true);
    }
    #region Load data and initialize board
    private void LoadData() {
        switch (language) {
            case "it":
                TextAsset file = Resources.Load("IT-it_words_solutions") as TextAsset;
                solution = file.text.Split("\t");
                file = Resources.Load("IT-it_words_all") as TextAsset;
                validWords = file.text.Split("\t");
                break;
            case "en":
                file = Resources.Load("official_wordle_common") as TextAsset;
                solution = file.text.Split("\n");
                file = Resources.Load("official_wordle_all") as TextAsset;
                validWords = file.text.Split("\n");
                break;
        }

    }
    private void LoadRandomWord() {
        word = solution[Random.Range(0, solution.Length)].ToUpper();
        word.Trim();
        charArray = word.ToCharArray();


    }
    private void BoardInitialization() {
        //create the first row
        var rowInstance = Instantiate(rowPrefab, this.transform);
        rows.Add(rowInstance.GetComponent<RowController>());
        //create much tiles as the letters count in the solution word
        for (int i = 0; i < charArray.Length; i++) {
            tilesOnBoard.Add(Instantiate(tilePrefab, rowInstance.transform));
        }
    }
    #endregion
    private void Start() {
        LoadData();
        ClearBoard();
        BoardInitialization();
        LoadRandomWord();
    }
    public void NewGame() {
        ClearBoard();
        BoardInitialization();
        LoadRandomWord();
    }
    public void Retry() {
        ClearBoard();
        BoardInitialization();
    }
    private void Update() {
        var currentRow = rows[rowIndex];
        debugRow = rows[rowIndex];
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            invalidWordText.SetActive(false);
            columnIndex = Mathf.Max(columnIndex - 1, 0);
            currentRow.tiles[columnIndex].SetLetter('\0');
        }
        else if (columnIndex >= currentRow.tiles.Length) {

            if (Input.GetKeyDown(KeyCode.Return)) {
                ValidateRow(currentRow);
            }

        }
        else {
            for (int i = 0; i < ALLOWED_KEYS.Length; i++) {
                if (Input.GetKeyDown(ALLOWED_KEYS[i])) {
                    currentRow.tiles[columnIndex].SetLetter(char.ToUpper((char)ALLOWED_KEYS[i]));
                    currentRow.tiles[columnIndex].SetState(occupied);
                    columnIndex++;
                    break;
                }
            }
        }
    }
    private void ValidateRow(RowController row) {
        if (!IsValid(row.word)) {
            invalidWordText.SetActive(true);
            return;
        }
        var remaining = word;

        for (int i = 0; i < row.tiles.Length; i++) {

            TilesController tile = row.tiles[i];
            if (tile.letter == word[i]) {
                Debug.Log(tile.letter + " " + word[i]);
                tile.SetState(correct);
                remaining = remaining.Remove(i, 1);
                remaining = remaining.Insert(i, " ");
            }
            else if (!word.Contains(tile.letter)) {
                tile.SetState(incorrect);
            }
        }

        for (int i = 0; i < row.tiles.Length; i++) {
            TilesController tile = row.tiles[i];

            if (tile.state != correct && tile.state != incorrect) {
                if (remaining.Contains(tile.letter)) {
                    tile.SetState(wrongSpot);

                    int index = remaining.IndexOf(tile.letter);
                    remaining = remaining.Remove(index, 1);
                    remaining = remaining.Insert(index, " ");
                }
                else {
                    tile.SetState(incorrect);
                }
            }
        }
        if (HasWon(row)) {
            enabled = false;
        }

        AddRow();
        rowIndex++;
        columnIndex = 0;
        rem = remaining;

        if (rowIndex >= difficulty) {
            enabled = false;
        }
    }

    private bool IsValid(string word) {
        for (int i = 0; i < validWords.Length; i++) {
            if (validWords[i] == word) {
                return true;
            }
        }
        return false;
    }

    private void ClearBoard() {
        rowIndex = 0;
        columnIndex = 0;
        tilesOnBoard = new List<GameObject>();
    }
    private bool HasWon(RowController row) {
        for (int i = 0; i < row.tiles.Length; i++) {
            if (row.tiles[i].state != correct) {
                return false;
            }
        }
        return true;
    }
    public int SetDifficulty(int diff) {
        switch (diff) {
            case 0:
                difficulty = easyRows;
                break;

            case 1:
                difficulty = hardRows;
                break;
        }
        return difficulty;
    }
    public string SetLanguage(string lang) {
        switch (lang) {
            case "it":
                language = lang;
                break;
            case "en":
                language = lang;
                break;

        }
        return language;

    }
    #region Debug Methods

    private void AddRow() {
        var rowInstance = Instantiate(rowPrefab, this.transform);
        rows.Add(rowInstance.GetComponent<RowController>());
        for (int i = 0; i < charArray.Length; i++) {
            tilesOnBoard.Add(Instantiate(tilePrefab, rowInstance.transform));
        }
    }
    public void Debuglog() {

        Debug.Log($"word: {word}");

    }

    public void CheckCorrect(RowController row) {
        var remaining = word;

        for (int i = 0; i < row.tiles.Length; i++) {

            TilesController tile = row.tiles[i];
            if (tile.letter == word[i]) {
                Debug.Log(tile.letter + " " + word[i]);
                tile.SetState(correct);
                remaining = remaining.Remove(i, 1);
                remaining = remaining.Insert(i, " ");
            }
            else if (!word.Contains(tile.letter)) {
                tile.SetState(incorrect);
            }
        }
        rem = remaining;
    }
    public void CheckWrong(RowController row) {
        var remaining = rem;
        for (int i = 0; i < row.tiles.Length; i++) {

            TilesController tile = row.tiles[i];
            if (tile.state != correct && tile.state != incorrect) {
                if (remaining.Contains(tile.letter)) {
                    tile.SetState(wrongSpot);
                    int index = remaining.IndexOf(tile.letter);
                    remaining = remaining.Remove(index, 1);
                    remaining = remaining.Insert(index, " ");

                }
                else {
                    tile.SetState(wrongSpot);
                }
            }

        }
    }
    public void ManualValidate() {
        columnIndex = 5;
        AddRow();
        rem = "";
        rowIndex++;
        columnIndex = 0;
    }
    #endregion
}
