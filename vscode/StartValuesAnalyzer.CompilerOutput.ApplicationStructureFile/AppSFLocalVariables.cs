using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFLocalVariables : AppSFVariablesBase
{
	public override string ElementName => "LocVars";

	public AppSFLocalVariables(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override SAXElementBase CreateChild()
	{
		return new AppSFLocalVariable(this, base.POUType);
	}
}
