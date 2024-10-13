namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CInitValueTable
{
	private ushort m_NrOfVars;

	private ushort m_NrOfFBsAndProgs;

	private ushort m_NrOfComposites;

	private CMemoryCell[] m_Values;

	public CInitValueTable(ushort NrOfVars, ushort NrOfFBsAndProgs, ushort NrOfComposites)
	{
		m_NrOfVars = NrOfVars;
		m_NrOfFBsAndProgs = NrOfFBsAndProgs;
		m_NrOfComposites = NrOfComposites;
		m_Values = new CMemoryCell[NrOfVars + NrOfFBsAndProgs + NrOfComposites];
	}

	public CMemoryCell GetMemoryCell(ushort index)
	{
		return m_Values[index];
	}

	public void SetMemoryCell(ushort index, CMemoryCell memCell)
	{
		m_Values[index] = memCell;
	}

	private ushort GetNrOfVars()
	{
		return m_NrOfVars;
	}
}
