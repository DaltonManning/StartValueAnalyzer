using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFDiagramInstances : AppSFPOUInstancesBase
{
	public override string ElementName => "DiagramInsts";

	public AppSFDiagramInstances(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override AppSFPOUInstance CreateSingleChild()
	{
		return new AppSFSingleDiagramInst(this, (RootModType)base.POUType);
	}

	protected override AppSFPOUInstance CreateReusableChild()
	{
		return new AppSFReusableDiagramInst(this, base.POUType);
	}
}
