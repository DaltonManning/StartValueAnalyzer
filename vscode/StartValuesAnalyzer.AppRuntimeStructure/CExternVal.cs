namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CExternVal : CMemoryCell
{
	private int externVal;

	public int ExternVal
	{
		get
		{
			return externVal;
		}
		set
		{
			externVal = value;
		}
	}

	public CExternVal(int val)
	{
		externVal = val;
	}

	public override string ToString()
	{
		return "ExternVal " + externVal;
	}
}
