using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public class StructComponent : DataInst
{
	public StructComponent(string name)
		: base(name)
	{
	}

	protected override void GetMemRef(ref tMemReference memRef, ushort externalOffset)
	{
		memRef.ValType = ((DataType)type).ValType;
		switch (memRef.Mode)
		{
		case tAddressingMode.DirectMode:
			memRef.UU.Offset += varOffset;
			break;
		case tAddressingMode.InDirectMode:
		case tAddressingMode.POUInDirectMode:
		case tAddressingMode.PtrInDirectMode:
			memRef.UU.U2.rOffset += varOffset;
			break;
		case tAddressingMode.Absolutemode:
			break;
		}
	}
}
