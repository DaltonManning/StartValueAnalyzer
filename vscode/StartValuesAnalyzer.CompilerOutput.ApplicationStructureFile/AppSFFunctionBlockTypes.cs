using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFFunctionBlockTypes : AppSFPOUTypes
{
	public override string ElementName => "FBTs";

	public AppSFFunctionBlockTypes(SAXElementBase parent, SourceCodeUnit sourceCodeUnit)
		: base(parent, sourceCodeUnit)
	{
	}

	protected override SAXElementBase CreateChild()
	{
		return new AppSFFunctionBlockType(this, base.SCU);
	}
}
