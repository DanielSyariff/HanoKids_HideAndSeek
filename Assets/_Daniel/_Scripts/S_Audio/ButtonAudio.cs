using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public AudioClip buttonClickClip;

    private Button button;
     
    private void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        AudioManager.instance.PlayOneShot(buttonClickClip);
    }
}
