namespace StartValuesAnalyzer;

internal class CompareFilesValues
{
	private string m_value1;

	private string m_value2;

	public string Value1
	{
		get
		{
			return m_value1;
		}
		set
		{
			m_value1 = value;
		}
	}

	public string Value2
	{
		get
		{
			return m_value2;
		}
		set
		{
			m_value2 = value;
		}
	}

	public CompareFilesValues(string val1, string val2)
	{
		m_value1 = val1;
		m_value2 = val2;
	}

	public bool EqualValues()
	{
		return m_value1 == m_value2;
	}
}
