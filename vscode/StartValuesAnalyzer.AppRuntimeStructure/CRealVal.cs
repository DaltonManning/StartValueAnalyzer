namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CRealVal : CMemoryCell
{
	private double realVal;

	public double RealVal
	{
		get
		{
			return realVal;
		}
		set
		{
			realVal = value;
		}
	}

	public CRealVal(double val)
	{
		realVal = val;
	}

	public override string ToString()
	{
		return realVal.ToString();
	}
}
