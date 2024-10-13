using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public abstract class POUInst : ObjInst
{
	private ushort pouOffset;

	public ushort POUOffset
	{
		get
		{
			return pouOffset;
		}
		set
		{
			pouOffset = value;
		}
	}

	public POUInst(string name)
		: base(name)
	{
		pouOffset = ushort.MaxValue;
	}

	public virtual POUType CreateSingleType()
	{
		return null;
	}

	public override bool GetVariableFromPath(string path, ref DataInst dataInst, ref ushort pouInstanceIndex, ref tMemReference memRef, ref bool writeAllowed, ref bool cvStatus)
	{
		if (type == null)
		{
			return false;
		}
		pouInstanceIndex += pouOffset;
		return type.GetVariableFromPath(path, ref dataInst, ref pouInstanceIndex, ref memRef, ref writeAllowed, ref cvStatus);
	}

	public override void GetRetainProperties(string path, CProgramInstance programInstance, ushort pouInstanceIndex, tMemReference memRef, List<RetainProperties> retainProperties)
	{
		if (type != null)
		{
			string path2 = path + "." + Name;
			ushort pouInstanceIndex2 = (ushort)(pouInstanceIndex + pouOffset);
			type.GetRetainProperties(path2, programInstance, pouInstanceIndex2, memRef, retainProperties);
		}
	}
}
