namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CPOUInstRef : CMemoryCell
{
	private CPOUInstance pouInstRef;

	public CPOUInstance POUInstRef
	{
		get
		{
			return pouInstRef;
		}
		set
		{
			pouInstRef = value;
		}
	}

	public CPOUInstRef(CPOUInstance val)
	{
		pouInstRef = val;
	}

	public override string ToString()
	{
		return "POUInstRef";
	}
}
