using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFVariablesBase : AppSFDataInstances
{
	public AppSFVariablesBase(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override bool CorrectElementName(string elementName)
	{
		return elementName.Equals("Var");
	}
}
