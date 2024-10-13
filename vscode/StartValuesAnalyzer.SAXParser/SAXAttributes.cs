using System;
using System.Collections.Generic;
using System.Xml;

namespace StartValuesAnalyzer.SAXParser;

public class SAXAttributes : Dictionary<string, string>
{
	public bool GetAttribute(string attributeName, ref string attributeValue)
	{
		return TryGetValue(attributeName, out attributeValue);
	}

	public bool GetAttribute(string attributeName, ref long attributeValue)
	{
		string attributeValue2 = "";
		bool attribute = GetAttribute(attributeName, ref attributeValue2);
		if (attribute)
		{
			attributeValue = XmlConvert.ToInt64(attributeValue2);
		}
		return attribute;
	}

	public bool GetAttribute(string attributeName, ref int attributeValue)
	{
		string attributeValue2 = "";
		bool attribute = GetAttribute(attributeName, ref attributeValue2);
		if (attribute)
		{
			attributeValue = XmlConvert.ToInt32(attributeValue2);
		}
		return attribute;
	}

	public bool GetAttribute(string attributeName, ref double attributeValue)
	{
		string attributeValue2 = "";
		bool attribute = GetAttribute(attributeName, ref attributeValue2);
		if (attribute)
		{
			attributeValue = XmlConvert.ToDouble(attributeValue2);
		}
		return attribute;
	}

	public bool GetAttribute(string attributeName, ref bool attributeValue)
	{
		string attributeValue2 = "";
		bool attribute = GetAttribute(attributeName, ref attributeValue2);
		if (attribute)
		{
			attributeValue = XmlConvert.ToBoolean(attributeValue2);
		}
		return attribute;
	}

	public bool GetAttribute(string attributeName, ref Guid attributeValue)
	{
		string attributeValue2 = "";
		bool attribute = GetAttribute(attributeName, ref attributeValue2);
		if (attribute)
		{
			attributeValue = new Guid(attributeValue2);
		}
		return attribute;
	}

	public int GetAttributeAsInt(string attributeName)
	{
		int attributeValue = 0;
		GetAttribute(attributeName, ref attributeValue);
		return attributeValue;
	}

	public string GetAttributeAsString(string attributeName)
	{
		string attributeValue = "";
		GetAttribute(attributeName, ref attributeValue);
		return attributeValue;
	}

	public bool GetAttributeAsBool(string attributeName)
	{
		bool attributeValue = false;
		GetAttribute(attributeName, ref attributeValue);
		return attributeValue;
	}

	public double GetAttributeAsDouble(string attributeName)
	{
		double attributeValue = 0.0;
		GetAttribute(attributeName, ref attributeValue);
		return attributeValue;
	}
}
