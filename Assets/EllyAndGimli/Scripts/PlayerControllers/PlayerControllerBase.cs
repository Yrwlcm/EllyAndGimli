using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerControllerBase : MonoBehaviour
{
    [Header("������� ��������")]
    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] protected float _jumpForce = 10f;

    [Header("�������� � ��������� ��� ����������")]
    [SerializeField] protected Transform _groundCheck;
    [SerializeField] protected float _groundCheckRadius = 0.2f;
    [SerializeField] protected List<LayerMask> _bottomCollisionLayers;

    protected Rigidbody2D _rb;
    protected Collider2D _collider;
    protected int _layerMask;
    protected int _slopeLayer;
    protected InputSystem_Actions _inputSystemActions;

    protected bool _isGrounded;
    protected bool _onSlope;
    protected bool _isDead;

    protected void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
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
            return true;
        }
        return false;
    }

    protected virtual void DoMove(float movementDirection)
    {
        if (_onSlope)
            return;
        
        _rb.linearVelocity = new Vector2(movementDirection * _moveSpeed, _rb.linearVelocity.y);
    }
    
    public virtual void Die()
    {
        if (_isDead) return;
        _isDead = true;

        _inputSystemActions.Disable();

        _rb.linearVelocity = Vector2.zero;
        _rb.simulated = false;

        StartCoroutine(WaitDeathAnim());
    }
    
    public virtual IEnumerator WaitDeathAnim(int seconds = 1)
    {
        yield return new WaitForSeconds(seconds);
        LevelManager.Instance.RestartLevel();
    }
}
