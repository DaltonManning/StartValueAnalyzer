using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFSingleModInst : AppSFSinglePOUInst
{
	private readonly POUType pouType;

	public AppSFSingleModInst(SAXElementBase parent, POUType pouType)
		: base(parent)
	{
		this.pouType = pouType;
	}

	protected override POUInst CreatePOUInst(string name)
	{
		return pouType.CreateControlModule(name);
	}

	protected override AppSFSinglePOUType CreateChild()
	{
		return new AppSFSingleModType(this, (CMInst)base.POUInst, base.SubTreeSize);
	}

	protected override void UpdateChild()
	{
		((AppSFSingleModType)base.Child).CMInst = (CMInst)base.POUInst;
		((AppSFSingleModType)base.Child).SubTreeSize = base.SubTreeSize;
	}
}
