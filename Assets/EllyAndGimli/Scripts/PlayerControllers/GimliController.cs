using UnityEngine;

public class GimliController : PlayerControllerBase
{
	protected override void Update()
	{
		base.Update();
	}

	protected override void FixedUpdate()
	{
		var movement = _inputSystemActions.Gimli.Move.ReadValue<Vector2>();
		var isJumpPressed = _inputSystemActions.Gimli.Jump.IsPressed();
		DoMove(movement.x);
		TryDoJump(isJumpPressed && _isGrounded);
		base.FixedUpdate();
	}
}
