using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFPOUTypes : AppSFObjTypes
{
	public AppSFPOUTypes(SAXElementBase parent, SourceCodeUnit sourceCodeUnit)
		: base(parent, sourceCodeUnit)
	{
	}

	protected override bool CorrectElementName(string elementName)
	{
		return elementName.Equals("POUType");
	}
}
