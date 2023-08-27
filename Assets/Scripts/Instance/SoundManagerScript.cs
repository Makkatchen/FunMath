using UnityEngine;
using UnityEngine.UI;

public class SoundManagerScript : MonoBehaviour
{
    public static SoundManagerScript instance { get; private set; }

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip[] _backgroundMusic;

    [SerializeField] private AudioClip _errorInsert;
    [SerializeField] private AudioClip _correctInsert;
    [SerializeField] private AudioClip _explosionStar;

    [SerializeField] private AudioClip _countScore;

    [SerializeField] private AudioClip _winGame;
    [SerializeField] private AudioClip _shineMedalFx;
    [SerializeField] private AudioClip _addMedalFx;
    [SerializeField] private AudioClip _updateLevelValue;

    [SerializeField] private AudioClip _pressButtonUI;

    [SerializeField] private Slider _sladerMusicVolume;
    [SerializeField] private Button _musicOn;
    [SerializeField] private Button _musicOff;


    private bool _toggleMusic;
    private float _volumeMusic;

    private int _numberClip = -1;


    private void Awake()
    {
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

    private void Start()
    {
        _volumeMusic = PlayerPrefs.GetFloat("VolumeMusic", 1);
        _toggleMusic = (PlayerPrefs.GetInt("ToggleMusic", 1) == 1) ? true : false;
        Volume—hange();
        ToggleButtonForMusic(_toggleMusic);
        UpdateMusicForGame();
    }


    private void Volume—hange()
    {
        _sladerMusicVolume.value = _volumeMusic;
        _audioSource.volume = (_toggleMusic) ? _sladerMusicVolume.value : 0;
    }

    public void UpdateMusicForGame()
    {
        int clipNum = Random.Range(0, _backgroundMusic.Length);

        if (clipNum == _numberClip)
        {
            while ((clipNum == _numberClip))
                clipNum = Random.Range(0, _backgroundMusic.Length);
        }

        _numberClip = clipNum;

        PlayBackgroundMusic(_backgroundMusic[_numberClip], true);
    }


    private void PlayBackgroundMusic(AudioClip musicClip, bool loop)
    {
        if (!_audioSource || !musicClip)
        {
            Debug.Log("ÕÂÚ ÌÂÓ·ıÓ‰ËÏ˚ı ‰‡ÌÌ˚ı");
            return;
        }
        _audioSource.clip = musicClip;
        _audioSource.loop = loop;
        _audioSource.Play();
    }

    public void PlaySoundEffects(string name)
    {
        if (name == "errorInsert") PlayFx(_errorInsert, 0.4f);
        else if (name == "correctInsert") PlayFx(_correctInsert, 0.4f);
        else if (name == "winGame") PlayFx(_winGame, 0.5f);
        else if (name == "explosionStar") PlayFx(_explosionStar, 0.4f);
        else if (name == "shineMedal") PlayFx(_shineMedalFx, 0.4f);
        else if (name == "addMedal") PlayFx(_addMedalFx, 0.4f);
        else if (name == "updateLevelValue") PlayFx(_updateLevelValue, 0.4f);
        else Debug.Log("ÌÂÚ Ú‡ÍÓ„Ó ËÏÂÌË ÍÎËÔ‡");
    }

    private void PlayFx(AudioClip clip, float volume)
    {
        if (_toggleMusic) AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume * _volumeMusic);
    }

    public void PressButtonUISound()
    {
        PlayFx(_pressButtonUI, 0.8f);
    }

    private void ToggleButtonForMusic(bool isOn)
    {
        _musicOn.interactable = !isOn;
        _musicOff.interactable = isOn;
    }

    public void ToggleMusicButton(bool isOn)
    {
        if (!isOn) PressButtonUISound();
        ToggleButtonForMusic(isOn);
        int saveParam = (isOn) ? 1 : 0;
        PlayerPrefs.SetInt("ToggleMusic", saveParam);

        _toggleMusic = isOn;

        Volume—hange();

        if (isOn) PressButtonUISound();
    }

    public void ChangedValueVolumeSlider()
    { 
        _volumeMusic = _sladerMusicVolume.value;
        PlayerPrefs.SetFloat("VolumeMusic", _volumeMusic);
        Volume—hange();
    }

}
