using UnityEngine;
using UnityEngine.UI;

public class SelectBGForGame : MonoBehaviour
{
    public static SelectBGForGame instance { get; private set; }

    [SerializeField] private Sprite[] _backgrounds;
    [SerializeField] private Image _background;

    private void Awake()
    {
        _background.enabled = false;

        if (instance == null)
        {
            instance = this;
            return;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void SelectBg()
    {
        _background.enabled = true;
        int numBg = Random.Range(0, _backgrounds.Length);
        _background.sprite = _backgrounds[numBg];
    }
}
