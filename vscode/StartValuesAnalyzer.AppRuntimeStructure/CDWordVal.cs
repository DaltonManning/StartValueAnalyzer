namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CDWordVal : CMemoryCell
{
	private int dwordVal;

	public int DWordVal
	{
		get
		{
			return dwordVal;
		}
		set
		{
			dwordVal = value;
		}
	}

	public CDWordVal(int val)
	{
		dwordVal = val;
	}

	public override string ToString()
	{
		return dwordVal.ToString();
	}
}
