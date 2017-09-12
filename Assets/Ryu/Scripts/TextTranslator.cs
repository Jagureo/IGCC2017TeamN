using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[DisallowMultipleComponent
,RequireComponent(typeof(Text))
]
public class TextTranslator : MonoBehaviour {
  private Text target;

  [SerializeField]
  private string key;

  public string Key {
    get { return key; }
    set {
      key = value;
      Translate();
    }
  }

  private void Awake() {
    target = GetComponent<Text>();
  }

  private void Start() {
    LocalizationManager.Instance.onLanguageChanged += Translate;
    if (key.Length > 0) {
      Translate();
    }
  }

  private void OnDestroy() {
    LocalizationManager.Instance.onLanguageChanged -= Translate;
  }

  private void Translate() {
    var text = LocalizationManager.Instance.GetText(key);

    var gender = GameSettingManager.Instance.Gender;
    string honorificsKey;
    switch (gender) {
    case Gender.Male: honorificsKey = "HonorificsForMen"; break;
    case Gender.Female: honorificsKey = "HonorificsForWomen"; break;
    default: honorificsKey = "HonorificsForOther"; break;
    }

    var honorifics = LocalizationManager.Instance.GetText(honorificsKey);
    var output = text.Replace("%(honorifics)", honorifics);

    target.text = output;
  }

}
