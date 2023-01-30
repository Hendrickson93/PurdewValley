using EdgarDev.NPCTool;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class StarterNPCMotor : MonoBehaviour
{
	[Header("Movement")]
	[Tooltip("Move speed of the character in m/s")]
	public float m_MoveSpeed = 2.0f;
	[Tooltip("How fast the character turns to face movement direction")]
	[Range(0.0f, 0.3f)]
	private float m_RotationSmoothTime = 0.12f;
	[Tooltip("Acceleration and deceleration")]
	private float m_SpeedChangeRate = 10.0f;

	private CharacterController m_Controller;

	private float _speed;
	private float _animationBlend;
	private float _targetRotation = 0.0f;
	private float _rotationVelocity;
	private float _verticalVelocity;

	private void Awake()
	{
		m_Controller = GetComponent<CharacterController>();
	}

	public void Move(Vector2 move)
	{
		// set target speed based on move speed, sprint speed and if sprint is pressed
		float targetSpeed = m_MoveSpeed;

		// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

		// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is no input, set the target speed to 0
		if (move == Vector2.zero) targetSpeed = 0.0f;

		// a reference to the players current horizontal velocity
		float currentHorizontalSpeed = new Vector3(m_Controller.velocity.x, 0.0f, m_Controller.velocity.z).magnitude;

		float speedOffset = 0.1f;

		// accelerate or decelerate to target speed
		if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
		{
			// creates curved result rather than a linear one giving a more organic speed change
			// note T in Lerp is clamped, so we don't need to clamp our speed
			_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * m_SpeedChangeRate);

			// round speed to 3 decimal places
			_speed = Mathf.Round(_speed * 1000f) / 1000f;
		}
		else
		{
			_speed = targetSpeed;
		}
		_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * m_SpeedChangeRate);

		// normalise input direction
		Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

		if (move != Vector2.zero)
		{
			_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, m_RotationSmoothTime);

			// rotate to face input direction relative to camera position
			transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
		}

		Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

		// move the npc
		m_Controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
	}
}
