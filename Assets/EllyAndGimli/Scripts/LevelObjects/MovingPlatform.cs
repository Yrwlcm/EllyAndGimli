using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Точки-маяки (child-объекты для удобства)")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Параметры")]
    public float speed      = 2f;
    public bool  pingPong   = true;
    public bool  moveOnStart = false;

    // ----------------------------------------------------

    private Vector3 startPos;
    private Vector3 endPos;

    private enum State { Idle, Forward, Backward }
    private State state = State.Idle;

    private bool buttonDown   = false;   // стоит ли игрок на кнопке
    private bool forwardFlag  = true;    // куда «должны» ехать дальше (нужно для ping-pong)

    // ----------------------------------------------------

    void Awake()
    {
        startPos = startPoint.position;
        endPos   = endPoint.position;

        // отвязываем маяки, чтобы они не ездили вместе с платформой
        startPoint.SetParent(null, true);
        endPoint.SetParent(null,  true);
    }

    void Start()
    {
        if (moveOnStart) StartMoving();
    }

    void Update()
    {
        float step = speed * Time.deltaTime;

        switch (state)
        {
            case State.Forward:
                MoveTowards(endPos, step);
                if (Arrived(endPos))
                {
                    forwardFlag = false;              // теперь «следующее» направление — назад
                    if (pingPong && buttonDown)       state = State.Backward;
                    else                              state = State.Idle;
                }
                break;

            case State.Backward:
                MoveTowards(startPos, step);
                if (Arrived(startPos))
                {
                    forwardFlag = true;               // «следующее» направление — вперёд
                    if (pingPong && buttonDown)       state = State.Forward;
                    else                              state = State.Idle;
                }
                break;
        }
    }

    // ----------------------------------------------------
    // публичные вызовы «кнопки»

    public void StartMoving()          // кнопка НАЖАТА
    {
        buttonDown = true;

        if (pingPong)
        {
            if (state == State.Idle)                       // продолжить путь
                state = forwardFlag ? State.Forward : State.Backward;
        }
        else
        {
            state = State.Forward;                         // всегда к endPos
        }
    }

    public void StopMoving()           // кнопка ОТПУЩЕНА
    {
        buttonDown = false;

        if (pingPong)
        {
            state = State.Idle;                            // просто пауза
        }
        else
        {
            state = State.Backward;                        // возврат к startPos
        }
    }

    // ----------------------------------------------------
    // вспомогательные

    private void MoveTowards(Vector3 target, float step) =>
        transform.position = Vector3.MoveTowards(transform.position, target, step);

    private static bool Arrived(Vector3 a, Vector3 b) =>
        Vector3.SqrMagnitude(a - b) < 1e-6f;

    private bool Arrived(Vector3 target) => Arrived(transform.position, target);
}
