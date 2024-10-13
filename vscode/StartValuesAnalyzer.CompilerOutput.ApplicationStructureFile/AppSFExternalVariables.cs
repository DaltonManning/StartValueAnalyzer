using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFExternalVariables : AppSFParametersBase
{
	public override string ElementName => "ExtVars";

	public AppSFExternalVariables(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override SAXElementBase CreateChild()
	{
		return new AppSFExternalVariable(this, base.POUType);
	}
}
