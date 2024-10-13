using System.Text;

namespace StartValuesAnalyzer.SAXParser;

public abstract class SAXContentHandlerBase : ISAXContentHandler
{
	private SAXElementBase currentElement;

	private readonly SAXElementBase rootElement;

	public SAXContentHandlerBase(SAXElementBase rootElement)
	{
		this.rootElement = rootElement;
		currentElement = rootElement;
	}

	public virtual void StartElement(string elementName, SAXAttributes attributes)
	{
		if (currentElement.StartElement(elementName, attributes))
		{
			currentElement = currentElement.Child;
		}
		else
		{
			ReportError();
		}
	}

	public virtual void EndElement(string elementName)
	{
		if (currentElement.EndElement(elementName))
		{
			currentElement = currentElement.Parent;
		}
		else
		{
			ReportError();
		}
	}

	public virtual void Characters(string charcters)
	{
		if (!currentElement.Characters(charcters))
		{
			ReportError();
		}
	}

	private void ReportError()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XMLError errorHandler = currentElement.ErrorHandler;
		switch (errorHandler.ErrorCode)
		{
		case XMLErrorCode.XMLOutOfMemory:
			stringBuilder.Append("An Out of memory exception occurred while creating a ");
			stringBuilder.Append(errorHandler.ErrorMessage);
			throw new SAXParser.SAXException(stringBuilder.ToString());
		case XMLErrorCode.XMLUnexpectedStartElement:
			stringBuilder.Append("Unexpected start element (");
			stringBuilder.Append(errorHandler.ErrorMessage);
			stringBuilder.Append(")");
			throw new SAXParser.SAXNotRecognizedException(stringBuilder.ToString());
		case XMLErrorCode.XMLUnexpectedEndElement:
			stringBuilder.Append("Unexpected end element (");
			stringBuilder.Append(errorHandler.ErrorMessage);
			stringBuilder.Append(")");
			throw new SAXParser.SAXNotRecognizedException(stringBuilder.ToString());
		case XMLErrorCode.XMLInvalidAttributeValue:
			stringBuilder.Append("Invalid attribute value (");
			stringBuilder.Append(errorHandler.ErrorMessage);
			stringBuilder.Append(")");
			throw new SAXParser.SAXNotRecognizedException(stringBuilder.ToString());
		case XMLErrorCode.XMLUnsupportedSyntaxVersion:
			stringBuilder.Append("File has an unsupported XML syntax version. ");
			stringBuilder.Append(errorHandler.ErrorMessage);
			stringBuilder.Append(" Make a dummy change and download the application again.");
			throw new SAXParser.SAXNotSupportedException(stringBuilder.ToString());
		case XMLErrorCode.XMLSubscriptOutOfRange:
			stringBuilder.Append("A buffer in method ");
			stringBuilder.Append(errorHandler.ErrorMessage);
			stringBuilder.Append(" has reached its limit.");
			throw new SAXParser.SAXException(stringBuilder.ToString());
		case XMLErrorCode.XMLFailToCreate:
			stringBuilder.Append("Failed to create ");
			stringBuilder.Append(errorHandler.ErrorMessage);
			throw new SAXParser.SAXException(stringBuilder.ToString());
		case XMLErrorCode.XMLGeneralError:
			stringBuilder.Append("An undefind error occured while parsing element ");
			stringBuilder.Append(errorHandler.ErrorMessage);
			throw new SAXParser.SAXException(stringBuilder.ToString());
		case XMLErrorCode.XMLSuccess:
		case XMLErrorCode.XMLFileNotFound:
		case XMLErrorCode.XMLIncorrectCheckSum:
		case XMLErrorCode.XMLNotRecognized:
		case XMLErrorCode.XMLParseError:
			break;
		}
	}
}
