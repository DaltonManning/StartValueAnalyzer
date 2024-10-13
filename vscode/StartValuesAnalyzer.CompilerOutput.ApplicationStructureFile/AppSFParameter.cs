using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFParameter : AppSFParameterBase
{
	public AppSFParameter(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override Parameter CreateParameter(string name)
	{
		return base.POUType.CreateParameter(name);
	}
}
