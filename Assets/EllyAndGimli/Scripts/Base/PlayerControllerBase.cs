using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerControllerBase : MonoBehaviour
{
    [Header("Главные свойства")]
    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] protected float _jumpForce = 10f;

    [Header("Коллизии с объектами под персонажем")]
    [SerializeField] protected Transform _groundCheck;
    [SerializeField] protected float _groundCheckRadius = 0.2f;
    [SerializeField] protected List<LayerMask> _bottomCollisionLayers;

    protected Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    protected int _layerMask;
    protected InputSystem_Actions _inputSystemActions;

    protected bool _isGrounded;
    protected bool _isDead;

    private readonly float _revivingTime = 1f;

    public delegate void Dead();
    public delegate void Revival();
    public event Revival OnRevival;
    public event Dead OnDead;

    protected void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inputSystemActions = new InputSystem_Actions();
    }

	protected virtual void Start()
	{
        var layerNames = _bottomCollisionLayers.Select(l => LayerMask.LayerToName((int)Mathf.Log(l.value, 2))).ToArray();
        _layerMask = LayerMask.GetMask(layerNames);
	}

	protected virtual void OnEnable()
	{
        _inputSystemActions.Enable();
	}

	protected virtual void OnDisable()
	{
        _inputSystemActions.Disable();
	}

    protected virtual void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _layerMask) != null;
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual bool TryDoJump(bool jumpingCondition)
    {
        if (jumpingCondition)
        {
            _rb.linearVelocity = Vector2.up * _jumpForce;
            return true;
        }
        return false;
    }

    protected virtual void DoMove(float movementDirection)
    {
        _rb.linearVelocity = new Vector2(movementDirection * _moveSpeed, _rb.linearVelocity.y);
    }

    public virtual void Die()
    {
        if (!_isDead)
            StartCoroutine(nameof(RevivalCoroutine));
    }

    private IEnumerator RevivalCoroutine()
    {
        _isDead = true;
        _inputSystemActions.Disable();
        _spriteRenderer.enabled = false;
        _rb.Sleep();
        OnDead();
        yield return new WaitForSeconds(_revivingTime);
        OnRevival();
        _inputSystemActions.Enable();
        _spriteRenderer.enabled = true;
        _rb.WakeUp();
        _isDead = false;
	}
}
