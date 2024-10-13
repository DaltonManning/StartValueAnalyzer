namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CStringVal : CMemoryCell
{
	private string stringVal;

	public string StringVal
	{
		get
		{
			return stringVal;
		}
		set
		{
			stringVal = value;
		}
	}

	public CStringVal(string val)
	{
		stringVal = val;
	}

	public override string ToString()
	{
		return stringVal.ToString();
	}
}
