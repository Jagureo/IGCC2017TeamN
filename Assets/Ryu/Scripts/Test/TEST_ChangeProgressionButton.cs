using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TEST_ChangeProgressionButton : MonoBehaviour {
  [SerializeField]
  private InputField inputField;
  [SerializeField]
  private ProgressionManager progressionManager;

  public void OnClicked() {
    progressionManager.ChangeProgression(inputField.text);
  }

}
