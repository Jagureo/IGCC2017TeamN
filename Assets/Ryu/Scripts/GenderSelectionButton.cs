using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GenderSelectionButton : MonoBehaviour {
  [SerializeField]
  private Image targetImage;
  [SerializeField]
  private Sprite offSprite;
  [SerializeField]
  private Sprite onSprite;

  public void ChangeImage(bool isOn) {
    Sprite sprite;
    if (isOn) {
      sprite = onSprite;
    }
    else {
      sprite = offSprite;
    }
    targetImage.sprite = sprite;
  }
}
