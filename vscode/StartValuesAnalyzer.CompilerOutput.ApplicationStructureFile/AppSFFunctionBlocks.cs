using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFFunctionBlocks : AppSFPOUInstancesBase
{
	public override string ElementName => "FBs";

	public AppSFFunctionBlocks(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override AppSFPOUInstance CreateSingleChild()
	{
		return null;
	}

	protected override AppSFPOUInstance CreateReusableChild()
	{
		return new AppSFFunctionBlock(this, base.POUType);
	}
}
