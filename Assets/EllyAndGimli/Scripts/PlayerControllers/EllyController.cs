using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EllyController : PlayerControllerBase
{
	private bool _canMakeSecondJump = false;

	protected override void Start()
	{
		base.Start();
		_inputSystemActions.Elly.Jump.started += context => TryDoJump(_isGrounded || _canMakeSecondJump);
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void FixedUpdate()
	{
		var movement = _inputSystemActions.Elly.Move.ReadValue<Vector2>();
		DoMove(movement.x);
		base.FixedUpdate();
	}

	protected override bool TryDoJump(bool jumpingCondition)
	{
		var hasJumped = base.TryDoJump(jumpingCondition);
		if (hasJumped)
		{
			StartCoroutine(nameof(JumpCoroutine));
		}
		return hasJumped;
	}

	private IEnumerator JumpCoroutine()
	{
		_canMakeSecondJump = true;
		yield return new WaitForSeconds(0.5f);
		_canMakeSecondJump = false;
	}
}