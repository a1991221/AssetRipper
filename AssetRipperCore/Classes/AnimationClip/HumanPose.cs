using AssetRipper.Core.Classes.Misc;
using AssetRipper.Core.IO.Asset;
using AssetRipper.Core.Math;
using AssetRipper.Core.Parser.Files;

namespace AssetRipper.Core.Classes.AnimationClip
{
	public struct HumanPose : IAssetReadable
	{
		/// <summary>
		/// 5.4.0 and greater
		/// </summary>
		public static bool IsVector3f(UnityVersion version) => version.IsGreaterEqual(5, 4);
		/// <summary>
		/// 5.2.0 and greater
		/// </summary>
		public static bool HasTArray(UnityVersion version) => version.IsGreaterEqual(5, 2);

		public void Read(AssetReader reader)
		{
			RootX.Read(reader);
			if (IsVector3f(reader.Version))
			{
				LookAtPosition = reader.ReadAsset<Vector3f>();
			}
			else
			{
				LookAtPosition.Read(reader);
			}
			LookAtWeight.Read(reader);
			GoalArray = reader.ReadAssetArray<HumanGoal>();
			LeftHandPose.Read(reader);
			RightHandPose.Read(reader);
			DoFArray = reader.ReadSingleArray();

			if (HasTArray(reader.Version))
			{
				if (IsVector3f(reader.Version))
				{
					TDoFArray = ReadVector3Array(reader);
				}
				else
				{
					TDoFArray = reader.ReadAssetArray<Vector4f>();
				}
			}
		}

		private static Vector4f[] ReadVector3Array(AssetReader reader)
		{
			int count = reader.ReadInt32();
			Vector4f[] array = new Vector4f[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = reader.ReadAsset<Vector3f>();
			}
			return array;
		}

		public HumanGoal[] GoalArray { get; set; }
		public float[] DoFArray { get; set; }
		public Vector4f[] TDoFArray { get; set; }

		public XForm RootX;
		public Vector4f LookAtPosition;
		public Vector4f LookAtWeight;
		public HandPose LeftHandPose;
		public HandPose RightHandPose;
	}
}
