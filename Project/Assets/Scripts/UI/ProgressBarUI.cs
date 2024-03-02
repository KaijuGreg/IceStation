using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {

    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start() {

        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>(); // this is because Interfaces can not be exposed in the editor 

        if(hasProgress == null) {
            Debug.LogError("Game Object " + hasProgressGameObject + " doesn't have a component that implements IHasProgress interface!");
        }

        // so then we can initialise this event...
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventsArgs e) {
        barImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f) {
         Hide();
        }
        else {
            Show();
        }

    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}



