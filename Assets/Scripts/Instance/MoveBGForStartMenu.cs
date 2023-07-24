using UnityEngine;
using DG.Tweening;

public class MoveBGForStartMenu : MonoBehaviour
{

    [SerializeField] private GameObject _bg;

    [SerializeField] private float _xPositionStop;
    [SerializeField] private float _time;

    private Tween _moveBg;

    private int _mod;

    public static MoveBGForStartMenu instance { get; private set; }

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

    void Start()
    {
        _mod = 1;
        StartMoveBg(_mod);
    }

    private void StartMoveBg(int mod)
    {
        _moveBg = _bg.transform.DOMoveX(_xPositionStop * mod, _time).SetDelay(0.5f).OnComplete(()=> RestartMove());
    }

    private void RestartMove()
    {
        KillTweenMoveBg();
        _mod = 0 - _mod;
        StartMoveBg(_mod);
    }

    public void KillTweenMoveBg()
    {
        _moveBg.Kill();
    }

    public void DellBG()
    {
        _moveBg.Kill();
        gameObject.SetActive(false);
    }

    
}
