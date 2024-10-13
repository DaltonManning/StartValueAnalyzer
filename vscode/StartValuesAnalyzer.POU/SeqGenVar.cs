using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public class SeqGenVar : SysGenVar
{
	public override bool Accessible
	{
		get
		{
			if (type == null || noUserAccess)
			{
				return false;
			}
			return ((DataType)type).Accessible;
		}
	}

	public SeqGenVar(string name)
		: base(name)
	{
	}

	public override void GetRetainProperties(string path, CProgramInstance programInstance, ushort pouInstanceIndex, tMemReference memRef, List<RetainProperties> retainProperties)
	{
		if (!Name.StartsWith("__") && type != null && !noUserAccess)
		{
			string path2 = path + "." + Name;
			GetMemRef(ref memRef);
			type.GetRetainProperties(path2, programInstance, pouInstanceIndex, memRef, retainProperties);
		}
	}
}
