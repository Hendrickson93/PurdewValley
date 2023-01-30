using UnityEngine;

namespace EdgarDev.NPCTool
{
	public class NPC : MonoBehaviour
	{
		public string _NPCName;

		protected Animator _animator;

		protected bool _hasAnimator;

		// animation IDs
		protected int _animIDSpeed;
		protected int _animIDGrounded;
		protected int _animIDJump;
		protected int _animIDFreeFall;
		protected int _animIDMotionSpeed;

		public virtual void Start()
		{
			_hasAnimator = TryGetComponent(out _animator);

			AssignAnimationIDs();
		}

		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreeFall = Animator.StringToHash("FreeFall");
			_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
		}
	}
}