﻿using AssetRipper.Core.Parser.Files.SerializedFiles.Parser.TypeTree;
using AssetRipper.Core.Reading.Classes;
using System.Collections.Generic;

namespace AssetRipper.Core.Reading.Utils
{
	public static class MonoBehaviourConverter
	{
		public static TypeTree ConvertToTypeTree(this MonoBehaviour m_MonoBehaviour, AssemblyLoader assemblyLoader)
		{
			var m_Type = new TypeTree();
			m_Type.Nodes = new List<TypeTreeNode>();
			var helper = new SerializedTypeHelper(m_MonoBehaviour.version);
			helper.AddMonoBehaviour(m_Type.Nodes, 0);
			if (m_MonoBehaviour.m_Script.TryGet(out var m_Script))
			{
				var typeDef = assemblyLoader.GetTypeDefinition(m_Script.m_AssemblyName, string.IsNullOrEmpty(m_Script.m_Namespace) ? m_Script.m_ClassName : $"{m_Script.m_Namespace}.{m_Script.m_ClassName}");
				if (typeDef != null)
				{
					var typeDefinitionConverter = new TypeDefinitionConverter(typeDef, helper, 1);
					m_Type.Nodes.AddRange(typeDefinitionConverter.ConvertToTypeTreeNodes());
				}
			}
			return m_Type;
		}
	}
}
