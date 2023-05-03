using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerMove : MonoBehaviour
{
    public Vector2 mousePos;
    public Rigidbody2D playerPuck;
    private float _distanceBetween;
    [SerializeField]
    private float maxPlayerX= 8.05f;
    private Vector2 _goalToPlayer;
    private readonly Vector2 _playerGoal = new Vector2(9,0);
    [SerializeField] private float radius;
    private void FixedUpdate()
    {
        if (Camera.main != null) mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _distanceBetween = Vector2.Distance(_playerGoal,mousePos);
        if((_distanceBetween<radius)&&(mousePos.x<maxPlayerX))
        {
            playerPuck.MovePosition(mousePos);
        }

        if ((_distanceBetween >= radius)&&(mousePos.x<maxPlayerX))
        {
            _goalToPlayer = _playerGoal - mousePos;
            _goalToPlayer.Normalize();
            
            playerPuck.MovePosition(_playerGoal - (_goalToPlayer * radius));
        }

        if (mousePos.x>8.05f)
        {
            playerPuck.MovePosition(new Vector2(maxPlayerX,mousePos.y));
        }
    }
}
