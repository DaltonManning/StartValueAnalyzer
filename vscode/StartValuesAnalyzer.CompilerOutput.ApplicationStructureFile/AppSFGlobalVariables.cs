using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFGlobalVariables : AppSFVariablesBase
{
	public override string ElementName => "GlobVars";

	public AppSFGlobalVariables(SAXElementBase parent, RootModType rootModType)
		: base(parent, rootModType)
	{
	}

	protected override SAXElementBase CreateChild()
	{
		return new AppSFGlobalVariable(this, (RootModType)base.POUType);
	}
}
