using StartValuesAnalyzer.ModApplic;

namespace StartValuesAnalyzer.POU;

public class CMInst : ModInst
{
	public override string Name
	{
		get
		{
			if (type != null && type.GetType() == typeof(RootModType))
			{
				return type.SCU.Name;
			}
			return name;
		}
		set
		{
			name = value;
		}
	}

	public override tObjTypeClass ObjTypeClass => tObjTypeClass.CMTypeClass;

	public CMInst(string name)
		: base(name)
	{
	}

	public override POUType CreateSingleType()
	{
		return (POUType)(base.SingleTypeRef = new SingleModType());
	}

	public RootModType CreateRootModType(ApplicationUnit application)
	{
		RootModType rootModType = (RootModType)(base.SingleTypeRef = new RootModType());
		rootModType.SCU = application;
		return rootModType;
	}
}
