using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFLocalVariable : AppSFVariableBase
{
	public AppSFLocalVariable(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override DataInst CreateVariable(string name)
	{
		return base.POUType.CreateVariable(name);
	}
}
