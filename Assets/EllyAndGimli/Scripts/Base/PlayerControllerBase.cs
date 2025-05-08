using Assets.EllyAndGimli.Constants;
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
    protected SpriteRenderer _spriteRenderer;
    protected int _layerMask;
    protected InputSystem_Actions _inputSystemActions;
    protected Animator _animator;

    protected bool _isGrounded;

    protected void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
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
            _animator.SetTrigger(AnimationsConst.Jump);
            return true;
        }
        return false;
    }

    protected virtual void DoMove(float movementDirection)
    {
        _animator.SetBool(AnimationsConst.IsWalking, movementDirection != 0);
        if (movementDirection > 0 && !_spriteRenderer.flipX) _spriteRenderer.flipX = true;
        else if (movementDirection < 0 && _spriteRenderer.flipX) _spriteRenderer.flipX = false;
        _rb.linearVelocity = new Vector2(movementDirection * _moveSpeed, _rb.linearVelocity.y);
    }
}
