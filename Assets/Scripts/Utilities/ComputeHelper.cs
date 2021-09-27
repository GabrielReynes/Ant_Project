using UnityEngine;

namespace Utilities
{
	public static class ComputeHelper
	{
		public static void Dispatch(ComputeShader _computeShader,
			float _groupSizeX, float _groupSizeY = 1, float _groupSizeZ = 1,
			int _kernelIndex = 0)
		{
			Vector3Int threadGroupSize = ThreadGroupSize(_computeShader, _kernelIndex);
			_computeShader.Dispatch(_kernelIndex,
				Mathf.CeilToInt(_groupSizeX / threadGroupSize.x),
				Mathf.CeilToInt(_groupSizeY / threadGroupSize.y),
				Mathf.CeilToInt(_groupSizeZ / threadGroupSize.z));
		}

		private static Vector3Int ThreadGroupSize(ComputeShader _computeShader, int _kernelIndex)
		{
			uint x, y, z;
			_computeShader.GetKernelThreadGroupSizes(_kernelIndex, out x, out y, out z);
			return new Vector3Int((int)x, (int)y, (int)z);
		}
	}
}