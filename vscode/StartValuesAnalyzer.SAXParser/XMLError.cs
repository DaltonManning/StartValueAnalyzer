using System.Collections.Generic;
using System.Text;

namespace StartValuesAnalyzer.SAXParser;

public class XMLError
{
	private XMLErrorCode errorCode;

	private string message;

	public XMLErrorCode ErrorCode => errorCode;

	public string ErrorMessage => message;

	public bool ErrorSet
	{
		get
		{
			if (errorCode == XMLErrorCode.XMLSuccess)
			{
				return message.Length > 0;
			}
			return true;
		}
	}

	public XMLError()
	{
		errorCode = XMLErrorCode.XMLSuccess;
		message = "";
	}

	public void ReportError(XMLErrorCode errorCode, string errorMessage)
	{
		this.errorCode = errorCode;
		message = errorMessage;
	}

	public void ReportStartElementError(string parentElementName, string childElementName)
	{
		errorCode = XMLErrorCode.XMLUnexpectedStartElement;
		message = "Current element: '" + parentElementName + "' Child element: '" + childElementName + "'";
	}

	public void ReportAttributeError(string elementName, SAXAttributes attributes)
	{
		errorCode = XMLErrorCode.XMLInvalidAttributeValue;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Element: '");
		stringBuilder.Append(elementName);
		stringBuilder.Append("' Attributes: ");
		bool flag = false;
		foreach (KeyValuePair<string, string> attribute in attributes)
		{
			if (flag)
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(attribute.Key);
			stringBuilder.Append("=\"");
			stringBuilder.Append(attribute.Value);
			stringBuilder.Append("\"");
			flag = true;
		}
		message = stringBuilder.ToString();
	}
}
