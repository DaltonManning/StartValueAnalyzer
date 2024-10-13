using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFGlobalVariable : AppSFVariableBase
{
	public AppSFGlobalVariable(SAXElementBase parent, RootModType rootModType)
		: base(parent, rootModType)
	{
	}

	protected override DataInst CreateVariable(string name)
	{
		return base.POUType.CreateGlobalVariable(name);
	}
}
