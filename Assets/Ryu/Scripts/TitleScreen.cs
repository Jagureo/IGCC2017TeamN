using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {
  [SerializeField]
  private GameObject languageSelectionPanel;
  [SerializeField]
  private GameObject panel;

  public void OpenLanguageSelectionPanel() {
    GameObject.Find("PersistentSoundManager").GetComponent<soundPlayer>().PlaySoundEffect("ErrorSound3");
    panel.SetActive(true);
    languageSelectionPanel.SetActive(true);
  }

  public void StartGame() {
    SceneManager.LoadScene("InventoryOnly+House+Dialog+SelectGender");
  }
}
