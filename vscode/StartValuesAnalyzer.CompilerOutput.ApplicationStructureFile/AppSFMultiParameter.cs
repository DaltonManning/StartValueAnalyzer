using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFMultiParameter : AppSFParameterBase
{
	public AppSFMultiParameter(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override Parameter CreateParameter(string name)
	{
		return base.POUType.CreateMultiParameter(name);
	}
}
