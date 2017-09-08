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
    target.text = LocalizationManager.Instance.GetText(key);
  }

}
