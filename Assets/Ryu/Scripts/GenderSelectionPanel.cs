using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class GenderSelectionPanel : MonoBehaviour {
  public delegate void ValueChangeEvent(Toggle newActive);
  public event ValueChangeEvent onValueChange;

  private ToggleGroup toggleGroup;

  private void Awake() {
    toggleGroup = GetComponent<ToggleGroup>();
  }

  public void Start() {
    onValueChange += (newActive) => {
      switch (newActive.name) {
      case "Male":
        GameSettingManager.Instance.Gender = Gender.Male;
        break;
      case "Female":
        GameSettingManager.Instance.Gender = Gender.Female;
        break;
      default:
        break;
      }
    };
    foreach (Transform transformToggle in gameObject.transform) {
      var toggle = transformToggle.GetComponent<Toggle>();
      if (toggle == null) continue;
      toggle.onValueChanged.AddListener((isSelected) => {
        if (!isSelected) return;
        var activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        onValueChange(activeToggle);
      });
    }
  }

  public void Submit() {
    gameObject.SetActive(false);
    var player = FindObjectOfType<PlayerController>();
    if (player == null) return;
    player.gender = (int)GameSettingManager.Instance.Gender;
  }
}
