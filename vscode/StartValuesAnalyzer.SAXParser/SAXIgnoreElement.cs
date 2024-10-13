namespace StartValuesAnalyzer.SAXParser;

public class SAXIgnoreElement : SAXElementBase
{
	public override string ElementName => "SAXIgnoreElement";

	public SAXIgnoreElement(SAXElementBase parent)
		: base(parent)
	{
	}

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		SAXElementBase sAXElementBase = base.Child;
		if (sAXElementBase == null)
		{
			sAXElementBase = new SAXIgnoreElement(this);
			base.Child = sAXElementBase;
		}
		return true;
	}

	public override bool EndElement(string elementName)
	{
		return true;
	}
}
