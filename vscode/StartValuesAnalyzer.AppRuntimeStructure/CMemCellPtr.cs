namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CMemCellPtr : CMemoryCell
{
	private CMemoryCell memCellPtr;

	public CMemoryCell MemCellPtr
	{
		get
		{
			return memCellPtr;
		}
		set
		{
			memCellPtr = value;
		}
	}

	public CMemCellPtr(CMemoryCell val)
	{
		memCellPtr = val;
	}

	public override string ToString()
	{
		return "MemCellPtr";
	}
}
