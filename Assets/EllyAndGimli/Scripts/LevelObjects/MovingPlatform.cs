using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Маяки (child-объекты для удобства)")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Параметры движения")]
    public float speed = 2f;
    public bool pingPong = true;
    public bool moveOnStart = false;

    // кешированные мировые позиции
    private Vector3 startPos;
    private Vector3 endPos;

    private bool moving  = false;
    private bool forward = true;

    private void Awake()
    {
        // 1) запоминаем мировые координаты
        startPos = startPoint.position;
        endPos   = endPoint.position;

        // 2) (опц.) отлепляем маяки от платформы, чтобы они не смещались
        startPoint.SetParent(null, true);
        endPoint.SetParent(null, true);
    }

    private void Start() { if (moveOnStart) moving = true; }

    private void Update()
    {
        if (!moving)
            return;

        var target = forward ? endPos : startPos;
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (!(Vector3.Distance(transform.position, target) < 0.001f)) 
            return;
        
        if (pingPong)
            forward = !forward;
        else
            moving = false;
    }

    public void StartMoving() => moving = true;
    public void StopMoving()  => moving = false;
}