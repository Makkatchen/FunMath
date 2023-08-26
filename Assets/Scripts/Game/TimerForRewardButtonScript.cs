using UnityEngine;

public class TimerForRewardButtonScript : MonoBehaviour
{
    public static TimerForRewardButtonScript instance { get; private set; }
    bool _isStart;

    float _time;

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

    public void StartTimer()
    {
        _time = 0;
        _isStart = true;
    }

    public void StopTimer()
    {
        _time = 0;
        _isStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isStart)
            _time += Time.deltaTime;

        if (_time >= 10)
        {
            StopTimer();
            NavigationGame.instance.ShowRewardButton(true);
        }
    }
}
