using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {
  [SerializeField]
  private GameObject languageSelectionPanel;
  [SerializeField]
  private GameObject panel;

  public void OpenLanguageSelectionPanel() {
    panel.SetActive(true);
    languageSelectionPanel.SetActive(true);
  }
}
