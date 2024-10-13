using System;
using System.IO;
using System.Xml;

namespace StartValuesAnalyzer.SAXParser;

public class XMLUtility
{
	public void StartDecompressingSAXParser(SAXContentHandlerBase contentHandler, string path)
	{
		if (!File.Exists(path))
		{
			throw new ArgumentException("File '" + path + "' was not found.");
		}
		CompressedLocalFileInputSource inputSource = new CompressedLocalFileInputSource(path);
		StartSAXParser(contentHandler, path, inputSource);
	}

	public void StartSAXParser(SAXContentHandlerBase contentHandler, string path)
	{
		if (!File.Exists(path))
		{
			throw new ArgumentException("File '" + path + "' was not found.");
		}
		LocalFileInputSource inputSource = new LocalFileInputSource(path);
		StartSAXParser(contentHandler, path, inputSource);
	}

	private void StartSAXParser(SAXContentHandlerBase contentHandler, string path, IInputSource inputSource)
	{
		SAXParser sAXParser = new SAXParser();
		sAXParser.ContentHandler = contentHandler;
		try
		{
			sAXParser.Parse(inputSource);
		}
		catch (XmlException ex)
		{
			string message = "An error occurred when parsing file " + inputSource.Source + ". Message: " + ex.Message + " Line number: " + ex.LineNumber + ". Position: " + ex.LinePosition;
			throw new Exception(message, ex);
		}
		catch (SAXParser.SAXNotRecognizedException ex2)
		{
			string message2 = "An error occurred when parsing file " + inputSource.Source + ". Message: " + ex2.Message;
			throw new Exception(message2, ex2);
		}
		catch (SAXParser.SAXNotSupportedException ex3)
		{
			string message3 = "An error occurred when parsing file " + inputSource.Source + ". Message: " + ex3.Message;
			throw new Exception(message3, ex3);
		}
		catch (SAXParser.SAXException ex4)
		{
			string message4 = "An error occurred when parsing file " + inputSource.Source + ". Message: " + ex4.Message;
			throw new Exception(message4, ex4);
		}
		catch (Exception ex5)
		{
			string message5 = "An error occurred when parsing file " + inputSource.Source + ". Message: " + ex5.Message;
			throw new Exception(message5, ex5);
		}
	}
}
