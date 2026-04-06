// Copyright © 2026 Mike Brown; see LICENSE at the root of this package

using UnityEngine;
using UnityEngine.UI;

namespace BotTweaks;

internal sealed class RemoteWorker : TrackedRobot {
    internal static readonly Tag PrefabTag = new Tag("RemoteWorker");

    internal override RobotType RobotType => RobotType.RemoteWorker;

	// Pinned resources panel: hardcode to a known-working symbol
	private const string PinnedAnimName = "body_comp_default_kanim";
	private const string PinnedSymbolName = "headfx_radiation4";
	private static Sprite CachedPinnedSprite;

	// All resources screen: keep the portrait (looks correct there) and hardcode the animation.
	private const string PortraitPrefabTag = "MinionUIPortrait";
	private const string AllResourcesPortraitAnim = "ui_idle";

	internal static void ApplyIcon(GameObject row, Image iconImage) {
		if (iconImage == null)
			return;

		bool isPinned = row != null && row.GetComponentInParent<global::PinnedResourcesPanel>() != null;

		if (isPinned) {
			ApplyPinnedIcon(iconImage);
		} else {
			ApplyAllResourcesIcon(iconImage);
		}
	}

	private static void ApplyPinnedIcon(Image iconImage) {
		// Remove any previously injected portrait, if present.
		var iconGO = iconImage.gameObject;
		var existing = iconGO.transform.Find("BotTweaks_RemoteWorkerIconPortrait");
		if (existing != null)
			global::Util.KDestroyGameObject(existing.gameObject);

		var sprite = CachedPinnedSprite;
		if (sprite == null) {
			sprite = CreatePinnedSprite();
			CachedPinnedSprite = sprite;
		}

		if (sprite != null) {
			iconImage.enabled = true;
			iconImage.raycastTarget = false;
			iconImage.sprite = sprite;
			iconImage.color = Color.white;
			iconImage.preserveAspect = true;
		}
	}

	private static Sprite CreatePinnedSprite() {
		var anim = Assets.GetAnim(PinnedAnimName);
		if (anim == null)
			return null;
		return Def.GetSpriteFromKAnimFile(anim, null, null, null, animName: null, centered: true, symbolName: PinnedSymbolName);
	}

	private static void ApplyAllResourcesIcon(Image iconImage) {
		var iconGO = iconImage.gameObject;

		// Avoid duplicating on refresh.
		if (iconGO.transform.Find("BotTweaks_RemoteWorkerIconPortrait") != null)
			return;

		var prefab = Assets.GetPrefab(new Tag(PortraitPrefabTag));
		if (prefab == null)
			return;

		// Hide the base Image but keep it enabled (avoids layout issues in some UI setups).
		iconImage.enabled = true;
		iconImage.raycastTarget = false;
		var c = iconImage.color;
		c.a = 0f;
		iconImage.color = c;

		var portrait = global::Util.KInstantiateUI(prefab, iconGO, true);
		portrait.name = "BotTweaks_RemoteWorkerIconPortrait";
		portrait.transform.SetAsLastSibling();
		portrait.SetActive(true);

		var rect = portrait.GetComponent<RectTransform>();
		if (rect != null) {
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.anchoredPosition = Vector2.zero;
			rect.sizeDelta = Vector2.zero;
			rect.localScale = Vector3.one;
		}

		var kbac = portrait.GetComponent<KBatchedAnimController>();
		if (kbac != null) {
			kbac.Play(AllResourcesPortraitAnim, KAnim.PlayMode.Loop);
			kbac.enabled = true;
		}

		var accessorizer = portrait.GetComponent<Accessorizer>();
		if (accessorizer != null) {
			accessorizer.ApplyBodyData(RemoteWorkerConfig.CreateBodyData());
			accessorizer.ApplyAccessories();
		}
	}
}
