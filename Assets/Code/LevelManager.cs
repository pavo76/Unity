using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using  UnityEngine;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Player Player { get; private set; }
    public CameraCotroller Camera { get; private set; }
    public TimeSpan RunningTime { get { return DateTime.UtcNow - _started; } }

    public int CurrentTimeBonus
    {
        get
        {
            var secondDifference = (int) (BonusCutoffSeconds - RunningTime.TotalSeconds);
            return Mathf.Max(0, secondDifference)*BonusSecondsMultiplier;
        }
    }

    private List<Checkpoint> _checkpoints;
    private int _currentCheckpoint;
    private DateTime _started;
    private int _savedPoints;


    public Checkpoint DebugSpawn;
    public int BonusCutoffSeconds;
    public int BonusSecondsMultiplier;

    public void Awake()
    {
        _savedPoints = GameManager.Instance.Points;
        Instance = this;

    }

    public void Start()
    {
        _checkpoints = FindObjectsOfType<Checkpoint>().OrderBy(t => t.transform.position.x).ToList();
        _currentCheckpoint = _checkpoints.Count > 0 ? 0 : -1;

        Player = FindObjectOfType<Player>();
        Camera = FindObjectOfType<CameraCotroller>();

        _started=DateTime.UtcNow;

        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();
        foreach (var listener in listeners)
        {
            for (var i = _checkpoints.Count - 1; i >= 0; i--)
            {
                var distance = ((MonoBehaviour)listener).transform.position.x - _checkpoints[i].transform.position.x;
                if (distance < 0)
                    continue;
                _checkpoints[i].AssignObjectToCheckpoint(listener);
                break;
            }
        }
#if UNITY_EDITOR
        if(DebugSpawn!=null)
            DebugSpawn.SpawnPlayer(Player);
        else if(_currentCheckpoint!=-1)
            _checkpoints[_currentCheckpoint].SpawnPlayer(Player);
#else
        if(_currentCheckpoint!=-1)
            _checkpoints[_currentCheckpoint].SpawnPlayer(Player);
#endif
    }

    public void Update()
    {
        var isAtLastCheckpoint = _currentCheckpoint + 1 >= _checkpoints.Count;
        if (isAtLastCheckpoint)
            return;
        var distanceToNextCheckpoint = _checkpoints[_currentCheckpoint+1].transform.position.x -
                                       Player.transform.position.x;
        if(distanceToNextCheckpoint>=0)
            return;

        _checkpoints[_currentCheckpoint].PlayerLeftCheckpoint();
        _currentCheckpoint++;
        _checkpoints[_currentCheckpoint].PlayerHitCheckpoint();
        
        GameManager.Instance.AddPoints(CurrentTimeBonus);
        _savedPoints = GameManager.Instance.Points;
        _started = DateTime.UtcNow;

        
    }

    public void KillPlayer()
    {
        StartCoroutine(KillPlayerCo());
    }

    public void GoToNextLevel(string levelName)
    {
        StartCoroutine(GoToNextLevelCo(levelName));
    }

    private IEnumerator GoToNextLevelCo(string levelName)
    {
        Player.FinishLevel();
        GameManager.Instance.AddPoints(CurrentTimeBonus);
        FloatingText.Show("Level Complete!", "CheckpointText", new CenteredTextPositioner(.5f));
        yield return new WaitForSeconds(1);
        FloatingText.Show(string.Format("{0} points!", GameManager.Instance.Points), "CheckpointText",
            new CenteredTextPositioner(.1f));

        yield return new WaitForSeconds(5f);

        if(string.IsNullOrEmpty(levelName))
            Application.LoadLevel("StartScreen");
        else
            Application.LoadLevel(levelName);
    }
    private IEnumerator KillPlayerCo()
    {
        Player.Kill();

        Camera.IsFollowing = false;
        yield return new WaitForSeconds(2f);

        Camera.IsFollowing = true;
        if(_currentCheckpoint!=-1)
            _checkpoints[_currentCheckpoint].SpawnPlayer(Player);

        _started = DateTime.UtcNow;
        GameManager.Instance.ResetPoints(_savedPoints);

    }
}