namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CWordVal : CMemoryCell
{
	private int wordVal;

	public int WordVal
	{
		get
		{
			return wordVal;
		}
		set
		{
			wordVal = value;
		}
	}

	public CWordVal(int val)
	{
		wordVal = val;
	}

	public override string ToString()
	{
		return wordVal.ToString();
	}
}
