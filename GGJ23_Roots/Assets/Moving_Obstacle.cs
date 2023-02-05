using UnityEngine;

public class Moving_Obstacle : MonoBehaviour
{
    [SerializeField]
    private bool _isMoving, MoveX, MoveY, eSpin;
    [Header("IF MoveX or MoveY is on this cant be 0")]
    [SerializeField]
    private Vector2 moving_dist;
    private Vector2 moving_force;
    [SerializeField]
    private float spin_range;
    [Header("small values < 1")]
    [SerializeField]
    private float spin_speed;
    private float angle;
    private void Start()
    {
        moving_force = new Vector2(0,0);
    }
    private void FixedUpdate()
    {
        angle += spin_speed;
        if (_isMoving)
        {
            if(MoveX)
                moving_force.x = Mathf.PingPong(Time.time / 0.5f, moving_dist.x) - moving_dist.x/2;
            if(MoveY)
                moving_force.y = Mathf.PingPong(Time.time / 0.5f, moving_dist.y) - moving_dist.y/2;

            transform.localPosition = new Vector3(moving_force.x, moving_force.y, 0);
        }
        if(eSpin)
        {
            transform.localPosition = new Vector3(Mathf.Cos(angle)* spin_range, Mathf.Sin(angle)* spin_range, 0);
        }
    }
}
