namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CVariableAccessArray
{
	private int m_NrOfVars;

	private CVariableAccess[] m_AccessArr;

	public CVariableAccessArray(int nrOfVars)
	{
		m_NrOfVars = nrOfVars;
		m_AccessArr = new CVariableAccess[m_NrOfVars];
		for (int i = 0; i < m_NrOfVars; i++)
		{
			m_AccessArr[i] = new CVariableAccess(isRead: false, isWritten: false, isDefined: false);
		}
	}

	public int GetNrOfVars()
	{
		return m_NrOfVars;
	}

	public CVariableAccess GetVariableAccess(int index)
	{
		if (index >= 0 && index < m_NrOfVars)
		{
			return m_AccessArr[index];
		}
		return null;
	}
}
