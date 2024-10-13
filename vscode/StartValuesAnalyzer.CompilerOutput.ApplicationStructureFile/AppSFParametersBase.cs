using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFParametersBase : AppSFDataInstances
{
	public AppSFParametersBase(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override bool CorrectElementName(string elementName)
	{
		return elementName.Equals("Param");
	}
}
