using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 _target;

    private void Start()
    {
        _target = pointB.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _target) < 0.05f)
        {
            _target = _target == pointA.position ? pointB.position : pointA.position;
        }
    }
}
