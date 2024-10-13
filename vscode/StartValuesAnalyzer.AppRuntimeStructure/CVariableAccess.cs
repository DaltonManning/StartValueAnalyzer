namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CVariableAccess
{
	private bool m_isRead;

	private bool m_isWritten;

	private bool m_isDefined;

	public bool IsRead
	{
		get
		{
			return m_isRead;
		}
		set
		{
			m_isRead = value;
		}
	}

	public bool IsWritten
	{
		get
		{
			return m_isWritten;
		}
		set
		{
			m_isWritten = value;
		}
	}

	public bool IsDefined
	{
		get
		{
			return m_isDefined;
		}
		set
		{
			m_isDefined = value;
		}
	}

	public CVariableAccess(bool isRead, bool isWritten, bool isDefined)
	{
		m_isRead = isRead;
		m_isWritten = isWritten;
		m_isDefined = isDefined;
	}
}
