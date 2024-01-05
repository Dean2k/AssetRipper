﻿using AssetRipper.Assets;
using AssetRipper.Assets.Traversal;
using AssetRipper.Processing;

namespace AssetRipper.Tests.Traversal;

internal sealed class VectorXY : UnityAssetBase
{
	private readonly ulong xy;

	public override int SerializedVersion => 2;

	public override void WalkStandard(AssetWalker walker)
	{
		if (walker.EnterAsset(this))
		{
			this.WalkPrimitiveField(walker, xy);
			walker.ExitAsset(this);
		}
	}
}
