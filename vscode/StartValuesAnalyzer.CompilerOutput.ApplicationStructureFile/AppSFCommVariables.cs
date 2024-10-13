using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFCommVariables : AppSFVariablesBase
{
	public override string ElementName => "CommVars";

	public AppSFCommVariables(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override SAXElementBase CreateChild()
	{
		return new AppSFCommVariable(this, base.POUType);
	}
}
