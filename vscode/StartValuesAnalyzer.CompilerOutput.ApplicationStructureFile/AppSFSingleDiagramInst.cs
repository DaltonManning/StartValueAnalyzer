using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFSingleDiagramInst : AppSFSinglePOUInst
{
	private readonly RootModType rootModType;

	public AppSFSingleDiagramInst(SAXElementBase parent, RootModType rootModType)
		: base(parent)
	{
		this.rootModType = rootModType;
	}

	protected override POUInst CreatePOUInst(string name)
	{
		return rootModType.CreateDiagram(name);
	}

	protected override AppSFSinglePOUType CreateChild()
	{
		return new AppSFSingleDiagramType(this, (DiagramInst)base.POUInst, base.SubTreeSize);
	}

	protected override void UpdateChild()
	{
		((AppSFSingleDiagramType)base.Child).DiagramInst = (DiagramInst)base.POUInst;
		((AppSFSingleDiagramType)base.Child).SubTreeSize = base.SubTreeSize;
	}
}
