using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public class CommunicationVariable : DataInst
{
	private tParDirection direction;

	public tParDirection Direction
	{
		set
		{
			direction = value;
		}
	}

	public CommunicationVariable(string name)
		: base(name)
	{
		direction = tParDirection.vInPar;
	}

	protected override void GetMemRef(ref tMemReference memRef, ushort externalOffset)
	{
		memRef.ValType = ((DataType)type).ValType;
		memRef.Mode = tAddressingMode.PtrInDirectMode;
		memRef.UU.U2.mOffset = varOffset;
		memRef.UU.U2.rOffset = (ushort)(externalOffset + 1);
	}

	public override bool GetVariableFromPath(string path, ref DataInst dataInst, ref ushort pouInstanceIndex, ref tMemReference memRef, ref bool writeAllowed, ref bool cvStatus)
	{
		if (path.Length > 0 && path.IndexOf('#') == 0)
		{
			dataInst = this;
			GetMemRef(ref memRef);
			memRef.ValType = tValType.DWordType;
			memRef.UU.U2.rOffset = 0;
			writeAllowed = false;
			cvStatus = true;
			return true;
		}
		return base.GetVariableFromPath(path, ref dataInst, ref pouInstanceIndex, ref memRef, ref writeAllowed, ref cvStatus);
	}

	public override void GetRetainProperties(string path, CProgramInstance programInstance, ushort pouInstanceIndex, tMemReference memRef, List<RetainProperties> retainProperties)
	{
		if (Name.StartsWith("__"))
		{
			return;
		}
		int count = retainProperties.Count;
		base.GetRetainProperties(path, programInstance, pouInstanceIndex, memRef, retainProperties);
		if (count == retainProperties.Count)
		{
			return;
		}
		bool IsValid = false;
		tMemoryLoc MemLoc = default(tMemoryLoc);
		GetMemRef(ref memRef);
		memRef.ValType = tValType.DWordType;
		memRef.UU.U2.rOffset = 0;
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
				string path2 = path + "." + Name + ".#Status";
				retainProperties.Add(new RetainProperties(path2, isRetain, isColdRetain, initValStr, memRef.ValType, isRead, isWritten));
			}
		}
	}
}
