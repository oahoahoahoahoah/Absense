using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200000F RID: 15
public static class Desync
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x0600003C RID: 60 RVA: 0x000052CD File Offset: 0x000034CD
	public static bool IsLagActive
	{
		get
		{
			return Desync.lagActive;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600003D RID: 61 RVA: 0x000052D4 File Offset: 0x000034D4
	private static bool InFreezeWindow
	{
		get
		{
			return Time.time < Desync.freezeUntil;
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000052E4 File Offset: 0x000034E4
	public static void TriggerLagSpike(float lastCaptureTime = 0.3f)
	{
		float num2 = Time.time + lastCaptureTime;
		bool flag = num2 > Desync.freezeUntil;
		if (flag)
		{
			Desync.freezeUntil = num2;
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00005318 File Offset: 0x00003518
	public static void Update()
	{
		bool flag2 = !Settings.DesyncEnabled;
		if (flag2)
		{
			int count = Desync.snapshots.Count;
			bool flag3 = count > 0;
			if (flag3)
			{
				Desync.Reset();
			}
		}
		else
		{
			LagMode desyncMode = Settings.DesyncMode;
			bool flag4 = desyncMode == LagMode.LagSwitch;
			if (flag4)
			{
				bool flag = Settings.IsPressed(Settings.DesyncLagSwitchKeybind);
				bool flag5 = flag && !Desync.lagSwitchHeld;
				if (flag5)
				{
					bool flag6 = !Desync.lagActive;
					if (flag6)
					{
						Desync.lagActive = true;
						Desync.visualizing = false;
					}
					else
					{
						Desync.lagActive = false;
					}
				}
				Desync.lagSwitchHeld = flag;
			}
			bool flag7 = Settings.DesyncMode != LagMode.FakeLag;
			if (!flag7)
			{
				Desync.nextFakeLagTime += Time.deltaTime * 1000f;
				bool flag8 = !Desync.snapshotHeld;
				if (flag8)
				{
					bool flag9 = Desync.nextFakeLagTime >= Settings.DesyncFakeLagIntervalMs;
					if (flag9)
					{
						Desync.snapshotHeld = true;
						Desync.fakeLagState = false;
						Desync.nextFakeLagTime = 0f;
					}
				}
				else
				{
					bool flag10 = Desync.nextFakeLagTime >= Settings.DesyncFakeLagFreezeMs;
					if (flag10)
					{
						Desync.snapshotHeld = false;
						Desync.nextFakeLagTime = 0f;
					}
				}
			}
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00005448 File Offset: 0x00003648
	public static void LateUpdate()
	{
		bool flag = !Settings.DesyncEnabled;
		if (!flag)
		{
			VRRig val = ((GorillaTagger.Instance != null) ? GorillaTagger.Instance.offlineVRRig : null);
			bool flag2 = val == null;
			if (!flag2)
			{
				Desync.RigSnapshot item = Desync.CaptureSnapshot(val);
				bool boolean_ = Desync.InFreezeWindow;
				bool flag3 = Settings.DesyncMode == LagMode.Delay;
				if (flag3)
				{
					Desync.snapshots.Add(item);
					for (;;)
					{
						int count = Desync.snapshots.Count;
						bool flag4 = count <= 2400;
						if (flag4)
						{
							break;
						}
						List<Desync.RigSnapshot> list = Desync.snapshots;
						list.RemoveAt(0);
					}
					bool flag5 = !boolean_;
					if (flag5)
					{
						Desync.RigSnapshot struct2_ = Desync.UpdateVisualize();
						Desync.ApplySnapshot(val, struct2_);
					}
				}
				else
				{
					LagMode desyncMode = Settings.DesyncMode;
					bool flag6 = desyncMode == LagMode.LagSwitch;
					if (flag6)
					{
						bool flag7 = Desync.lagActive;
						if (flag7)
						{
							bool flag8 = !Desync.visualizing;
							if (flag8)
							{
								Desync.lastSnapshot = item;
								Desync.visualizing = true;
							}
							bool flag9 = !boolean_;
							if (flag9)
							{
								Desync.ApplySnapshot(val, Desync.lastSnapshot);
							}
						}
					}
					else
					{
						LagMode desyncMode2 = Settings.DesyncMode;
						bool flag10 = desyncMode2 == LagMode.FakeLag && Desync.snapshotHeld;
						if (flag10)
						{
							bool flag11 = !Desync.fakeLagState;
							if (flag11)
							{
								Desync.heldSnapshot = item;
								Desync.fakeLagState = true;
							}
							bool flag12 = !boolean_;
							if (flag12)
							{
								Desync.ApplySnapshot(val, Desync.heldSnapshot);
							}
						}
					}
				}
				bool desyncVisualise = Settings.DesyncVisualise;
				if (desyncVisualise)
				{
					Color color_ = default(Color);
					color_..ctor(1f, 0.2f, 0.2f, 0.4f);
					GTPlayer instance = GTPlayer.Instance;
					bool flag13 = instance != null;
					if (flag13)
					{
						Vector3 vector3_ = ((instance.LeftHand.controllerTransform != null) ? instance.LeftHand.controllerTransform.position : Vector3.zero);
						Vector3 savedScale = ((instance.RightHand.controllerTransform != null) ? instance.RightHand.controllerTransform.position : Vector3.zero);
						Desync.visualHead = Desync.EnsureVisualObjects(Desync.visualHead, ref Desync.visualHeadRenderer, vector3_, 0.08f, color_);
						Desync.visualLeft = Desync.EnsureVisualObjects(Desync.visualLeft, ref Desync.visualLeftRenderer, savedScale, 0.08f, color_);
					}
				}
				else
				{
					Desync.BuildRigList(ref Desync.visualHead, ref Desync.visualHeadRenderer);
					Desync.BuildRigList(ref Desync.visualLeft, ref Desync.visualLeftRenderer);
				}
			}
		}
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000056CC File Offset: 0x000038CC
	private static Desync.RigSnapshot CaptureSnapshot(VRRig rig)
	{
		Desync.RigSnapshot result = default(Desync.RigSnapshot);
		result = new Desync.RigSnapshot
		{
			time = Time.time,
			rigRootPos = rig.transform.position,
			rigRootRot = rig.transform.rotation
		};
		bool flag = rig.head != null;
		Desync.RigSnapshot rigSnapshot;
		if (flag)
		{
			bool flag2 = rig.head.rigTarget != null;
			if (flag2)
			{
				bool flag3 = rig.leftHand != null;
				if (flag3)
				{
					bool flag4 = !(rig.leftHand.rigTarget != null);
					if (flag4)
					{
					}
					bool flag5 = rig.rightHand == null;
					if (!flag5)
					{
						bool flag6 = rig.rightHand.rigTarget != null;
						if (flag6)
						{
						}
					}
					bool flag7 = !(rig.headMesh != null);
					if (flag7)
					{
					}
					bool flag8 = !(rig.mainSkin != null);
					if (!flag8)
					{
						bool flag9 = rig.mainSkin.bones != null;
						if (flag9)
						{
							Transform[] array = rig.mainSkin.bones;
							result.bones = new Desync.Struct1[array.Length];
							int num2 = 0;
							for (;;)
							{
								bool flag10 = num2 >= array.Length;
								if (flag10)
								{
									break;
								}
								bool flag11 = array[num2] != null;
								if (flag11)
								{
									num2++;
								}
								else
								{
									num2++;
								}
							}
						}
					}
					rigSnapshot = result;
				}
				else
				{
					bool flag12 = rig.rightHand == null;
					if (!flag12)
					{
						bool flag13 = rig.rightHand.rigTarget != null;
						if (flag13)
						{
						}
					}
					bool flag14 = !(rig.headMesh != null);
					if (flag14)
					{
					}
					bool flag15 = !(rig.mainSkin != null);
					if (!flag15)
					{
						bool flag16 = rig.mainSkin.bones != null;
						if (flag16)
						{
							Transform[] array = rig.mainSkin.bones;
							result.bones = new Desync.Struct1[array.Length];
							int num2 = 0;
							for (;;)
							{
								bool flag17 = num2 >= array.Length;
								if (flag17)
								{
									break;
								}
								bool flag18 = array[num2] != null;
								if (flag18)
								{
									num2++;
								}
								else
								{
									num2++;
								}
							}
						}
					}
					rigSnapshot = result;
				}
			}
			else
			{
				bool flag19 = rig.leftHand != null;
				if (flag19)
				{
					bool flag20 = !(rig.leftHand.rigTarget != null);
					if (flag20)
					{
					}
					bool flag21 = rig.rightHand == null;
					if (!flag21)
					{
						bool flag22 = rig.rightHand.rigTarget != null;
						if (flag22)
						{
						}
					}
					bool flag23 = !(rig.headMesh != null);
					if (flag23)
					{
					}
					bool flag24 = !(rig.mainSkin != null);
					if (!flag24)
					{
						bool flag25 = rig.mainSkin.bones != null;
						if (flag25)
						{
							Transform[] array = rig.mainSkin.bones;
							result.bones = new Desync.Struct1[array.Length];
							int num2 = 0;
							for (;;)
							{
								bool flag26 = num2 >= array.Length;
								if (flag26)
								{
									break;
								}
								bool flag27 = array[num2] != null;
								if (flag27)
								{
									num2++;
								}
								else
								{
									num2++;
								}
							}
						}
					}
					rigSnapshot = result;
				}
				else
				{
					bool flag28 = rig.rightHand == null;
					if (!flag28)
					{
						bool flag29 = rig.rightHand.rigTarget != null;
						if (flag29)
						{
						}
					}
					bool flag30 = !(rig.headMesh != null);
					if (flag30)
					{
					}
					bool flag31 = !(rig.mainSkin != null);
					if (!flag31)
					{
						bool flag32 = rig.mainSkin.bones != null;
						if (flag32)
						{
							Transform[] array = rig.mainSkin.bones;
							result.bones = new Desync.Struct1[array.Length];
							int num2 = 0;
							for (;;)
							{
								bool flag33 = num2 >= array.Length;
								if (flag33)
								{
									break;
								}
								bool flag34 = array[num2] != null;
								if (flag34)
								{
									num2++;
								}
								else
								{
									num2++;
								}
							}
						}
					}
					rigSnapshot = result;
				}
			}
		}
		else
		{
			bool flag35 = rig.leftHand != null;
			if (flag35)
			{
				bool flag36 = !(rig.leftHand.rigTarget != null);
				if (flag36)
				{
				}
				bool flag37 = rig.rightHand == null;
				if (!flag37)
				{
					bool flag38 = rig.rightHand.rigTarget != null;
					if (flag38)
					{
					}
				}
				bool flag39 = !(rig.headMesh != null);
				if (flag39)
				{
				}
				bool flag40 = !(rig.mainSkin != null);
				if (!flag40)
				{
					bool flag41 = rig.mainSkin.bones != null;
					if (flag41)
					{
						Transform[] array = rig.mainSkin.bones;
						result.bones = new Desync.Struct1[array.Length];
						int num2 = 0;
						for (;;)
						{
							bool flag42 = num2 >= array.Length;
							if (flag42)
							{
								break;
							}
							bool flag43 = array[num2] != null;
							if (flag43)
							{
								num2++;
							}
							else
							{
								num2++;
							}
						}
					}
				}
				rigSnapshot = result;
			}
			else
			{
				bool flag44 = rig.rightHand == null;
				if (!flag44)
				{
					bool flag45 = rig.rightHand.rigTarget != null;
					if (flag45)
					{
					}
				}
				bool flag46 = !(rig.headMesh != null);
				if (flag46)
				{
				}
				bool flag47 = !(rig.mainSkin != null);
				if (!flag47)
				{
					bool flag48 = rig.mainSkin.bones != null;
					if (flag48)
					{
						Transform[] array = rig.mainSkin.bones;
						result.bones = new Desync.Struct1[array.Length];
						int num2 = 0;
						for (;;)
						{
							bool flag49 = num2 >= array.Length;
							if (flag49)
							{
								break;
							}
							bool flag50 = array[num2] != null;
							if (flag50)
							{
								num2++;
							}
							else
							{
								num2++;
							}
						}
					}
				}
				rigSnapshot = result;
			}
		}
		return rigSnapshot;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00005CBC File Offset: 0x00003EBC
	private static void ApplySnapshot(VRRig rig, Desync.RigSnapshot struct2_2)
	{
		bool flag = rig.head != null;
		if (flag)
		{
			bool flag2 = !(rig.head.rigTarget != null);
			if (flag2)
			{
			}
			bool flag3 = rig.leftHand == null;
			if (!flag3)
			{
				bool flag4 = rig.leftHand.rigTarget != null;
				if (flag4)
				{
				}
			}
			bool flag5 = rig.rightHand != null;
			if (flag5)
			{
				bool flag6 = rig.rightHand.rigTarget != null;
				if (flag6)
				{
				}
			}
			bool flag7 = rig.headMesh != null;
			if (flag7)
			{
			}
			bool flag8 = rig.mainSkin != null;
			if (flag8)
			{
				bool flag9 = rig.mainSkin.bones != null;
				if (flag9)
				{
					bool flag10 = struct2_2.bones == null;
					if (!flag10)
					{
						Transform[] array = rig.mainSkin.bones;
						int num3 = Mathf.Min(array.Length, struct2_2.bones.Length);
						int num4 = 0;
						for (;;)
						{
							bool flag11 = num4 < num3;
							if (!flag11)
							{
								break;
							}
							bool flag12 = !(array[num4] != null);
							if (flag12)
							{
							}
							num4++;
						}
					}
				}
			}
		}
		else
		{
			bool flag13 = rig.leftHand == null;
			if (!flag13)
			{
				bool flag14 = rig.leftHand.rigTarget != null;
				if (flag14)
				{
				}
			}
			bool flag15 = rig.rightHand != null;
			if (flag15)
			{
				bool flag16 = rig.rightHand.rigTarget != null;
				if (flag16)
				{
				}
			}
			bool flag17 = rig.headMesh != null;
			if (flag17)
			{
			}
			bool flag18 = rig.mainSkin != null;
			if (flag18)
			{
				bool flag19 = rig.mainSkin.bones != null;
				if (flag19)
				{
					bool flag20 = struct2_2.bones == null;
					if (!flag20)
					{
						Transform[] array = rig.mainSkin.bones;
						int num3 = Mathf.Min(array.Length, struct2_2.bones.Length);
						int num4 = 0;
						for (;;)
						{
							bool flag21 = num4 < num3;
							if (!flag21)
							{
								break;
							}
							bool flag22 = !(array[num4] != null);
							if (flag22)
							{
							}
							num4++;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00005F00 File Offset: 0x00004100
	private static Desync.RigSnapshot UpdateVisualize()
	{
		Desync.RigSnapshot @struct = default(Desync.RigSnapshot);
		Desync.RigSnapshot struct2_ = default(Desync.RigSnapshot);
		bool flag = Desync.snapshots.Count != 0;
		Desync.RigSnapshot rigSnapshot;
		if (flag)
		{
			float num5 = Time.time - Settings.DesyncDelayMs / 1000f;
			int num6 = -1;
			int count = Desync.snapshots.Count;
			int num7 = count - 1;
			for (;;)
			{
				int num8 = num7;
				bool flag2 = num8 < 0;
				if (flag2)
				{
					break;
				}
				bool flag3 = Desync.snapshots[num7].time > num5;
				if (!flag3)
				{
					goto IL_009B;
				}
				num7--;
			}
			goto IL_00A6;
			IL_009B:
			num6 = num7;
			IL_00A6:
			int num9 = num6;
			bool flag4 = num9 >= 0;
			if (flag4)
			{
				int num10 = num6;
				int count2 = Desync.snapshots.Count;
				bool flag5 = num10 >= count2 - 1;
				if (flag5)
				{
					rigSnapshot = Desync.snapshots[num6];
				}
				else
				{
					@struct = Desync.snapshots[num6];
					List<Desync.RigSnapshot> list = Desync.snapshots;
					int num11 = num6;
					struct2_ = list[num11 + 1];
					float num12 = struct2_.time - @struct.time;
					bool flag6 = num12 > 0f;
					if (flag6)
					{
						float float_ = Mathf.Clamp01((num5 - @struct.time) / num12);
						rigSnapshot = Desync.ClearVisualize(@struct, struct2_, float_);
					}
					else
					{
						rigSnapshot = @struct;
					}
				}
			}
			else
			{
				rigSnapshot = Desync.snapshots[0];
			}
		}
		else
		{
			rigSnapshot = default(Desync.RigSnapshot);
		}
		return rigSnapshot;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00006084 File Offset: 0x00004284
	private static Desync.RigSnapshot ClearVisualize(Desync.RigSnapshot struct2_2, Desync.RigSnapshot struct2_3, float lastCaptureTime)
	{
		Desync.RigSnapshot result = default(Desync.RigSnapshot);
		result = new Desync.RigSnapshot
		{
			time = Mathf.Lerp(struct2_2.time, struct2_3.time, lastCaptureTime),
			rigRootPos = Vector3.Lerp(struct2_2.rigRootPos, struct2_3.rigRootPos, lastCaptureTime),
			rigRootRot = Quaternion.Slerp(struct2_2.rigRootRot, struct2_3.rigRootRot, lastCaptureTime),
			headRigTargetPos = Vector3.Lerp(struct2_2.headRigTargetPos, struct2_3.headRigTargetPos, lastCaptureTime),
			headRigTargetRot = Quaternion.Slerp(struct2_2.headRigTargetRot, struct2_3.headRigTargetRot, lastCaptureTime),
			leftHandRigTargetPos = Vector3.Lerp(struct2_2.leftHandRigTargetPos, struct2_3.leftHandRigTargetPos, lastCaptureTime),
			leftHandRigTargetRot = Quaternion.Slerp(struct2_2.leftHandRigTargetRot, struct2_3.leftHandRigTargetRot, lastCaptureTime),
			rightHandRigTargetPos = Vector3.Lerp(struct2_2.rightHandRigTargetPos, struct2_3.rightHandRigTargetPos, lastCaptureTime),
			rightHandRigTargetRot = Quaternion.Slerp(struct2_2.rightHandRigTargetRot, struct2_3.rightHandRigTargetRot, lastCaptureTime),
			headMeshPos = Vector3.Lerp(struct2_2.headMeshPos, struct2_3.headMeshPos, lastCaptureTime),
			headMeshRot = Quaternion.Slerp(struct2_2.headMeshRot, struct2_3.headMeshRot, lastCaptureTime),
			headMeshScale = Vector3.Lerp(struct2_2.headMeshScale, struct2_3.headMeshScale, lastCaptureTime)
		};
		bool flag = struct2_2.bones != null;
		if (flag)
		{
			bool flag2 = struct2_3.bones != null;
			if (flag2)
			{
				bool flag3 = struct2_2.bones.Length != struct2_3.bones.Length;
				if (!flag3)
				{
					int num2 = 0;
					for (;;)
					{
						bool flag4 = num2 >= struct2_2.bones.Length;
						if (flag4)
						{
							break;
						}
						num2++;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00006244 File Offset: 0x00004444
	private static GameObject EnsureVisualObjects(GameObject visualRight, ref Renderer visualRightRenderer, Vector3 savedPosition, float lastCaptureTime, Color markerColor)
	{
		bool flag = visualRight == null;
		if (flag)
		{
			visualRight = GameObject.CreatePrimitive(0);
			SphereCollider val = visualRight.GetComponent<SphereCollider>();
			bool flag2 = val != null;
			if (flag2)
			{
				Object.DestroyImmediate(val);
				visualRightRenderer = visualRight.GetComponent<Renderer>();
			}
			visualRightRenderer = visualRight.GetComponent<Renderer>();
		}
		bool flag3 = visualRightRenderer != null;
		if (flag3)
		{
		}
		return visualRight;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000062AC File Offset: 0x000044AC
	private static void BuildRigList(ref GameObject visualRight, ref Renderer visualRightRenderer)
	{
		bool flag = !(visualRight != null);
		if (!flag)
		{
			Object.Destroy(visualRight);
			visualRight = null;
		}
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000062DC File Offset: 0x000044DC
	public static void Reset()
	{
		Desync.snapshots.Clear();
		Desync.lagActive = false;
		Desync.lagSwitchHeld = false;
		Desync.visualizing = false;
		Desync.nextFakeLagTime = 0f;
		Desync.snapshotHeld = false;
		Desync.fakeLagState = false;
		Desync.BuildRigList(ref Desync.visualHead, ref Desync.visualHeadRenderer);
		Desync.BuildRigList(ref Desync.visualLeft, ref Desync.visualLeftRenderer);
	}

	// Token: 0x04000167 RID: 359
	private static List<Desync.RigSnapshot> snapshots = new List<Desync.RigSnapshot>(2400);

	// Token: 0x04000168 RID: 360
	private const int snapshotIndex = 2400;

	// Token: 0x04000169 RID: 361
	private static Desync.RigSnapshot lastSnapshot;

	// Token: 0x0400016A RID: 362
	private static bool lagActive = false;

	// Token: 0x0400016B RID: 363
	private static bool lagSwitchHeld = false;

	// Token: 0x0400016C RID: 364
	private static bool visualizing = false;

	// Token: 0x0400016D RID: 365
	private static float nextFakeLagTime = 0f;

	// Token: 0x0400016E RID: 366
	private static bool snapshotHeld = false;

	// Token: 0x0400016F RID: 367
	private static Desync.RigSnapshot heldSnapshot;

	// Token: 0x04000170 RID: 368
	private static bool fakeLagState = false;

	// Token: 0x04000171 RID: 369
	private static float freezeUntil = 0f;

	// Token: 0x04000172 RID: 370
	private static GameObject visualHead;

	// Token: 0x04000173 RID: 371
	private static GameObject visualLeft;

	// Token: 0x04000174 RID: 372
	private static Renderer visualHeadRenderer;

	// Token: 0x04000175 RID: 373
	private static Renderer visualLeftRenderer;

	// Token: 0x02000059 RID: 89
	private struct Struct1
	{
		// Token: 0x040004D3 RID: 1235
		public Vector3 localPos;

		// Token: 0x040004D4 RID: 1236
		public Quaternion localRot;

		// Token: 0x040004D5 RID: 1237
		public Vector3 localScale;
	}

	// Token: 0x0200005A RID: 90
	private struct RigSnapshot
	{
		// Token: 0x040004D6 RID: 1238
		public float time;

		// Token: 0x040004D7 RID: 1239
		public Vector3 rigRootPos;

		// Token: 0x040004D8 RID: 1240
		public Quaternion rigRootRot;

		// Token: 0x040004D9 RID: 1241
		public Vector3 headRigTargetPos;

		// Token: 0x040004DA RID: 1242
		public Quaternion headRigTargetRot;

		// Token: 0x040004DB RID: 1243
		public Vector3 leftHandRigTargetPos;

		// Token: 0x040004DC RID: 1244
		public Quaternion leftHandRigTargetRot;

		// Token: 0x040004DD RID: 1245
		public Vector3 rightHandRigTargetPos;

		// Token: 0x040004DE RID: 1246
		public Quaternion rightHandRigTargetRot;

		// Token: 0x040004DF RID: 1247
		public Vector3 headMeshPos;

		// Token: 0x040004E0 RID: 1248
		public Quaternion headMeshRot;

		// Token: 0x040004E1 RID: 1249
		public Vector3 headMeshScale;

		// Token: 0x040004E2 RID: 1250
		public Desync.Struct1[] bones;
	}
}
