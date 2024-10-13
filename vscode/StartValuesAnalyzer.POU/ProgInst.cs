namespace StartValuesAnalyzer.POU;

public class ProgInst : FBInst
{
	public override tObjTypeClass ObjTypeClass => tObjTypeClass.ProgTypeClass;

	public ProgInst(string name)
		: base(name)
	{
	}

	public override POUType CreateSingleType()
	{
		return (POUType)(base.SingleTypeRef = new SingleProgType(name));
	}
}
