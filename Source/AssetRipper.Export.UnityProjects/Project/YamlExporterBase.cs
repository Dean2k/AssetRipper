using AssetRipper.Assets;
using AssetRipper.Assets.Export;
using AssetRipper.Assets.Metadata;
using AssetRipper.Export.Modules.Shaders.IO;
using AssetRipper.IO.Files;
using AssetRipper.Yaml;
using System.Text;

namespace AssetRipper.Export.UnityProjects.Project
{
	public abstract class YamlExporterBase : IAssetExporter
	{
		public abstract bool TryCreateCollection(IUnityObjectBase asset, [NotNullWhen(true)] out IExportCollection? exportCollection);

		public bool Export(IExportContainer container, IUnityObjectBase asset, string path)
		{
			using Stream fileStream = File.Create(path);
			using InvariantStreamWriter streamWriter = new InvariantStreamWriter(fileStream, UTF8);
			YamlWriter writer = new();
			YamlDocument doc = asset.ExportYamlDocument(container);
			writer.AddDocument(doc);
			writer.Write(streamWriter);
			return true;
		}

		public void Export(IExportContainer container, IUnityObjectBase asset, string path, Action<IExportContainer, IUnityObjectBase, string>? callback)
		{
			Export(container, asset, path);
			callback?.Invoke(container, asset, path);
		}

		public bool Export(IExportContainer container, IEnumerable<IUnityObjectBase> assets, string path)
		{
			using Stream fileStream = File.Create(path);
			using InvariantStreamWriter streamWriter = new InvariantStreamWriter(fileStream, UTF8);
			YamlWriter writer = new();
			writer.WriteHead(streamWriter);
			foreach (IUnityObjectBase asset in assets)
			{
				YamlDocument doc = asset.ExportYamlDocument(container);
				writer.WriteDocument(doc);
			}
			writer.WriteTail(streamWriter);
			return true;
		}

		public void Export(IExportContainer container, IEnumerable<IUnityObjectBase> assets, string path, Action<IExportContainer, IUnityObjectBase, string>? callback)
		{
			throw new NotSupportedException("Yaml supports only single file export");
		}

		public AssetType ToExportType(IUnityObjectBase asset)
		{
			return AssetType.Serialized;
		}

		public bool ToUnknownExportType(Type type, out AssetType assetType)
		{
			assetType = AssetType.Serialized;
			return true;
		}

		private static readonly Encoding UTF8 = new UTF8Encoding(false);

		private sealed class YamlWalker(IExportContainer container, TextWriter innerWriter) : DefaultYamlWalker(innerWriter)
		{
			public IUnityObjectBase CurrentAsset { get; set; } = null!;

			public override void WritePPtr<TAsset>(PPtr<TAsset> pptr)
			{
				TAsset? asset = CurrentAsset.Collection.TryGetAsset(pptr);
				if (asset is null)
				{
					Writer.Write("{fileID: 0}");
				}
				else
				{
					MetaPtr metaPtr = container.CreateExportPointer(asset);
					Writer.Write(metaPtr.ToString());
				}
			}
		}
	}
}
