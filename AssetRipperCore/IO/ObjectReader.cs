﻿using AssetRipper.IO.Endian;
using AssetRipper.Reading;
using AssetRipper.SerializedFiles;
using System;

namespace AssetRipper.IO
{
	public class ObjectReader : AltEndianBinaryReader
	{
		public SerializedFile assetsFile;
		public long m_PathID;
		public long byteStart;
		public uint byteSize;
		public ClassIDType type;
		public SerializedType serializedType;
		public BuildTarget platform;
		public SerializedFileFormatVersion m_Version;

		public int[] version => assetsFile.version;
		public BuildType buildType => assetsFile.buildType;

		public ObjectReader(AltEndianBinaryReader reader, SerializedFile assetsFile, ObjectInfo objectInfo) : base(reader.BaseStream, reader.endian)
		{
			this.assetsFile = assetsFile;
			m_PathID = objectInfo.m_PathID;
			byteStart = objectInfo.byteStart;
			byteSize = objectInfo.byteSize;
			if (Enum.IsDefined(typeof(ClassIDType), objectInfo.classID))
			{
				type = (ClassIDType)objectInfo.classID;
			}
			else
			{
				type = ClassIDType.UnknownType;
			}
			serializedType = objectInfo.serializedType;
			platform = assetsFile.m_TargetPlatform;
			m_Version = assetsFile.header.m_Version;
		}

		public void Reset()
		{
			Position = byteStart;
		}
	}
}
