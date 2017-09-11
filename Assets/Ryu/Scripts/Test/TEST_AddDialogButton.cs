using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TEST_AddDialogButton : MonoBehaviour {
  [SerializeField]
  private InputField inputField;
  [SerializeField]
  private DialogWindow dialogWindow;

  public void OnClicked() {
    dialogWindow.AddDialog(inputField.text);
  }

}
