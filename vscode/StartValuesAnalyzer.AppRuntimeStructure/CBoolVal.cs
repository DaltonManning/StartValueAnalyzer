namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CBoolVal : CMemoryCell
{
	private bool boolVal;

	public bool BoolVal
	{
		get
		{
			return boolVal;
		}
		set
		{
			boolVal = value;
		}
	}

	public CBoolVal(bool val)
	{
		boolVal = val;
	}

	public override string ToString()
	{
		return boolVal.ToString();
	}
}
