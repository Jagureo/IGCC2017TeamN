using UnityEngine;
using System.Collections;

public class TEST_ChangeLanguageButton : MonoBehaviour {
  [SerializeField]
  private string languageKey;

  public void onClicked() {
    LocalizationManager.Instance.ChangeLanguage(languageKey);
  }
}
