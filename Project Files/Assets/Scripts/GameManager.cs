using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    [SerializeField] private Puck puckPrefab;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private PlayerMove _playerPrefab;
    [SerializeField] private TextMeshProUGUI _PscoreText;
    [SerializeField] private TextMeshProUGUI _EscoreText;
    [SerializeField] private TextMeshProUGUI _ResetText;
    [SerializeField] private TextMeshProUGUI _Countdown;
    [SerializeField] private TextMeshProUGUI WinText;
    [SerializeField] private TextMeshProUGUI LoseText;
    [SerializeField] private GameObject BackgroundB;
    [SerializeField] private GameObject BackgroundY;
    [SerializeField] private GameObject BackgroundR;
    [SerializeField] private GameObject warn1;
    [SerializeField] private GameObject warn2;
    [SerializeField] private Portal _portalPrefab;

    private readonly Vector2 _leftGoal = new Vector2(-8f, 0);
    private readonly Vector2 _rightGoal = new Vector2(8f, 0);
    private Puck _spawnedPuck;
    private Portal _portal1;
    private Portal _portal2;
    private Portal _placeholderP;
    private Enemy _enemy;
    private bool _checkPortalPos = false;
    private bool GameOver = false;
    private PlayerMove _player;
    public int playerScore = 5;
    public int enemyScore = 5;
    private float t = 0f;
    private float _signalTimer;
    private bool _resetPoint = false;
    private float _portalSpawnTimer = 20f;
    private float _stuckPuckTimer = 0f;
    private bool _stuckPuck = false;
    private bool firstReset;

    void Start()
    {
        _spawnedPuck = Instantiate(puckPrefab);
        _player = Instantiate(_playerPrefab);
        _enemy = Instantiate(_enemyPrefab);
        _enemy.puck = _spawnedPuck.puck;
        _enemy.puckclass = _spawnedPuck;
        _portal1 = Instantiate(_portalPrefab);
        _portal2 = Instantiate(_portalPrefab);
        firstReset = true;
        _resetPoint = true;
        
    }

    private void Update()
    {
        if (GameOver == false)
        {
            if (playerScore >= 3)
            {
                WinText.gameObject.SetActive(true);
                GameOver = true;
                
                
            }

            if (enemyScore >= 3)
            {
                LoseText.gameObject.SetActive(true); 
                GameOver = true;
            }

            if (_spawnedPuck.lastHitBy == 1)
            {
                BackgroundY.SetActive(true);
                BackgroundB.SetActive(false);
                BackgroundR.SetActive(false);
            }
            else if (_spawnedPuck.lastHitBy == 2)
            {
                BackgroundR.SetActive(true);
                BackgroundB.SetActive(false);
                BackgroundY.SetActive(false);
            }
            else
            {
                BackgroundB.SetActive(true);
                BackgroundY.SetActive(false);
                BackgroundR.SetActive(false);
            }

            if (_spawnedPuck.eFoul)
            {
                Foul(false);
                _spawnedPuck.eFoul = false;

            }

            if (_spawnedPuck.pFoul)
            {
                Foul(true);
                _spawnedPuck.pFoul = false;
            }

            if (_spawnedPuck.puck.velocity.magnitude < 0.5f)
            {
                _stuckPuck = true;
            }
            else if (_spawnedPuck.puck.velocity.x < 1f &&
                     Vector2.Distance(_leftGoal, _spawnedPuck.puck.position) > 5f &&
                     Vector2.Distance(_rightGoal, _spawnedPuck.puck.position) > 5f)
            {
                _stuckPuck = true;
            }
            else
            {
                _stuckPuck = false;
            }

            if (_stuckPuck)
            {
                _stuckPuckTimer += Time.deltaTime;
            }
            else
            {
                _stuckPuckTimer = 0f;
                _resetPoint = false;
                _ResetText.gameObject.SetActive(false);
                _Countdown.gameObject.SetActive(false);
                t = 0f;
            }

            if (_stuckPuckTimer >= 2f)
            {

                _resetPoint = true;
            }


            _portalSpawnTimer += Time.deltaTime;
            if (_portalSpawnTimer >= 17f && _portalSpawnTimer < 20f)
            {
                warn1.SetActive(true);
                warn2.SetActive(true);
            }

            if (_portalSpawnTimer >= 20f)
            {
                _portalSpawnTimer = 0f;
                while (_checkPortalPos == false)
                {
                    SetPortalPos(_portal1);
                    SetPortalPos(_portal2);
                    if ((_portal1.gameObject.transform.position.x - _portal2.gameObject.transform.position.x > 1.6f) &&
                        (_portal1.gameObject.transform.position.y - _portal2.gameObject.transform.position.y > 1.6f))
                    {
                        _checkPortalPos = true;
                        Debug.Log("portals good");
                        warn1.transform.position = _portal1.gameObject.transform.position;
                        warn1.SetActive(false);
                        warn2.transform.position = _portal2.gameObject.transform.position;
                        warn2.SetActive(false);
                    }
                }

                _checkPortalPos = false;
            }

            if (_resetPoint)
            {
                var displayt = Mathf.RoundToInt(t);
                if (firstReset != true)
                {
                    _ResetText.gameObject.SetActive(true);
                }
                else
                {
                    _ResetText.gameObject.SetActive(false);
                }
                _Countdown.gameObject.SetActive(true);
                if (t < 5f)
                {

                    _Countdown.text = (5 - displayt).ToString();
                    t += Time.deltaTime;
                }
                else
                {
                    _ResetText.gameObject.SetActive(false);
                    _Countdown.gameObject.SetActive(false);
                    firstReset = false;
                    _spawnedPuck.lastHitBy = 3;
                    _resetPoint = false;
                    _spawnedPuck.puck.velocity = new Vector2(3, 0);
                    _spawnedPuck.puck.position = Vector2.zero;
                    _enemy.puck = _spawnedPuck.puck;
                    _enemy.puckclass = _spawnedPuck;
                    t = 0f;
                }
            }

            if (_spawnedPuck.playerGoal)
            {
                t = 0f;
                _spawnedPuck.puck.velocity = Vector2.zero;
                _spawnedPuck.puck.position = new Vector2(-20, -20);
                playerScore += 1;
                _PscoreText.text = playerScore.ToString();
                _spawnedPuck.playerGoal = false;
                Debug.Log("LADUMA");
                _resetPoint = true;
            }

            if (_spawnedPuck.enemyGoal)
            {
                t = 0f;
                _spawnedPuck.puck.velocity = Vector2.zero;
                _spawnedPuck.puck.position = new Vector2(-20, -20);
                enemyScore += 1;
                _EscoreText.text = enemyScore.ToString();
                _spawnedPuck.enemyGoal = false;
                Debug.Log("LADUMA");
                _resetPoint = true;
            }
        }
        else
        {
            _ResetText.gameObject.SetActive(false);
            _Countdown.gameObject.SetActive(false);
        }
    }

    private void SetPortalPos(Portal p)
    {
        int xory = Random.Range(0, 2);
        int upordown = 0;
        while (upordown == 0)
        {
            upordown = Random.Range(-1, 2);
        }

        int lorr = 0;
        while (lorr == 0)
        {
            lorr = Random.Range(-1, 2);
        }

        if (xory == 0)
        {
            p.transform.rotation = Quaternion.Euler(0, 0, -1 * upordown * 90);
            p.transform.position = new Vector3(Random.Range(-7.5f, 7.51f), upordown * 4.73f, 0f);
        }
        else
        {
            if (lorr > 0)
            {
                p.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else
            {
                p.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            p.transform.position = new Vector3(lorr * 8.68f, upordown * Random.Range(2f, 3.6f), 0f);
        }
    }

    private void Foul(bool whofoul)
    {
        if (whofoul)
        {
            if (playerScore > 0)
            {
                playerScore -= 1;
                _PscoreText.text = playerScore.ToString();
            }
        }
        else if (enemyScore > 0)
        {
            enemyScore -= 1;
            _EscoreText.text = playerScore.ToString();
        }
        
    }
}