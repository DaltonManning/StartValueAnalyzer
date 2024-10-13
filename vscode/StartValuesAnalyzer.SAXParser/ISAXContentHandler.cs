namespace StartValuesAnalyzer.SAXParser;

public interface ISAXContentHandler
{
	void StartElement(string elementName, SAXAttributes attributes);

	void EndElement(string elementName);

	void Characters(string charcters);
}
