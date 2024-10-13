using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFExternalVariable : AppSFParameterBase
{
	public AppSFExternalVariable(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override Parameter CreateParameter(string name)
	{
		return base.POUType.CreateExternalVariable(name);
	}
}
