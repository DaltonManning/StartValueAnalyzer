namespace StartValuesAnalyzer.POU;

public class DiagramInst : ModInst
{
	public override tObjTypeClass ObjTypeClass => tObjTypeClass.DiagramTypeClass;

	public DiagramInst(string name)
		: base(name)
	{
	}

	public override POUType CreateSingleType()
	{
		return (POUType)(base.SingleTypeRef = new SingleDiagramType());
	}
}
