using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TilesController : MonoBehaviour {
    [System.Serializable]
    public class State {
        public Color fillColor;
        public Color outlineColor;
    }
    public State state { get; private set; }
    public char letter { get; private set; }

    private Image fillImage;
    private Outline outline;
    private TextMeshProUGUI text;

    private void Awake() {
        fillImage = GetComponent<Image>();
        outline = GetComponent<Outline>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetLetter(char letter) {
        this.letter = letter;
        text.text = letter.ToString();
    }
    public void SetState(State state) {
        this.state = state;
        fillImage.color = state.fillColor;
        outline.effectColor = state.outlineColor;
    }

}
