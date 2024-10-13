namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CIntVal : CMemoryCell
{
	private int intVal;

	public int IntVal
	{
		get
		{
			return intVal;
		}
		set
		{
			intVal = value;
		}
	}

	public CIntVal(int val)
	{
		intVal = val;
	}

	public override string ToString()
	{
		return intVal.ToString();
	}
}
