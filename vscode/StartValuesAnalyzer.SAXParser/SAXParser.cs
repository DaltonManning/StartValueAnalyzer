using System;
using System.Xml;

namespace StartValuesAnalyzer.SAXParser;

public class SAXParser
{
	public class SAXException : Exception
	{
		public SAXException(string message)
			: base(message)
		{
		}
	}

	public class SAXNotRecognizedException : Exception
	{
		public SAXNotRecognizedException(string message)
			: base(message)
		{
		}
	}

	public class SAXNotSupportedException : Exception
	{
		public SAXNotSupportedException(string message)
			: base(message)
		{
		}
	}

	private ISAXContentHandler contentHandler;

	public ISAXContentHandler ContentHandler
	{
		set
		{
			contentHandler = value;
		}
	}

	public void Parse(IInputSource inputSource)
	{
		if (contentHandler == null)
		{
			throw new NullReferenceException("Content handler not set.");
		}
		XmlTextReader xmlTextReader = null;
		try
		{
			xmlTextReader = new XmlTextReader(inputSource.MakeStream());
			while (xmlTextReader.Read())
			{
				switch (xmlTextReader.NodeType)
				{
				case XmlNodeType.Element:
				{
					SAXAttributes sAXAttributes = new SAXAttributes();
					string name = xmlTextReader.Name;
					bool isEmptyElement = xmlTextReader.IsEmptyElement;
					if (xmlTextReader.HasAttributes)
					{
						for (int i = 0; i < xmlTextReader.AttributeCount; i++)
						{
							xmlTextReader.MoveToAttribute(i);
							sAXAttributes.Add(xmlTextReader.Name, xmlTextReader.Value);
						}
					}
					contentHandler.StartElement(name, sAXAttributes);
					if (isEmptyElement)
					{
						contentHandler.EndElement(name);
					}
					break;
				}
				case XmlNodeType.EndElement:
					contentHandler.EndElement(xmlTextReader.Name);
					break;
				case XmlNodeType.Text:
					contentHandler.Characters(xmlTextReader.Value);
					break;
				}
			}
		}
		finally
		{
			xmlTextReader?.Close();
		}
	}
}
