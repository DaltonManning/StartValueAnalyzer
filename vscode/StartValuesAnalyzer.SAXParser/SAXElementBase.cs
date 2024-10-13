namespace StartValuesAnalyzer.SAXParser;

public abstract class SAXElementBase
{
	private readonly SAXElementBase parent;

	private SAXElementBase child;

	public SAXElementBase Parent => parent;

	public SAXElementBase Child
	{
		get
		{
			return child;
		}
		set
		{
			child = value;
		}
	}

	public abstract string ElementName { get; }

	public virtual XMLError ErrorHandler => parent.ErrorHandler;

	public SAXElementBase()
	{
	}

	public SAXElementBase(SAXElementBase parent)
	{
		this.parent = parent;
	}

	public virtual bool StartElement(string elementName, SAXAttributes attributes)
	{
		ErrorHandler.ReportStartElementError(ElementName, elementName);
		return false;
	}

	public virtual bool EndElement(string elementName)
	{
		return true;
	}

	public virtual bool HandleAttributes(SAXAttributes attributes)
	{
		return true;
	}

	public virtual bool Characters(string characters)
	{
		return true;
	}
}
