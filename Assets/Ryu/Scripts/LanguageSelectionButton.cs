using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LanguageSelectionButton : MonoBehaviour {
  private Image image;
  [SerializeField]
  private Sprite onSprite;
  [SerializeField]
  private Sprite offSprite;

  private void Awake() {
    image = GetComponent<Image>();
  }

  public void ChangeSprite(bool isOn) {
    if (isOn) {
      image.sprite = onSprite;
    }
    else {
      image.sprite = offSprite;
    }
  }

}
