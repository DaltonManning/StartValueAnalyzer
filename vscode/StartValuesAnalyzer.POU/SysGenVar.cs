using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public abstract class SysGenVar : Variable
{
	public override bool Accessible => false;

	public SysGenVar(string name)
		: base(name)
	{
	}

	public override void GetRetainProperties(string path, CProgramInstance programInstance, ushort pouInstanceIndex, tMemReference memRef, List<RetainProperties> retainProperties)
	{
	}
}
