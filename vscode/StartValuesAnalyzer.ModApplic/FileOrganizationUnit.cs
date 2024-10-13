namespace StartValuesAnalyzer.ModApplic;

public abstract class FileOrganizationUnit
{
	private string name;

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public FileOrganizationUnit(string name)
	{
		this.name = name;
	}
}
