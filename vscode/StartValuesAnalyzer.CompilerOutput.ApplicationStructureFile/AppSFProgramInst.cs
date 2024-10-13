using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFProgramInst : AppSFSinglePOUInst
{
	private readonly RootModType rootModType;

	public AppSFProgramInst(SAXElementBase parent, RootModType rootModType)
		: base(parent)
	{
		this.rootModType = rootModType;
	}

	protected override POUInst CreatePOUInst(string name)
	{
		return rootModType.CreateProgram(name);
	}

	protected override AppSFSinglePOUType CreateChild()
	{
		return new AppSFSingleProgType(this, (ProgInst)base.POUInst, base.SubTreeSize);
	}

	protected override void UpdateChild()
	{
		((AppSFSingleProgType)base.Child).ProgInst = (ProgInst)base.POUInst;
		((AppSFSingleProgType)base.Child).SubTreeSize = base.SubTreeSize;
	}
}
