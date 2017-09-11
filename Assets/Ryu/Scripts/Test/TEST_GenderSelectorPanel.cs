using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

[DisallowMultipleComponent
,RequireComponent(typeof(ToggleGroup))
]
public class TEST_GenderSelectorPanel : MonoBehaviour {
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
      toggle.onValueChanged.AddListener((isSelected) => {
        if (!isSelected) return;
        var activeToggle = Active();
        onValueChange(activeToggle);
      });
    }
  }

  public Toggle Active() {
    return toggleGroup.ActiveToggles().FirstOrDefault();
  }

}
