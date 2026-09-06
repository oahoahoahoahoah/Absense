using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200002B RID: 43
public static class PullSystem
{
	// Token: 0x0600013C RID: 316 RVA: 0x00017218 File Offset: 0x00015418
	public static void ApplyPSA()
	{
		bool flag = !Settings.PSAEnabled;
		if (!flag)
		{
			try
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag2 = !(instance == null) && !(instance.bodyCollider == null) && Settings.IsPressed(Settings.PSAKeybind);
				if (flag2)
				{
					Transform transform = instance.transform;
					transform.position += instance.bodyCollider.transform.forward * Settings.PSASpeed * Time.deltaTime * instance.scale;
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x0600013D RID: 317 RVA: 0x000172CC File Offset: 0x000154CC
	public static void ApplyVeloPSA()
	{
		bool flag = !Settings.VeloPSAEnabled;
		if (!flag)
		{
			try
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag2 = instance == null || instance.bodyCollider == null;
				if (!flag2)
				{
					bool flag3 = !Settings.IsPressed(Settings.VeloPSAKeybind);
					if (flag3)
					{
						PullSystem.veloStickyDir = Vector3.zero;
					}
					else
					{
						Rigidbody attachedRigidbody = instance.bodyCollider.attachedRigidbody;
						Vector3 val = ((attachedRigidbody != null) ? attachedRigidbody.linearVelocity : instance.AveragedVelocity);
						Vector3 val2 = default(Vector3);
						val2..ctor(val.x, 0f, val.z);
						float veloPSAMinSpeed = Settings.VeloPSAMinSpeed;
						bool flag4 = val2.sqrMagnitude >= veloPSAMinSpeed * veloPSAMinSpeed;
						Vector3 val4;
						if (flag4)
						{
							Vector3 val3 = default(Vector3);
							val3..ctor(val.x, val.y * 0.35f, val.z);
							bool flag5 = val3.sqrMagnitude < 0.0001f;
							if (flag5)
							{
								return;
							}
							val4 = (PullSystem.veloStickyDir = val3.normalized);
						}
						else
						{
							bool flag6 = !Settings.VeloPSAStickyDirection || PullSystem.veloStickyDir == Vector3.zero;
							if (flag6)
							{
								return;
							}
							val4 = PullSystem.veloStickyDir;
						}
						bool flag7 = Settings.VeloPSAAirTurnAssist && attachedRigidbody != null && Mathf.Abs(val.y) > 0.08f;
						if (flag7)
						{
							Vector3 forward = instance.bodyCollider.transform.forward;
							forward.y = 0f;
							bool flag8 = forward.sqrMagnitude > 0.0001f;
							if (flag8)
							{
								float num = Mathf.Clamp01(Settings.VeloPSAAirTurnBlend);
								val4 = Vector3.Slerp(val4.normalized, forward.normalized, num).normalized;
							}
						}
						Transform transform = instance.transform;
						transform.position += val4 * Settings.VeloPSASpeed * Time.deltaTime * instance.scale;
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0001750C File Offset: 0x0001570C
	public static void ApplyBurstPSA()
	{
		bool flag = !Settings.BurstPSAEnabled;
		if (!flag)
		{
			try
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag2 = instance == null || instance.bodyCollider == null;
				if (!flag2)
				{
					bool flag3 = !Settings.IsPressed(Settings.BurstPSAKeybind);
					if (flag3)
					{
						PullSystem.veloTurnActive = false;
						PullSystem.burstStickyDir = Vector3.zero;
						PullSystem.burstActive = instance.LeftHand.wasColliding;
						PullSystem.burstDirLocked = instance.RightHand.wasColliding;
					}
					else
					{
						bool wasColliding = instance.LeftHand.wasColliding;
						bool wasColliding2 = instance.RightHand.wasColliding;
						bool flag4 = ((wasColliding && !PullSystem.burstActive) || (wasColliding2 && !PullSystem.burstDirLocked)) && !PullSystem.veloTurnActive;
						if (flag4)
						{
							PullSystem.veloTurnActive = true;
							PullSystem.burstTimer = Settings.BurstPSADuration / 1000f;
						}
						PullSystem.burstActive = wasColliding;
						PullSystem.burstDirLocked = wasColliding2;
						bool flag5 = !PullSystem.veloTurnActive;
						if (!flag5)
						{
							PullSystem.burstTimer -= Time.deltaTime;
							bool flag6 = PullSystem.burstTimer <= 0f;
							if (flag6)
							{
								PullSystem.veloTurnActive = false;
								PullSystem.burstStickyDir = Vector3.zero;
							}
							else
							{
								Rigidbody attachedRigidbody = instance.bodyCollider.attachedRigidbody;
								Vector3 val = ((attachedRigidbody != null) ? attachedRigidbody.linearVelocity : instance.AveragedVelocity);
								Vector3 val2 = default(Vector3);
								val2..ctor(val.x, 0f, val.z);
								float burstPSAMinSpeed = Settings.BurstPSAMinSpeed;
								bool flag7 = val2.sqrMagnitude >= burstPSAMinSpeed * burstPSAMinSpeed;
								Vector3 val4;
								if (flag7)
								{
									Vector3 val3 = default(Vector3);
									val3..ctor(val.x, val.y * 0.35f, val.z);
									bool flag8 = val3.sqrMagnitude < 0.0001f;
									if (flag8)
									{
										return;
									}
									val4 = (PullSystem.burstStickyDir = val3.normalized);
								}
								else
								{
									bool flag9 = !Settings.BurstPSAStickyDirection || PullSystem.burstStickyDir == Vector3.zero;
									if (flag9)
									{
										return;
									}
									val4 = PullSystem.burstStickyDir;
								}
								bool flag10 = Settings.BurstPSAAirTurnAssist && attachedRigidbody != null && Mathf.Abs(val.y) > 0.08f;
								if (flag10)
								{
									Vector3 forward = instance.bodyCollider.transform.forward;
									forward.y = 0f;
									bool flag11 = forward.sqrMagnitude > 0.0001f;
									if (flag11)
									{
										float num = Mathf.Clamp01(Settings.BurstPSAAirTurnBlend);
										val4 = Vector3.Slerp(val4.normalized, forward.normalized, num).normalized;
									}
								}
								Transform transform = instance.transform;
								transform.position += val4 * Settings.BurstPSASpeed * Time.deltaTime * instance.scale;
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00017830 File Offset: 0x00015A30
	public static void ApplyPSASkidded()
	{
		bool flag4 = !Settings.PSASkiddedEnabled;
		if (!flag4)
		{
			try
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag5 = instance == null || instance.bodyCollider == null;
				if (!flag5)
				{
					bool flag = Settings.IsPressed(Settings.PSASkiddedKeybind);
					bool flag2 = Settings.IsPressed(Settings.PSASkiddedBackwardKeybind);
					bool flag3 = flag || flag2;
					bool flag6 = Settings.PSASkiddedFakeGlitchwalk && flag;
					if (flag6)
					{
						Transform transform = instance.transform;
						transform.position += instance.bodyCollider.transform.right * Time.deltaTime;
					}
					bool flag7 = Settings.PSASkiddedRecRoom && flag3;
					if (flag7)
					{
						Vector3 val = PullSystem.GetRecRoomDirection();
						Transform transform2 = instance.transform;
						transform2.position += val * Settings.PSASkiddedRecRoomSpeed * Time.deltaTime;
					}
					bool flag8 = flag3;
					if (flag8)
					{
						bool flag9 = Settings.PSASkiddedGravityStrength > 0f;
						if (flag9)
						{
							Transform transform3 = instance.transform;
							transform3.position += Vector3.down * Settings.PSASkiddedGravityStrength * Time.deltaTime;
						}
						Vector3 val2 = PullSystem.GetSkiddedDirection();
						bool flag10 = flag2 && !flag;
						if (flag10)
						{
							val2 = -val2;
						}
						Transform transform4 = instance.transform;
						transform4.position += val2 * Settings.PSASkiddedSpeed * Time.deltaTime;
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x06000140 RID: 320 RVA: 0x000179F0 File Offset: 0x00015BF0
	private static Vector3 GetSkiddedDirection()
	{
		GTPlayer val2 = GTPlayer.Instance;
		bool flag = val2 == null;
		Vector3 vector;
		if (flag)
		{
			vector = Vector3.forward;
		}
		else
		{
			bool flag2 = val2.bodyCollider == null;
			if (flag2)
			{
				vector = Vector3.forward;
			}
			else
			{
				Transform val3 = val2.bodyCollider.transform;
				switch (Settings.PSASkiddedDirection)
				{
				case MoveDirection.Forward:
					vector = val3.forward;
					break;
				case MoveDirection.Backward:
					vector = -val3.forward;
					break;
				case MoveDirection.Left:
					vector = -val3.right;
					break;
				case MoveDirection.Right:
					vector = val3.right;
					break;
				default:
					vector = Vector3.forward;
					break;
				}
			}
		}
		return vector;
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00017AA4 File Offset: 0x00015CA4
	private static Vector3 GetRecRoomDirection()
	{
		Vector3 val = default(Vector3);
		Vector3 val2 = default(Vector3);
		GTPlayer val3 = GTPlayer.Instance;
		bool flag = !(val3 == null);
		Vector3 vector;
		if (flag)
		{
			bool flag2 = !(val3.bodyCollider == null);
			if (flag2)
			{
				Transform transform = val3.bodyCollider.transform;
				val = transform.forward;
				val2 = transform.right;
				switch (Settings.PSASkiddedRecRoomDirection)
				{
				case DiagonalDirection.LeftDiagonal:
					vector = (val - val2).normalized;
					break;
				case DiagonalDirection.RightDiagonal:
					vector = (val + val2).normalized;
					break;
				case DiagonalDirection.BLeftDiagonal:
					vector = (-val - val2).normalized;
					break;
				case DiagonalDirection.BRightDiagonal:
					vector = (-val + val2).normalized;
					break;
				default:
					vector = Vector3.forward;
					break;
				}
			}
			else
			{
				vector = Vector3.forward;
			}
		}
		else
		{
			vector = Vector3.forward;
		}
		return vector;
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00017BB4 File Offset: 0x00015DB4
	public static void ApplyHighJump()
	{
		bool flag = !Settings.HighJumpEnabled;
		if (!flag)
		{
			try
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag2 = !(instance == null) && !(instance.bodyCollider == null) && Settings.IsPressed(Settings.HighJumpKeybind);
				if (flag2)
				{
					Transform transform = instance.transform;
					transform.position += instance.bodyCollider.transform.up * Settings.HighJumpSpeed * Time.deltaTime;
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00017C58 File Offset: 0x00015E58
	public static void ApplyRecRoom()
	{
		bool flag = !Settings.RecRoomEnabled;
		if (!flag)
		{
			try
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag2 = !(instance == null) && !(instance.bodyCollider == null);
				if (flag2)
				{
					bool flag3 = Settings.IsPressed(Settings.RecRoomForwardKeybind);
					if (flag3)
					{
						Transform transform = instance.transform;
						transform.position += instance.bodyCollider.transform.forward * Settings.RecRoomSpeed * Time.deltaTime * instance.scale;
						Transform transform2 = instance.transform;
						transform2.position += instance.bodyCollider.transform.right * Settings.RecRoomSpeed * Time.deltaTime * instance.scale;
					}
					bool flag4 = Settings.IsPressed(Settings.RecRoomBackwardKeybind);
					if (flag4)
					{
						Transform transform3 = instance.transform;
						transform3.position += instance.bodyCollider.transform.forward * (0f - Settings.RecRoomSpeed) * Time.deltaTime * instance.scale;
						Transform transform4 = instance.transform;
						transform4.position += instance.bodyCollider.transform.right * (0f - Settings.RecRoomSpeed) * Time.deltaTime * instance.scale;
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00017E24 File Offset: 0x00016024
	public static void ApplyCr1ptsPSA()
	{
		bool flag = !Settings.Cr1ptsPSAEnabled;
		if (!flag)
		{
			try
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag2 = instance == null;
				if (!flag2)
				{
					Rigidbody attachedRigidbody = instance.bodyCollider.attachedRigidbody;
					bool flag3 = !(attachedRigidbody == null);
					if (flag3)
					{
						bool flag4 = Settings.IsPressed(Settings.Cr1ptsPSAKeybind);
						if (flag4)
						{
							PullSystem.burstDir = instance.AveragedVelocity.normalized * Settings.Cr1ptsPSAStrength;
						}
						else
						{
							PullSystem.burstDir = Vector3.zero;
						}
						attachedRigidbody.transform.position = Vector3.Lerp(attachedRigidbody.transform.position, attachedRigidbody.transform.position + PullSystem.burstDir, Settings.Cr1ptsPSALerpSpeed);
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x04000267 RID: 615
	private static Vector3 veloStickyDir = Vector3.zero;

	// Token: 0x04000268 RID: 616
	private static Vector3 burstStickyDir = Vector3.zero;

	// Token: 0x04000269 RID: 617
	private static bool veloTurnActive = false;

	// Token: 0x0400026A RID: 618
	private static float burstTimer = 0f;

	// Token: 0x0400026B RID: 619
	private static bool burstActive = false;

	// Token: 0x0400026C RID: 620
	private static bool burstDirLocked = false;

	// Token: 0x0400026D RID: 621
	private static Vector3 burstDir = Vector3.zero;
}
