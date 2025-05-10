using Assets.EllyAndGimli.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerControllerBase : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] protected float _jumpForce = 10f;

    [SerializeField] protected Transform _groundCheck;
    [SerializeField] protected float _groundCheckRadius = 0.2f;
    [SerializeField] protected List<LayerMask> _bottomCollisionLayers;

    protected Rigidbody2D _rb;
    protected Collider2D _collider;
    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;
    protected int _layerMask;
    protected int _slopeLayer;
    protected InputSystem_Actions _inputSystemActions;

    protected bool _isGrounded;
    protected bool _onSlope;
    protected bool _isDead;

	protected readonly Func<Vector2, float, int, bool> _isOverlapping = (Vector2 position, float radius, int layerMask)
		=> Physics2D.OverlapCircle(position, radius, layerMask) != null;

	protected void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _inputSystemActions = new InputSystem_Actions();
    }

	protected virtual void Start()
	{
        var layerNames = _bottomCollisionLayers.Select(l => LayerMask.LayerToName((int)Mathf.Log(l.value, 2))).ToArray();
        _layerMask = LayerMask.GetMask(layerNames);
        _slopeLayer = LayerMask.GetMask("Slope");
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
        _isGrounded = CheckGroundLayer(_layerMask);
        _onSlope = CheckGroundLayer(_slopeLayer);
    }

    private bool CheckGroundLayer(int layerMask)
    {
        const float boxHeight = 0.1f;
        var size = new Vector2(_collider.bounds.size.x, boxHeight);

        var hit = Physics2D.BoxCast(_groundCheck.position,
            size,
            0f,
            Vector2.down,
            0.05f,
            layerMask);
        
        return hit.collider != null;
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
		if (movementDirection > 0 && !_spriteRenderer.flipX) _spriteRenderer.flipX = true;
		else if (movementDirection < 0 && _spriteRenderer.flipX) _spriteRenderer.flipX = false;
		if (_onSlope)
            return;
        _rb.linearVelocity = new Vector2(movementDirection * _moveSpeed, _rb.linearVelocity.y);
		_animator.SetBool(AnimationsConst.IsWalking, movementDirection != 0);
	}
    
    public virtual void Die()
    {
        if (_isDead) return;
        _isDead = true;
        _animator.SetTrigger(AnimationsConst.Die);
        _inputSystemActions.Disable();

        _rb.linearVelocity = Vector2.zero;
        _rb.simulated = false;

        StartCoroutine(WaitDeathAnim());
    }
    
    public virtual IEnumerator WaitDeathAnim(float seconds = 1.1f)
    {
        yield return new WaitForSeconds(seconds);
        LevelManager.Instance.RestartLevel();
    }
}
