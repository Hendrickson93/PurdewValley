using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EdgarDev.NPCTool
{
	[System.Serializable]
	public class NPCMoveEvent : UnityEvent<Vector2>
	{
	}

	public enum LoopMode { None, ContinueMode, ReverseMode }

	public class NPCMovable : NPC
	{
		private enum NPCMovableState { Idle, Moving }
		private const float closeDistance = 0.1f;

		[Header("Movement")]
		[Tooltip("Move speed of the npc in m/s")]
		public float m_MoveSpeed = 2.0f;
		[Tooltip("Sprint speed of the npc in m/s")]
		public float m_SprintSpeed = 5.335f;
		[Tooltip("How fast the npc turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float m_RotationSmoothTime = 0.12f;
		[Tooltip("Acceleration and deceleration")]
		public float m_SpeedChangeRate = 10.0f;
		[Tooltip("Where you want your npc to move on")]
		public Transform[] m_Pathpoints = new Transform[0];
		[Tooltip("How you want your npc to behave at the end of the path")]
		public LoopMode m_LoopMode = LoopMode.None;
		[Tooltip("Set the amount of loop that you want (0 for infinite)")]
		public int m_NumberOfLoops = 0;

		public NPCMoveEvent m_MoveEvent;

		private NPCMovableState m_State;
		private List<Vector3> m_PathList;
		private Vector3 m_TargetPosition;
		private int m_PathpointIndex;

		private int m_LoopIteration = 0;
		private bool m_IsReverse = false;

		public override void Start()
		{
			base.Start();

			InitPathList();
			m_State = NPCMovableState.Moving;
		}

		private void InitPathList()
		{
			// store path points values in a list

			// initialize list and add first element to start position
			m_PathList = new List<Vector3>();
			m_PathList.Add(transform.position);

			foreach (Transform point in m_Pathpoints)
			{
				if (point.childCount == 0)
				{
					m_PathList.Add(point.position);
				}
				else
				{
					int rand = Random.Range(0, point.childCount);
					m_PathList.Add(point.GetChild(rand).position);
				}
			}

			SetPathpointIndexAndTarget(0);
		}

		private void Update()
		{
			switch (m_State)
			{
				// represent the actions of character on Idle state
				case NPCMovableState.Idle:
					Idle();
					break;
				// represent the actions of character on Moving state
				case NPCMovableState.Moving:
					Move();
					break;
			}
		}

		private void Move()
		{
			CheckDistance(transform.position);
			Vector2 move = new Vector2(m_TargetPosition.x - transform.position.x, m_TargetPosition.z - transform.position.z);
			m_MoveEvent.Invoke(move);
		}

		private void Idle()
		{
			m_MoveEvent.Invoke(Vector2.zero);
		}

		public void CheckDistance(Vector3 p1)
		{
			float distance = (p1 - m_TargetPosition).sqrMagnitude;
			if (distance < closeDistance * closeDistance)
			{
				SetTargetDispatcher();
			}
		}

		private void SetTargetDispatcher()
		{
			// npc keep going on the path
			if (m_PathpointIndex < m_PathList.Count - 1)
			{
				bool hasFinishedPath = KeepOnThePath();
				if (!hasFinishedPath) return;
			}

			// npc has finished the path

			// check is loop then restart from pathpoint index 0
			// else switch to idle state
			if (m_LoopMode == LoopMode.ContinueMode && (m_LoopIteration < m_NumberOfLoops || m_NumberOfLoops == 0))
			{
				SetPathpointIndexAndTarget(0);

				m_LoopIteration++;
			}
			else if (m_LoopMode == LoopMode.ReverseMode && (m_LoopIteration < m_NumberOfLoops || m_NumberOfLoops == 0))
			{
				m_IsReverse = !m_IsReverse;
				SetPathpointIndexAndTarget(m_PathpointIndex + (m_IsReverse ? -1 : 1));

				if (m_IsReverse == false) m_LoopIteration++;
			}
			else
			{
				m_TargetPosition = transform.position;
				m_State = NPCMovableState.Idle;
			}
		}

		private bool KeepOnThePath()
		{
			bool hasFinished = false;

			// check if the loop mode is reverse and direction is reversed
			if (m_LoopMode == LoopMode.ReverseMode && m_IsReverse)
			{
				// check if npc has not reversly finished the path
				if (m_PathpointIndex > 0)
				{
					SetPathpointIndexAndTarget(m_PathpointIndex - 1);
				}
				else
				{
					hasFinished = true;
				}
			}
			else
			{
				SetPathpointIndexAndTarget(m_PathpointIndex + 1);
			}

			return hasFinished;
		}

		private void SetPathpointIndexAndTarget(int index)
		{
			// set index and new target
			m_PathpointIndex = index;
			m_TargetPosition = m_PathList[m_PathpointIndex];

			/*Debug.Log("index is :" + m_PathpointIndex);*/
		}
	}
}