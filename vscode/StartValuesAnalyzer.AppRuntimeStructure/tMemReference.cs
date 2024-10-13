using System.Runtime.InteropServices;

namespace StartValuesAnalyzer.AppRuntimeStructure;

public struct tMemReference
{
	[StructLayout(LayoutKind.Explicit)]
	public struct UUStruct
	{
		public struct U2Struct
		{
			public ushort mOffset;

			public ushort rOffset;
		}

		[FieldOffset(0)]
		public ushort Offset;

		[FieldOffset(0)]
		public U2Struct U2;

		[FieldOffset(0)]
		public tMemoryLoc MemVal;
	}

	public tValType ValType;

	public tAddressingMode Mode;

	public UUStruct UU;
}
