using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TEST_ChangeProgressionButton : MonoBehaviour {
  [SerializeField]
  private InputField inputField;
  [SerializeField]
  private DialogWindow dialogWindow;

  public void OnClicked() {
    dialogWindow.AddDialog(inputField.text);
  }

}
