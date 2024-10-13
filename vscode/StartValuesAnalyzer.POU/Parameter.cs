using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public class Parameter : DataInst
{
	protected tParDirection direction;

	protected tParTransfer transfer;

	public tParDirection ParDirection
	{
		set
		{
			direction = value;
		}
	}

	public tParTransfer ParTransfer
	{
		set
		{
			transfer = value;
		}
	}

	public Parameter(string name)
		: base(name)
	{
		direction = tParDirection.vInOutPar;
		transfer = tParTransfer.vStaticRefPar;
	}

	protected override void GetMemRef(ref tMemReference memRef, ushort externalOffset)
	{
		memRef.ValType = ((DataType)type).ValType;
		switch (transfer)
		{
		case tParTransfer.vValuePar:
			memRef.Mode = tAddressingMode.DirectMode;
			memRef.UU.Offset = varOffset;
			break;
		case tParTransfer.vStaticRefPar:
			memRef.Mode = tAddressingMode.InDirectMode;
			memRef.UU.U2.mOffset = varOffset;
			memRef.UU.U2.rOffset = externalOffset;
			break;
		case tParTransfer.vDynamicRefPar:
			memRef.Mode = tAddressingMode.PtrInDirectMode;
			memRef.UU.U2.mOffset = varOffset;
			memRef.UU.U2.rOffset = externalOffset;
			break;
		}
	}
}
