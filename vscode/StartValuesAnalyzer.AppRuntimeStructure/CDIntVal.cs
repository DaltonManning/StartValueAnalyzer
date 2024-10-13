namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CDIntVal : CMemoryCell
{
	private int dintVal;

	public int DIntVal
	{
		get
		{
			return dintVal;
		}
		set
		{
			dintVal = value;
		}
	}

	public CDIntVal(int val)
	{
		dintVal = val;
	}

	public override string ToString()
	{
		return dintVal.ToString();
	}
}
