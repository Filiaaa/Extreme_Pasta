"""Fridge, sink, frozen sink, ice inventory icon. Run: py Sprites/Interior/_gen_sink_fridge.py"""
import os
import sys

ROOT = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, ROOT)

from _generate_props import (  # noqa: E402
    T,
    BODY,
    BODY_HI,
    BODY_SH,
    EDGE,
    EDGE_LT,
    WHITE,
    rect,
    paste,
    write_meta,
)

# Fridge palette
FRIDGE = (232, 238, 245, 255)
FRIDGE_SH = (190, 198, 210, 255)
FRIDGE_DK = (160, 170, 185, 255)
FREEZER_IN = (180, 220, 255, 255)
ICE = (220, 245, 255, 255)
ICE_HI = (255, 255, 255, 255)
ICE_SH = (140, 200, 235, 255)
HANDLE = (120, 125, 135, 255)
SINK_METAL = (170, 175, 185, 255)
SINK_METAL_D = (120, 125, 135, 255)
WATER = (60, 140, 220, 255)
WATER_HI = (120, 200, 255, 255)
WATER_SH = (40, 90, 160, 255)
FROZEN = (200, 235, 255, 255)
FROZEN_SH = (150, 200, 230, 255)


def new(w, h):
    from PIL import Image

    return Image.new("RGBA", (w, h), T)


def gen_fridge():
    w, h = 64, 96
    img = new(w, h)
    # main body
    rect(img, 4, 28, 59, 93, FRIDGE)
    rect(img, 4, 28, 59, 30, FRIDGE_SH)
    rect(img, 57, 28, 59, 93, FRIDGE_DK)
    # freezer cavity open at top
    rect(img, 6, 4, 57, 26, FREEZER_IN)
    rect(img, 6, 4, 57, 5, EDGE_LT)
    rect(img, 6, 4, 7, 26, EDGE)
    rect(img, 56, 4, 57, 26, EDGE)
    # ice pile in freezer
    for y in range(8, 24):
        for x in range(10, 54):
            if (x + y) % 5 < 3:
                c = ICE_HI if (x + y) % 7 == 0 else ICE
                rect(img, x, y, x, y, c)
    rect(img, 12, 18, 48, 23, ICE_SH)
    rect(img, 20, 10, 42, 16, ICE_HI)
    # door line
    rect(img, 6, 48, 57, 49, FRIDGE_SH)
    rect(img, 52, 50, 54, 88, HANDLE)
    # feet
    rect(img, 10, 92, 14, 94, FRIDGE_DK)
    rect(img, 50, 92, 54, 94, FRIDGE_DK)
    return img


def gen_sink_water():
    w, h = 96, 56
    img = new(w, h)
    # basin outer
    rect(img, 2, 8, 93, 52, SINK_METAL)
    rect(img, 2, 8, 93, 10, SINK_METAL_D)
    rect(img, 4, 12, 91, 50, WATER_SH)
    rect(img, 6, 14, 89, 48, WATER)
    # water highlights
    for x in range(12, 84, 6):
        rect(img, x, 20, x + 2, 22, WATER_HI)
    rect(img, 20, 28, 70, 40, WATER_HI)
    rect(img, 30, 16, 38, 18, WATER_HI)
    # tap
    rect(img, 44, 0, 52, 6, SINK_METAL)
    rect(img, 47, 6, 49, 14, SINK_METAL_D)
    return img


def gen_sink_frozen():
    w, h = 96, 56
    img = new(w, h)
    rect(img, 2, 8, 93, 52, SINK_METAL)
    rect(img, 2, 8, 93, 10, SINK_METAL_D)
    rect(img, 6, 14, 89, 48, FROZEN_SH)
    for y in range(16, 47):
        for x in range(8, 88):
            if (x * 3 + y * 2) % 7 < 4:
                c = ICE_HI if (x + y) % 9 == 0 else FROZEN
                rect(img, x, y, x, y, c)
    rect(img, 12, 20, 80, 42, ICE)
    rect(img, 25, 24, 65, 36, ICE_HI)
    rect(img, 44, 0, 52, 6, SINK_METAL)
    rect(img, 47, 6, 49, 14, SINK_METAL_D)
    return img


def gen_sink_partial(level):
    """level 1..2 intermediate freeze."""
    from PIL import Image

    base = gen_sink_water()
    overlay = new(96, 56)
    strength = level * 0.35
    for y in range(14, 48):
        for x in range(8, 88):
            if (x * 2 + y) % (8 - level) == 0:
                rect(overlay, x, y, x, y, ICE if level == 1 else ICE_HI)
    return Image.alpha_composite(base, overlay)


def gen_ice_icon():
    img = new(16, 16)
    for y in range(3, 13):
        for x in range(3, 13):
            if (x + y) % 3 != 0:
                c = ICE_HI if (x + y) % 5 == 0 else ICE
                rect(img, x, y, x, y, c)
    rect(img, 5, 5, 10, 10, ICE_SH)
    rect(img, 7, 7, 9, 9, ICE_HI)
    return img


def extend_inventory_sheet():
    """Append ice icon at x=96. Does not modify .meta (only inventory slot sprites 0-6)."""
    from PIL import Image

    inv_path = os.path.join(ROOT, "..", "inventorySlotSprites.png")
    inv_path = os.path.normpath(inv_path)
    ice = gen_ice_icon()
    target_w = 112
    if os.path.exists(inv_path):
        sheet = Image.open(inv_path).convert("RGBA")
        w0, h0 = sheet.size
        if w0 < target_w:
            extended = new(target_w, h0)
            paste(extended, sheet, 0, 0)
            paste(extended, ice, 96, 0)
            extended.save(inv_path)
        else:
            paste(sheet, ice, 96, 0)
            sheet.save(inv_path)
    else:
        sheet = new(target_w, 16)
        paste(sheet, ice, 96, 0)
        sheet.save(inv_path)

    print(f"Extended inventory PNG: {inv_path} (update .meta in Unity if needed)")


def main():
    assets = []
    singles = [
        ("fridge.png", gen_fridge(), [("fridge", 0, 0, 64, 96)], (0.5, 0.0)),
        ("sink_water.png", gen_sink_water(), [("sink_water", 0, 0, 96, 56)], (0.5, 0.0)),
        ("sink_frozen.png", gen_sink_frozen(), [("sink_frozen", 0, 0, 96, 56)], (0.5, 0.0)),
        (
            "sink_partial_1.png",
            gen_sink_partial(1),
            [("sink_partial_1", 0, 0, 96, 56)],
            (0.5, 0.0),
        ),
        (
            "sink_partial_2.png",
            gen_sink_partial(2),
            [("sink_partial_2", 0, 0, 96, 56)],
            (0.5, 0.0),
        ),
        ("ice_icon.png", gen_ice_icon(), [("ice_icon", 0, 0, 16, 16)], (0.5, 0.5)),
    ]
    for p, img, sp, pivot in singles:
        path = os.path.join(ROOT, p)
        img.save(path)
        write_meta(p, sp, pivot=pivot)
        assets.append(p)

    extend_inventory_sheet()
    print("Generated:", ", ".join(assets))


if __name__ == "__main__":
    main()
