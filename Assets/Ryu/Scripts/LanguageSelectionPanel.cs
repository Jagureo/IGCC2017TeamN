using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class LanguageSelectionPanel : MonoBehaviour {
  public delegate void ValueChangeEvent(Toggle newActive);
  public event ValueChangeEvent onValueChange;

  private ToggleGroup toggleGroup;
  private string selectedLanguage;

  private void Awake() {
    toggleGroup = GetComponent<ToggleGroup>();
    selectedLanguage = toggleGroup.ActiveToggles().FirstOrDefault().name;
  }

  public void Start() {
    foreach (Transform transformToggle in gameObject.transform) {
      var toggle = transformToggle.GetComponent<Toggle>();
      if (toggle == null) continue;
      toggle.onValueChanged.AddListener((isSelected) => {
        if (!isSelected) return;
        var activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        onValueChange(activeToggle);
      });
    }

    onValueChange += (newActive) => {
      selectedLanguage = newActive.name;
    };
  }

  public void Submit() {
    gameObject.SetActive(false);
    LocalizationManager.Instance.ChangeLanguage(selectedLanguage);
  }

}
