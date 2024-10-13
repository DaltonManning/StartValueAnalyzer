namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CMemVal : CMemoryCell
{
	private tMemoryLoc memVal;

	public tMemoryLoc MemVal
	{
		get
		{
			return memVal;
		}
		set
		{
			memVal = value;
		}
	}

	public CMemVal(tMemoryLoc val)
	{
		memVal = val;
	}

	public override string ToString()
	{
		return "MemVal, P=" + memVal.POUInstance + " V=" + memVal.VarOffset;
	}
}
