namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CUIntVal : CMemoryCell
{
	private int uintVal;

	public int UIntVal
	{
		get
		{
			return uintVal;
		}
		set
		{
			uintVal = value;
		}
	}

	public CUIntVal(int val)
	{
		uintVal = val;
	}

	public override string ToString()
	{
		return uintVal.ToString();
	}
}
