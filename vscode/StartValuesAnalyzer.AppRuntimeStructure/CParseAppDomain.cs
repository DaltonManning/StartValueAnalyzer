using System;
using System.Xml;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.AppRuntimeStructure;

internal class CParseAppDomain
{
	private string GetInnerText(XmlNode node)
	{
		string text = node.InnerText;
		if (text.IndexOf("#") >= 0)
		{
			text = text.Replace("#And;", "&");
			text = text.Replace("#More;", ">");
			text = text.Replace("#Less;", "<");
			text = text.Replace("#Nbr;", "#");
		}
		return text;
	}

	private string GetInitValueInnerText(XmlNode node)
	{
		string text = node.InnerText;
		if (text.IndexOf("#") >= 0)
		{
			text = text.Replace("#And;", "&");
			text = text.Replace("#More;", ">");
			text = text.Replace("#Less;", "<");
			text = text.Replace("#Nbr;", "#");
		}
		return text;
	}

	private string GetAttributeStr(XmlNode node, string attrName)
	{
		string result = "";
		try
		{
			if (node is XmlElement xmlElement)
			{
				result = xmlElement.GetAttribute(attrName);
			}
		}
		catch
		{
			result = "";
		}
		return result;
	}

	private string GetNumAttributeStr(XmlNode node, string attrName)
	{
		string text = GetAttributeStr(node, attrName);
		if (text.Length == 0)
		{
			text = "0";
		}
		return text;
	}

	private void RemoveCommentNodes(XmlDocument doc)
	{
		XmlNodeList xmlNodeList = doc.SelectNodes("//comment()");
		foreach (XmlNode item in xmlNodeList)
		{
			item.ParentNode.RemoveChild(item);
		}
	}

	public CProgramInstance Parse(string appDoaminFileName)
	{
		CProgramInstance cProgramInstance = null;
		CompressedFileStream compressedFileStream = null;
		try
		{
			compressedFileStream = new CompressedFileStream(appDoaminFileName);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(compressedFileStream);
			RemoveCommentNodes(xmlDocument);
			XmlNode firstChild = xmlDocument.DocumentElement.FirstChild;
			string attributeStr = GetAttributeStr((XmlElement)xmlDocument.DocumentElement.FirstChild, "Name");
			cProgramInstance = new CProgramInstance(attributeStr);
			XmlNode nextSibling = firstChild.FirstChild.NextSibling.NextSibling;
			ushort num = XmlConvert.ToUInt16(GetAttributeStr(nextSibling, "NrInst"));
			CProgramDefinition progDefs = cProgramInstance.CreateProgramDefinition(num);
			XmlNode xmlNode = nextSibling.FirstChild.FirstChild;
			for (ushort num2 = 1; num2 <= num; num2++)
			{
				ParsePOUDefinition(xmlNode.FirstChild, progDefs, num2);
				xmlNode = xmlNode.NextSibling;
			}
			XmlNode nextSibling2 = nextSibling.NextSibling;
			ushort num3 = XmlConvert.ToUInt16(GetAttributeStr(nextSibling2, "NrInst"));
			uint num4 = XmlConvert.ToUInt32(GetAttributeStr(nextSibling2, "OpSz"));
			CProgramMemory pouInsts = cProgramInstance.CreateProgramMemory(num3, num4);
			XmlNode xmlNode2 = nextSibling2.FirstChild.FirstChild;
			for (ushort num5 = 1; num5 <= num3; num5++)
			{
				ParsePOUInstance(xmlNode2, pouInsts);
				xmlNode2 = xmlNode2.NextSibling;
			}
			return cProgramInstance;
		}
		finally
		{
			compressedFileStream?.Close();
		}
	}

	private void ParsePOUInstance(XmlNode pouInstNode, CProgramMemory pouInsts)
	{
		ushort definitionNr = XmlConvert.ToUInt16(GetAttributeStr(pouInstNode, "DNr"));
		ushort pOUInstanceNr = XmlConvert.ToUInt16(GetAttributeStr(pouInstNode, "INr"));
		ushort multiSize = XmlConvert.ToUInt16(GetNumAttributeStr(pouInstNode, "MParSz"));
		CPOUInstance pouInst = pouInsts.CreatePOUInstance(pOUInstanceNr, definitionNr, multiSize);
		for (XmlNode xmlNode = pouInstNode.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			if (xmlNode.Name == "PIfo")
			{
				ParseParameterInfo(xmlNode, pouInst);
			}
			else if (xmlNode.Name == "Link")
			{
				ParseLink(xmlNode, pouInst);
			}
			else if (xmlNode.Name == "InOut")
			{
				ParseInOutPar(xmlNode, pouInst);
			}
			else if (!(xmlNode.Name == "GTup") && !(xmlNode.Name == "OExt") && !(xmlNode.Name == "MParam"))
			{
				throw new ArgumentOutOfRangeException("UNknown POUInst Child node");
			}
		}
	}

	private void ParseParameterInfo(XmlNode parInfoNode, CPOUInstance pouInst)
	{
		ushort pOUInstance = XmlConvert.ToUInt16(GetNumAttributeStr(parInfoNode, "PIdx"));
		string innerText = GetInnerText(parInfoNode);
		string[] array = innerText.Split(';');
		int num = array.Length - 1;
		if (num != pouInst.POUDefinition.GetNrOfParameters / 2)
		{
			throw new ArgumentOutOfRangeException("Wrong nr of parameters");
		}
		int num2 = 0;
		tMemoryLoc val = default(tMemoryLoc);
		for (int i = 0; i < num; i++)
		{
			string text = array[i];
			string[] array2 = text.Split(' ');
			if (array2.Length == 1)
			{
				val.POUInstance = pOUInstance;
				val.VarOffset = XmlConvert.ToUInt16(text);
			}
			else
			{
				if (array2.Length != 2)
				{
					throw new ArgumentOutOfRangeException("Wrong nr of parameters 2");
				}
				val.POUInstance = XmlConvert.ToUInt16(array2[0]);
				val.VarOffset = XmlConvert.ToUInt16(array2[1]);
			}
			pouInst.SetMemoryCell((ushort)num2, new CMemVal(val));
			num2 += 2;
		}
	}

	private void ParseInOutPar(XmlNode parInfoNode, CPOUInstance pouInst)
	{
		string innerText = GetInnerText(parInfoNode);
		string[] array = innerText.Split(';');
		int num = array.Length - 1;
		int num2 = 0;
		tMemoryLoc val = default(tMemoryLoc);
		for (int i = 0; i < num; i++)
		{
			string text = array[i];
			string[] array2 = text.Split(' ');
			num2 = XmlConvert.ToUInt16(array2[0]);
			val.VarOffset = XmlConvert.ToUInt16(array2[1]);
			val.POUInstance = XmlConvert.ToUInt16(array2[2]);
			pouInst.SetMemoryCell((ushort)num2, new CMemVal(val));
		}
	}

	private void ParseLink(XmlNode linkNode, CPOUInstance pouInst)
	{
		ushort num = XmlConvert.ToUInt16(GetNumAttributeStr(linkNode, "SIdx"));
		string innerText = GetInnerText(linkNode);
		string[] array = innerText.Split(';');
		int num2 = array.Length - 1;
		if (num2 != pouInst.POUDefinition.GetNrOfFBsAndProgs)
		{
			throw new ArgumentOutOfRangeException("Wrong nr of FBAndProgs");
		}
		tMemoryLoc val = default(tMemoryLoc);
		val.VarOffset = 0;
		ushort num3 = num;
		for (int i = 0; i < num2; i++)
		{
			string s = array[i];
			val.POUInstance = XmlConvert.ToUInt16(s);
			pouInst.SetMemoryCell(num3, new CMemVal(val));
			num3++;
		}
	}

	private void ParsePOUDefinition(XmlNode pouDefNode, CProgramDefinition progDefs, ushort defIndex)
	{
		CPOUDefinition cPOUDefinition = progDefs.CreatePOUDefinition(defIndex, GetAttributeStr((XmlElement)pouDefNode, "Name"), XmlConvert.ToUInt16(GetAttributeStr(pouDefNode, "NrPars")), XmlConvert.ToUInt16(GetAttributeStr(pouDefNode, "NrVars")), XmlConvert.ToUInt16(GetAttributeStr(pouDefNode, "NrLits")), XmlConvert.ToUInt16(GetAttributeStr(pouDefNode, "NrFBPr")), XmlConvert.ToUInt16(GetAttributeStr(pouDefNode, "NrMPar")), XmlConvert.ToUInt16(GetAttributeStr(pouDefNode, "NrCOVa")), XmlConvert.ToUInt16(GetAttributeStr(pouDefNode, "SGVIdx")));
		ParseTypeArray(pouDefNode.FirstChild, cPOUDefinition.CreateTypeArray());
		ParseInitArray(pouDefNode.FirstChild.NextSibling, cPOUDefinition.CreateInitValueTable(), cPOUDefinition);
		ParseVariableAccessArray(pouDefNode.FirstChild.NextSibling.NextSibling, cPOUDefinition.CreateVariableAccessArray());
	}

	private void ParseInitArray(XmlNode initValueNode, CInitValueTable initValueTable, CPOUDefinition pouDef)
	{
		string initValueInnerText = GetInitValueInnerText(initValueNode);
		CTypeArray getTypeArray = pouDef.GetTypeArray;
		CMemoryCell cMemoryCell = null;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		ushort num4 = 0;
		tMemoryLoc val2 = default(tMemoryLoc);
		while (num4 < getTypeArray.GetNrOfTypes())
		{
			tMemoryCellSimpleType simpleType = getTypeArray.GetMemCellType(num4).GetSimpleType();
			string text;
			if (simpleType == tMemoryCellSimpleType.StringMCType)
			{
				num2 = initValueInnerText.IndexOf("'", num);
				num3 = initValueInnerText.IndexOf("'", num2 + 1);
				text = initValueInnerText.Substring(num2 + 1, num3 - num2 - 1);
				num = num3 + 1;
				if (num2 <= 0 || num3 <= num2)
				{
					throw new ArgumentOutOfRangeException("String initval not OK");
				}
			}
			else
			{
				num3 = initValueInnerText.IndexOf(";", num);
				text = initValueInnerText.Substring(num, num3 - num);
				num = num3;
			}
			if (num3 <= 0)
			{
				throw new ArgumentOutOfRangeException("initval not OK");
			}
			if (initValueInnerText[num] == ';')
			{
				num++;
				text = text.Trim();
				switch (simpleType)
				{
				case tMemoryCellSimpleType.StringMCType:
					cMemoryCell = new CStringVal(text);
					break;
				case tMemoryCellSimpleType.BoolMCType:
				{
					bool val9 = true;
					if (text == "0")
					{
						val9 = false;
					}
					cMemoryCell = new CBoolVal(val9);
					break;
				}
				case tMemoryCellSimpleType.DIntMCType:
				{
					int val8 = XmlConvert.ToInt32(text);
					cMemoryCell = new CDIntVal(val8);
					break;
				}
				case tMemoryCellSimpleType.IntMCType:
				{
					int val7 = XmlConvert.ToInt32(text);
					cMemoryCell = new CIntVal(val7);
					break;
				}
				case tMemoryCellSimpleType.UIntMCType:
				{
					int val6 = XmlConvert.ToInt32(text);
					cMemoryCell = new CUIntVal(val6);
					break;
				}
				case tMemoryCellSimpleType.DWordMCType:
				{
					int val5 = XmlConvert.ToInt32(text);
					cMemoryCell = new CDWordVal(val5);
					break;
				}
				case tMemoryCellSimpleType.WordMCType:
				{
					int val4 = XmlConvert.ToInt32(text);
					cMemoryCell = new CWordVal(val4);
					break;
				}
				case tMemoryCellSimpleType.RealMCType:
				{
					double val3 = XmlConvert.ToDouble(text);
					cMemoryCell = new CRealVal(val3);
					break;
				}
				case tMemoryCellSimpleType.MemMCType:
				case tMemoryCellSimpleType.CVMemMCType:
					XmlConvert.ToInt32(text);
					val2.VarOffset = 0;
					val2.POUInstance = 0;
					cMemoryCell = new CMemVal(val2);
					break;
				case tMemoryCellSimpleType.ObjectMCType:
				case tMemoryCellSimpleType.ArrayMCType:
				case tMemoryCellSimpleType.QueueMCType:
				case tMemoryCellSimpleType.AnyMCType:
					cMemoryCell = new CObjVal();
					break;
				case tMemoryCellSimpleType.ExternMCType:
				{
					int val = XmlConvert.ToInt32(text);
					cMemoryCell = new CExternVal(val);
					break;
				}
				case tMemoryCellSimpleType.MemCellPtrMCType:
				case tMemoryCellSimpleType.CVMemCellPtrMCType:
					cMemoryCell = new CMemCellPtr(null);
					break;
				case tMemoryCellSimpleType.POUInstRefMCType:
					cMemoryCell = new CPOUInstRef(null);
					break;
				default:
					throw new ArgumentOutOfRangeException(getTypeArray.GetMemCellType(num4).GetSimpleType().ToString());
				}
				initValueTable.SetMemoryCell(num4, cMemoryCell);
				num4++;
				continue;
			}
			throw new ArgumentOutOfRangeException("initval parse error ;");
		}
	}

	private void ParseVariableAccessArray(XmlNode varaccessArrNode, CVariableAccessArray varaccessArr)
	{
		if (varaccessArrNode == null)
		{
			return;
		}
		string innerText = GetInnerText(varaccessArrNode);
		string[] array = innerText.Split(';');
		int num = array.Length - 1;
		for (int i = 0; i < num; i++)
		{
			string text = array[i].Trim();
			if (text.Length <= 0)
			{
				continue;
			}
			CVariableAccess variableAccess = varaccessArr.GetVariableAccess(i);
			if (variableAccess != null)
			{
				variableAccess.IsDefined = true;
				if (text.IndexOf('r') >= 0)
				{
					variableAccess.IsRead = true;
				}
				if (text.IndexOf('w') >= 0)
				{
					variableAccess.IsWritten = true;
				}
			}
		}
	}

	private void ParseTypeArray(XmlNode typeArrNode, CTypeArray typeArr)
	{
		string innerText = GetInnerText(typeArrNode);
		string[] array = innerText.Split(';');
		int num = array.Length - 1;
		if (num != typeArr.GetNrOfTypes())
		{
			throw new ArgumentOutOfRangeException("NrOfTypes");
		}
		for (int i = 0; i < num; i++)
		{
			string[] array2 = array[i].Split(' ');
			tMemoryCellType tMemoryCellType2 = new tMemoryCellType(StringTotMemoryCellSimpleType(array2[0]));
			typeArr.SetMemCellType((ushort)i, tMemoryCellType2);
			for (int j = 1; j < array2.Length; j++)
			{
				if (array2[j] == "cr")
				{
					tMemoryCellType2.ColdRetain = true;
					tMemoryCellType2.Retain = true;
				}
				else if (array2[j] == "r")
				{
					tMemoryCellType2.Retain = true;
				}
				else if (array2[j] == "c")
				{
					tMemoryCellType2.Constant = true;
				}
				else if (array2[j] == "ns")
				{
					tMemoryCellType2.NoSort = true;
				}
				else if (array2[j] == "ip")
				{
					tMemoryCellType2.InParameter = true;
				}
				else if (array2[j] == "op")
				{
					tMemoryCellType2.OutParameter = true;
				}
				else if (array2[j] == "ipc")
				{
					tMemoryCellType2.InPicture = true;
				}
			}
		}
	}

	private tMemoryCellSimpleType StringTotMemoryCellSimpleType(string simpleTypeStr)
	{
		tMemoryCellSimpleType tMemoryCellSimpleType2 = tMemoryCellSimpleType.UndefMCType;
		return simpleTypeStr switch
		{
			"B" => tMemoryCellSimpleType.BoolMCType, 
			"DI" => tMemoryCellSimpleType.DIntMCType, 
			"R" => tMemoryCellSimpleType.RealMCType, 
			"S" => tMemoryCellSimpleType.StringMCType, 
			"I" => tMemoryCellSimpleType.IntMCType, 
			"UI" => tMemoryCellSimpleType.UIntMCType, 
			"DW" => tMemoryCellSimpleType.DWordMCType, 
			"W" => tMemoryCellSimpleType.WordMCType, 
			"M" => tMemoryCellSimpleType.MemMCType, 
			"G" => tMemoryCellSimpleType.GroupRefMCType, 
			"O" => tMemoryCellSimpleType.ObjectMCType, 
			"A" => tMemoryCellSimpleType.ArrayMCType, 
			"Q" => tMemoryCellSimpleType.QueueMCType, 
			"D" => tMemoryCellSimpleType.DurationMCType, 
			"T" => tMemoryCellSimpleType.TimeMCType, 
			"E" => tMemoryCellSimpleType.ExternMCType, 
			"De" => tMemoryCellSimpleType.DefaultMCType, 
			"An" => tMemoryCellSimpleType.AnyMCType, 
			"MC" => tMemoryCellSimpleType.MemCellPtrMCType, 
			"P" => tMemoryCellSimpleType.POUInstRefMCType, 
			"CVMC" => tMemoryCellSimpleType.CVMemCellPtrMCType, 
			"CVM" => tMemoryCellSimpleType.CVMemMCType, 
			_ => throw new ArgumentOutOfRangeException("UndefMCType" + simpleTypeStr), 
		};
	}
}
