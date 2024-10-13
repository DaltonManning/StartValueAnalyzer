namespace StartValuesAnalyzer.POU;

public class FBInst : POUInst
{
	protected ushort varOffset;

	public ushort VarOffset
	{
		get
		{
			return varOffset;
		}
		set
		{
			varOffset = value;
		}
	}

	public override tObjTypeClass ObjTypeClass => tObjTypeClass.FBTypeClass;

	public FBInst(string name)
		: base(name)
	{
		varOffset = 0;
	}
}
