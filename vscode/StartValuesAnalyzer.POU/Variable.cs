using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public abstract class Variable : DataInst
{
	public Variable(string name)
		: base(name)
	{
	}

	protected override void GetMemRef(ref tMemReference memRef, ushort externalOffset)
	{
		memRef.ValType = ((DataType)type).ValType;
		memRef.Mode = tAddressingMode.DirectMode;
		if (state)
		{
			memRef.UU.Offset = (ushort)(varOffset + 1);
		}
		else
		{
			memRef.UU.Offset = varOffset;
		}
	}

	public override void GetRetainProperties(string path, CProgramInstance programInstance, ushort pouInstanceIndex, tMemReference memRef, List<RetainProperties> retainProperties)
	{
		if (!Name.StartsWith("__"))
		{
			base.GetRetainProperties(path, programInstance, pouInstanceIndex, memRef, retainProperties);
		}
	}
}
