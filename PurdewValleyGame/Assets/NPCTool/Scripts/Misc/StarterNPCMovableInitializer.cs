using UnityEngine;

/* initialize every component that a empty npc needs */

namespace EdgarDev.NPCTool.Utils
{
	[ExecuteInEditMode]
	public class StarterNPCMovableInitializer : MonoBehaviour
	{
		private void Start()
		{
			GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			capsule.transform.position += Vector3.up;
			capsule.transform.SetParent(transform);

			CapsuleCollider collider = capsule.GetComponent<CapsuleCollider>();

			CharacterController characterController = gameObject.AddComponent<CharacterController>();
			characterController.slopeLimit = 45;
			characterController.stepOffset = 0.25f;
			characterController.skinWidth = 0.02f;
			characterController.minMoveDistance = 0;
			characterController.center = new Vector3(0, collider.bounds.max.y, 0);
			characterController.radius = collider.radius;
			characterController.height = collider.height;

			NPC npc = gameObject.AddComponent<NPC>();
			NPCMovable movable = gameObject.AddComponent<NPCMovable>();
			StarterNPCMotor motor = gameObject.AddComponent<StarterNPCMotor>();

			if (movable.m_MoveEvent == null)
				movable.m_MoveEvent = new NPCMoveEvent();

#if UNITY_EDITOR
			UnityEditor.Events.UnityEventTools.AddPersistentListener(movable.m_MoveEvent, motor.Move);
#endif			

			DestroyImmediate(this);
		}
	}
}
