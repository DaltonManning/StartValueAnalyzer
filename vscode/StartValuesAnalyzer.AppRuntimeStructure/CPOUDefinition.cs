namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CPOUDefinition
{
	private string m_Name;

	private ushort m_NrOfParameters;

	private ushort m_NrOfVariables;

	private ushort m_NrOfLiterals;

	private ushort m_NrOfFBsAndProgs;

	private ushort m_NrOfMultiParameters;

	private ushort m_NrOfCompositeObjectVariables;

	private ushort m_StartOfSysGenVariables;

	private ushort m_NrParsVarsFBsAndProgs;

	private CTypeArray m_pTypeArray;

	private CInitValueTable m_pInitTable;

	private CVariableAccessArray m_VariableAccessArray;

	public ushort GetNrOfParameters => m_NrOfParameters;

	public ushort GetNrOfVariables => m_NrOfVariables;

	public ushort GetNrOfLiterals => m_NrOfLiterals;

	public ushort GetNrOfFBsAndProgs => m_NrOfFBsAndProgs;

	public ushort GetNrOfMultiParameters => m_NrOfMultiParameters;

	public ushort GetNrOfCompositeObjectVariables => m_NrOfCompositeObjectVariables;

	public ushort GetStartOfSysGenVariables => m_StartOfSysGenVariables;

	public ushort GetStartOfMultiParameters => (ushort)(m_NrParsVarsFBsAndProgs + 1);

	public CTypeArray GetTypeArray => m_pTypeArray;

	public CVariableAccessArray GetVariableAccessArray => m_VariableAccessArray;

	public CInitValueTable GetInitValueTable => m_pInitTable;

	public string Name => m_Name;

	public CPOUDefinition(string name, ushort noOfParameters, ushort noOfVariables, ushort noOfLiterals, ushort noOfFBsAndProgs, ushort noOfMultiParameters, ushort noOfCompositeObjectVariables, ushort startOfSysGenVariables)
	{
		m_Name = name;
		m_NrOfParameters = noOfParameters;
		m_NrOfVariables = noOfVariables;
		m_NrOfLiterals = noOfLiterals;
		m_NrOfFBsAndProgs = noOfFBsAndProgs;
		m_NrOfMultiParameters = noOfMultiParameters;
		m_NrOfCompositeObjectVariables = noOfCompositeObjectVariables;
		m_StartOfSysGenVariables = startOfSysGenVariables;
		m_NrParsVarsFBsAndProgs = (ushort)(noOfParameters + noOfVariables + noOfFBsAndProgs);
	}

	public int GetNrOfMemoryCells()
	{
		return GetNrOfMemoryCells(0);
	}

	public int GetNrOfMemoryCells(int MultiSize)
	{
		return GetMaxMemoryCellAddress(MultiSize) + GetNrOfCellsForOwnerExt(MultiSize);
	}

	public int GetMaxMemoryCellAddress()
	{
		return GetMaxMemoryCellAddress(0);
	}

	public int GetMaxMemoryCellAddress(int MultiSize)
	{
		return m_NrParsVarsFBsAndProgs + m_NrOfMultiParameters * MultiSize + m_NrOfCompositeObjectVariables;
	}

	public CTypeArray CreateTypeArray()
	{
		return m_pTypeArray = new CTypeArray((ushort)(m_NrOfVariables + m_NrOfMultiParameters));
	}

	public CVariableAccessArray CreateVariableAccessArray()
	{
		return m_VariableAccessArray = new CVariableAccessArray(m_NrOfVariables + m_NrOfMultiParameters);
	}

	public CInitValueTable CreateInitValueTable()
	{
		ushort nrOfFBsAndProgs = (ushort)((m_NrOfCompositeObjectVariables > 0) ? m_NrOfFBsAndProgs : 0);
		return m_pInitTable = new CInitValueTable((ushort)(m_NrOfVariables + m_NrOfMultiParameters), nrOfFBsAndProgs, m_NrOfCompositeObjectVariables);
	}

	public ushort ToTypeIdx(ushort Offset)
	{
		if (Offset <= m_NrOfParameters + m_NrOfVariables)
		{
			return (ushort)(Offset - m_NrOfParameters - 1);
		}
		return (ushort)(m_NrOfVariables + (Offset - m_NrParsVarsFBsAndProgs - 1) % m_NrOfMultiParameters);
	}

	private int GetNrOfCellsForOwnerExt(int MultiSize)
	{
		int num = ((m_NrOfMultiParameters != 0) ? (m_NrOfVariables + m_NrOfFBsAndProgs + m_NrOfMultiParameters * MultiSize) : (m_NrOfVariables - m_NrOfLiterals));
		int num2 = num / 4;
		if ((num & 3) > 0)
		{
			num2++;
		}
		return num2;
	}
}
