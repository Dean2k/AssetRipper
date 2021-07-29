﻿using AssetRipper.Project;
using AssetRipper.IO.Asset;
using AssetRipper.YAML;
using System;
using System.Collections.Generic;
using AssetRipper.IO;

namespace AssetRipper.Classes.Renderer
{
	public struct StaticBatchInfo : IAssetReadable, IYAMLExportable
	{
		public ushort FirstSubMesh { get; set; }
		public ushort SubMeshCount { get; set; }

		public const string FirstSubMeshName = "firstSubMesh";
		public const string SubMeshCountName = "subMeshCount";

		public StaticBatchInfo(IReadOnlyList<uint> subsetIndices)
		{
			if (subsetIndices.Count == 0)
			{
				FirstSubMesh = 0;
				SubMeshCount = 0;
			}
			else
			{
				FirstSubMesh = (ushort)subsetIndices[0];
				SubMeshCount = (ushort)subsetIndices.Count;
				for (int i = 0, j = FirstSubMesh; i < SubMeshCount; i++, j++)
				{
					if (subsetIndices[i] != j)
					{
						throw new Exception("Can't create static batch info from subset indices");
					}
				}
			}
		}

		public StaticBatchInfo(ObjectReader reader)
		{
			FirstSubMesh = reader.ReadUInt16();
			SubMeshCount = reader.ReadUInt16();
		}

		public void Read(AssetReader reader)
		{
			FirstSubMesh = reader.ReadUInt16();
			SubMeshCount = reader.ReadUInt16();
		}

		public YAMLNode ExportYAML(IExportContainer container)
		{
			YAMLMappingNode node = new YAMLMappingNode();
			node.Add(FirstSubMeshName, FirstSubMesh);
			node.Add(SubMeshCountName, SubMeshCount);
			return node;
		}
	}
}
