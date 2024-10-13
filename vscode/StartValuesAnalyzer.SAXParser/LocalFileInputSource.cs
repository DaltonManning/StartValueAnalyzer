using System.IO;

namespace StartValuesAnalyzer.SAXParser;

public class LocalFileInputSource : IInputSource
{
	private string fileName;

	private FileStream fileStream;

	public virtual string Source => fileName;

	public LocalFileInputSource(string fileName)
	{
		this.fileName = fileName;
		fileStream = null;
	}

	public virtual Stream MakeStream()
	{
		fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
		return fileStream;
	}
}
