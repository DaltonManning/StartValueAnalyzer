using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFReusablePOUType : AppSFPOUType
{
	private readonly SourceCodeUnit sourceCodeUnit;

	protected SourceCodeUnit SCU => sourceCodeUnit;

	public AppSFReusablePOUType(SAXElementBase parent, SourceCodeUnit sourceCodeUnit)
		: base(parent)
	{
		this.sourceCodeUnit = sourceCodeUnit;
	}
}
