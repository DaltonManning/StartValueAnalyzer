namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CTypeArray
{
	private ushort m_NrOfTypes;

	private tMemoryCellType[] m_Types;

	public CTypeArray(ushort nrOfTypes)
	{
		m_NrOfTypes = nrOfTypes;
		m_Types = new tMemoryCellType[m_NrOfTypes];
		for (int i = 0; i < nrOfTypes; i++)
		{
			m_Types[i] = new tMemoryCellType(tMemoryCellSimpleType.UndefMCType);
		}
	}

	public ushort GetNrOfTypes()
	{
		return m_NrOfTypes;
	}

	public tMemoryCellType GetMemCellType(ushort index)
	{
		return m_Types[index];
	}

	public void SetMemCellType(ushort index, tMemoryCellType MemoryCellType)
	{
		m_Types[index] = MemoryCellType;
	}
}
