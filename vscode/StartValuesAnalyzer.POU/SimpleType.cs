using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public class SimpleType : DataType
{
	public SimpleType(string name)
		: base(name)
	{
	}

	public override bool GetVariableFromPath(string path, ref DataInst dataInst, ref ushort pouInstanceIndex, ref tMemReference memRef, ref bool writeAllowed, ref bool cvStatus)
	{
		return path.Length == 0;
	}

	public override void GetRetainProperties(string path, CProgramInstance programInstance, ushort pouInstanceIndex, tMemReference memRef, List<RetainProperties> retainProperties)
	{
		if (!accessible)
		{
			return;
		}
		bool IsValid = false;
		tMemoryLoc MemLoc = default(tMemoryLoc);
		programInstance.GetRuntimeAddr(pouInstanceIndex, memRef, out MemLoc, out IsValid, Resolve: true);
		if (IsValid)
		{
			bool isRetain = false;
			bool isColdRetain = false;
			bool isRead = false;
			bool isWritten = false;
			programInstance.GetRTAttributes(MemLoc, out isRetain, out isColdRetain, out var initValStr, out isRead, out isWritten);
			if (isRetain || isColdRetain)
			{
				retainProperties.Add(new RetainProperties(path, isRetain, isColdRetain, initValStr, memRef.ValType, isRead, isWritten));
			}
		}
	}
}
