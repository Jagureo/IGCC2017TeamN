using UnityEngine;
using System.Collections;

public class WebcamAnimator : MonoBehaviour {
  private Animator animator;

  private void Awake() {
    animator = GetComponent<Animator>();
  }

  public void OnProgressionChanged(string current) {
    if (current == "DisableWebcam") {
      animator.Play("Webcam@Disabled");
    }
  }

}
