using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public Rigidbody2D enemy;
    [SerializeField] public Rigidbody2D puck;
    [SerializeField] public Puck puckclass;
    [SerializeField] float maxVelocity;
    private readonly Vector2 _readyDefensePos = new Vector2(-6, 0);
    private bool _ballStuckMySide = false;
    private float _ballMySideTimer = 0f;
    private Vector2 _target;
    private readonly Vector2 _rightGoal = new Vector2(8,0);
    private readonly Vector2 _leftGoal = new Vector2(-8,0);
    private readonly Vector2 _nullVector = new Vector2(-10f, -20f);
    private bool _moveToTargetState = true;
    private bool _shootState = false;
    
    private void FixedUpdate()
    {
        if (puckclass.lastHitBy != 1)
        {
            if (Vector2.Distance(enemy.position, _leftGoal) > 4f)
            {
                var dir = (enemy.position - _leftGoal).normalized;
                enemy.position = _leftGoal + dir * 4f;
            }

            if (enemy.position.x < -8f)
            {
                enemy.position = new Vector2(-8f, enemy.position.y);
            }

            if (_moveToTargetState)
            {

                _target = AcquireTarget();
                if (_target != _nullVector)
                {
                    if (Vector2.Distance(enemy.position, _target) > 0.25f)
                    {
                        enemy.velocity = (_target - enemy.position).normalized * maxVelocity;
                    }
                    else
                    {
                        _moveToTargetState = false;
                        _shootState = true;
                    }
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, _readyDefensePos, maxVelocity * Time.deltaTime);
                }

            }

            if (_shootState)
            {
                _target = puck.position + (puck.velocity.normalized * -2);
                if (Vector2.Distance(enemy.position, _target) > 0.25f)
                {
                    enemy.velocity = (_target - enemy.position).normalized * maxVelocity;
                }
                else
                {
                    _moveToTargetState = true;
                    _shootState = false;
                }
            }
        }
        else if(_ballStuckMySide ==false)
        {
            enemy.position = Vector2.MoveTowards(enemy.position, _readyDefensePos, maxVelocity * Time.deltaTime);
        }

        if (puck.position.x < -1f && puckclass.lastHitBy == 1)
        {
            _ballMySideTimer += Time.deltaTime;
        }
        else
        {
            _ballMySideTimer = 0;
        }

        if (_ballMySideTimer >= 3f)
        {
            _ballStuckMySide = true;
        }

        if (Vector2.Distance(puck.position, _leftGoal) > 5f)
        {
            _ballStuckMySide = false;
        }

        if (_ballStuckMySide)
        {
            var position = puck.position;
            _target = (position - enemy.position).normalized + position;
            if (Vector2.Distance(_target, _leftGoal) < 5f)
            {
                enemy.velocity = (_target - enemy.position).normalized * maxVelocity;
            }
        }
    }

    private Vector2 AcquireTarget()
    {
        var puckPos = puck.position;
        var nullVector = new Vector2(-10f, -20f);
        var puckFuturePos = puckPos + puck.velocity * 0.25f;
        if (puckFuturePos.x > 0f)
        {
            return nullVector;
        }
        else
        {
            puckFuturePos = puckFuturePos + (puckFuturePos - _rightGoal).normalized;
            if (Vector2.Distance(puckFuturePos, _leftGoal) > 4f)
            {
                return nullVector;  
            }
            else
            {
                if (puckFuturePos.x < -8f)
                {
                    puckFuturePos.x = -8f;
                }
                return puckFuturePos;
            }
        }
    }
}