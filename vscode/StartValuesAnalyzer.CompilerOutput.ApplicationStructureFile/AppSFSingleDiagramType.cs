using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFSingleDiagramType : AppSFSinglePOUType
{
	public DiagramInst DiagramInst
	{
		set
		{
			base.POUInst = value;
		}
	}

	public AppSFSingleDiagramType(SAXElementBase parent, DiagramInst diagramInst, int subTreeSize)
		: base(parent, diagramInst, subTreeSize)
	{
	}

	protected override POUType CreatePOUType(string name)
	{
		return ((DiagramInst)base.POUInst).CreateSingleType();
	}
}
