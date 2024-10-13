namespace StartValuesAnalyzer.AppRuntimeStructure;

public class RetainProperties
{
	private string m_Path;

	private bool m_Retain;

	private bool m_ColdRetain;

	private string m_InitValue;

	private tValType m_valtype;

	private bool m_IsRead;

	private bool m_IsWritten;

	private bool m_Communicated;

	private string m_Quality;

	private string m_CommunicatedValue;

	public tValType ValType
	{
		get
		{
			return m_valtype;
		}
		set
		{
			m_valtype = value;
		}
	}

	public string ValTypeAsString
	{
		get
		{
			string text = m_valtype.ToString().Replace("Type", "");
			if (text == "Duration")
			{
				text = "time";
			}
			else if (text == "Time")
			{
				text = "date_and_time";
			}
			return text.ToLower();
		}
	}

	public string Quality => m_Quality;

	public string CommunicatedValue => m_CommunicatedValue;

	public string Path
	{
		get
		{
			return m_Path;
		}
		set
		{
			m_Path = value;
		}
	}

	public bool Retain
	{
		get
		{
			return m_Retain;
		}
		set
		{
			m_Retain = value;
		}
	}

	public bool ColdRetain
	{
		get
		{
			return m_ColdRetain;
		}
		set
		{
			m_ColdRetain = value;
		}
	}

	public string InitValue
	{
		get
		{
			return m_InitValue;
		}
		set
		{
			m_InitValue = value;
		}
	}

	public bool Read
	{
		get
		{
			return m_IsRead;
		}
		set
		{
			m_IsRead = value;
		}
	}

	public bool Written
	{
		get
		{
			return m_IsWritten;
		}
		set
		{
			m_IsWritten = value;
		}
	}

	public RetainProperties()
	{
		m_Path = "";
		m_Retain = false;
		m_ColdRetain = false;
		m_InitValue = "";
		m_IsRead = false;
		m_IsWritten = false;
		m_valtype = tValType.UndefType;
		m_Communicated = false;
		m_Quality = "bad";
		m_CommunicatedValue = "";
	}

	public RetainProperties(string path, bool retain, bool coldRetain, string initValue, tValType valType, bool isRead, bool isWritten)
	{
		m_Path = path;
		m_Retain = retain;
		m_ColdRetain = coldRetain;
		m_InitValue = initValue;
		m_valtype = valType;
		m_IsRead = isRead;
		m_IsWritten = isWritten;
		m_Communicated = false;
		m_Quality = "Bad";
		m_CommunicatedValue = "";
	}

	public void SetCommunicatedProperties(string quality, string theValue)
	{
		m_Communicated = true;
		m_Quality = quality;
		m_CommunicatedValue = theValue;
	}
}
