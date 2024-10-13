using System;
using System.IO;

namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CProgramInstance
{
	private string m_Name;

	private CProgramDefinition m_pProgramDefinition;

	private CProgramMemory m_pProgramMemory;

	private ushort InvalidMemoryIndex = ushort.MaxValue;

	public string GetName => m_Name;

	public CProgramDefinition ProgramDefinition => m_pProgramDefinition;

	public CProgramMemory ProgramMemory => m_pProgramMemory;

	public CProgramInstance(string name)
	{
		m_Name = name;
	}

	public CProgramDefinition CreateProgramDefinition(ushort nrOfPOUDefinitions)
	{
		return m_pProgramDefinition = new CProgramDefinition(nrOfPOUDefinitions);
	}

	public CProgramMemory CreateProgramMemory(ushort nrOfInstances, ulong totalOpSaveSize)
	{
		m_pProgramMemory = new CProgramMemory(nrOfInstances, m_pProgramDefinition, totalOpSaveSize);
		return m_pProgramMemory;
	}

	private void GraphVarAddrToRuntAddr(ushort POUInstanceNr, ushort VarOffset, out tMemoryLoc NewMemLoc, out bool AddrValid)
	{
		NewMemLoc.POUInstance = POUInstanceNr;
		NewMemLoc.VarOffset = VarOffset;
		AddrValid = true;
	}

	private void GraphParAddrToRuntAddr(ushort POUInstanceNr, ushort ParOffset, ushort RecOffset, out tMemoryLoc NewMemLoc, out bool AddrValid)
	{
		CPOUInstance pOUInstance = m_pProgramMemory.GetPOUInstance(POUInstanceNr);
		CMemoryCell memoryCell = pOUInstance.GetMemoryCell((ushort)(ParOffset - 1));
		if (memoryCell is CMemVal)
		{
			NewMemLoc = ((CMemVal)memoryCell).MemVal;
			NewMemLoc.VarOffset += RecOffset;
			AddrValid = true;
			return;
		}
		throw new ArgumentException("GraphParAddrToRuntAddr");
	}

	private void GraphPtrAddrToRuntAddr(ushort POUInstanceNr, ushort ParOffset, ushort RecOffset, out tMemoryLoc NewMemLoc, out bool AddrValid)
	{
		GraphParAddrToRuntAddr(POUInstanceNr, ParOffset, RecOffset, out NewMemLoc, out AddrValid);
		if (NewMemLoc.VarOffset == InvalidMemoryIndex || NewMemLoc.POUInstance == InvalidMemoryIndex)
		{
			NewMemLoc.POUInstance = InvalidMemoryIndex;
			NewMemLoc.VarOffset = InvalidMemoryIndex;
			AddrValid = false;
		}
	}

	public void GetRTAttributes(tMemoryLoc MemLoc, out bool isRetain, out bool isColdRetain, out string initValStr, out bool isRead, out bool isWritten)
	{
		CPOUDefinition pOUDefinition = m_pProgramMemory.GetPOUInstance(MemLoc.POUInstance).POUDefinition;
		if (MemLoc.VarOffset > (ushort)(pOUDefinition.GetNrOfParameters + pOUDefinition.GetNrOfVariables))
		{
			throw new ArgumentException("GetRTAttributes, VarOffset out of range");
		}
		ushort index = (ushort)(MemLoc.VarOffset - pOUDefinition.GetNrOfParameters - 1);
		tMemoryCellType memCellType = pOUDefinition.GetTypeArray.GetMemCellType(index);
		isColdRetain = memCellType.ColdRetain;
		isRetain = memCellType.Retain;
		isRead = pOUDefinition.GetVariableAccessArray.GetVariableAccess(index).IsRead;
		isWritten = pOUDefinition.GetVariableAccessArray.GetVariableAccess(index).IsWritten;
		memCellType.GetSimpleType();
		initValStr = pOUDefinition.GetInitValueTable.GetMemoryCell(index).ToString();
	}

	public void GetRuntimeAddr(ushort IH, tMemReference MemRef, out tMemoryLoc MemLoc, out bool IsValid, bool Resolve)
	{
		switch (MemRef.Mode)
		{
		case tAddressingMode.DirectMode:
			GraphVarAddrToRuntAddr(IH, MemRef.UU.Offset, out MemLoc, out IsValid);
			break;
		case tAddressingMode.InDirectMode:
			GraphParAddrToRuntAddr(IH, MemRef.UU.U2.mOffset, MemRef.UU.U2.rOffset, out MemLoc, out IsValid);
			break;
		case tAddressingMode.PtrInDirectMode:
			if (Resolve)
			{
				GraphPtrAddrToRuntAddr(IH, MemRef.UU.U2.mOffset, MemRef.UU.U2.rOffset, out MemLoc, out IsValid);
			}
			else
			{
				GraphVarAddrToRuntAddr(IH, MemRef.UU.Offset, out MemLoc, out IsValid);
			}
			break;
		case tAddressingMode.Absolutemode:
			GraphVarAddrToRuntAddr(MemRef.UU.MemVal.POUInstance, MemRef.UU.MemVal.VarOffset, out MemLoc, out IsValid);
			break;
		default:
			throw new ArgumentException("GetRuntimeAddr");
		}
	}

	public void PrintVarMem()
	{
		using FileStream stream = File.Create(m_Name + "VarMem.txt");
		using StreamWriter sw = new StreamWriter(stream);
		m_pProgramMemory.PrintProgMemToFile(sw);
	}
}
