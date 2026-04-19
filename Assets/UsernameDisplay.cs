using TMPro;
using UnityEngine;

public class UsernameDisplay : MonoBehaviour
{
    private TMP_Text text;

    void Awake() {
        text = GetComponent<TMP_Text>();
    }

    void Start() {
        text.text = "User: " + UserSession.CurrentUsername;
    }
}