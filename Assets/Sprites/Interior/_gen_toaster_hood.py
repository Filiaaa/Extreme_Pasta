"""Toaster, hood, can, yeast dough. Run: py Sprites/Interior/_gen_toaster_hood.py"""
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
    YELLOW,
    YELLOW_HI,
    rect,
    paste,
    sheet,
    write_meta,
    write_anim,
    sprite_list_from_sheet,
)

TOAST = (210, 175, 120, 255)
TOAST_D = (160, 130, 80, 255)
TOAST_HI = (240, 210, 160, 255)
HANDLE = (90, 90, 95, 255)
HANDLE_D = (60, 60, 65, 255)
METAL = (175, 180, 190, 255)
METAL_D = (120, 125, 135, 255)
HOOD = (185, 190, 200, 255)
HOOD_D = (140, 145, 155, 255)
HOOD_SH = (100, 105, 115, 255)
CAN = (190, 50, 45, 255)
CAN_D = (120, 30, 28, 255)
CAN_HI = (230, 90, 70, 255)
PACK = (255, 240, 200, 255)
PACK_D = (220, 180, 120, 255)
PACK_RED = (200, 60, 50, 255)
DOUGH = (255, 235, 190, 255)
DOUGH_D = (230, 200, 150, 255)
POP = (255, 248, 220, 255)


def new(w, h):
    from PIL import Image

    return Image.new("RGBA", (w, h), T)


def gen_toaster():
    img = new(44, 28)
    rect(img, 2, 6, 30, 25, TOAST_D)
    rect(img, 3, 7, 29, 24, TOAST)
    rect(img, 4, 8, 28, 10, TOAST_HI)
    rect(img, 5, 18, 27, 19, EDGE)
    rect(img, 8, 20, 24, 21, METAL_D)
    # side handle (lever)
    rect(img, 30, 12, 41, 14, HANDLE_D)
    rect(img, 31, 11, 40, 15, HANDLE)
    rect(img, 32, 13, 33, 13, METAL)
    return img


def gen_toaster_pop_frames():
    frames = []
    base = gen_toaster()
    for i in range(4):
        f = base.copy()
        if i >= 1:
            rect(f, 6, 4 - i, 26, 6, POP)
            rect(f, 10, 2, 22, 5 - i, YELLOW_HI)
        if i >= 2:
            rect(f, 12, 0, 20, 2, WHITE)
        frames.append(f)
    return frames


def gen_hood():
    """Side view: bottom intake, vertical riser, 90 elbow, top horizontal walk duct."""
    w, h = 64, 96
    img = new(w, h)
    inner = (42, 46, 54, 255)
    inner_hi = (62, 68, 82, 255)
    lip = (95, 100, 110, 255)

    # --- top horizontal pipe (walk here) ---
    rect(img, 2, 2, 61, 20, HOOD)
    rect(img, 2, 2, 61, 5, HOOD_D)
    rect(img, 6, 8, 57, 16, inner)
    rect(img, 6, 12, 57, 13, inner_hi)
    rect(img, 58, 6, 61, 17, HOOD_SH)
    rect(img, 2, 6, 5, 17, HOOD_SH)

    # --- 90 elbow (top-right corner of L) ---
    rect(img, 38, 14, 61, 22, HOOD)
    rect(img, 40, 16, 58, 20, inner)
    rect(img, 54, 18, 60, 21, HOOD_D)

    # --- vertical riser (connects elbow to bottom intake) ---
    rect(img, 38, 20, 61, 78, HOOD)
    rect(img, 40, 22, 58, 76, inner)
    rect(img, 56, 24, 60, 74, HOOD_SH)
    rect(img, 38, 30, 40, 70, HOOD_D)

    # suction hint inside shaft
    for y in range(28, 70, 8):
        rect(img, 46, y, 50, y + 2, inner_hi)

    # --- bottom intake (wide opening, suck from below) ---
    rect(img, 22, 78, 61, 94, HOOD)
    rect(img, 22, 78, 61, 80, HOOD_D)
    rect(img, 26, 82, 57, 92, inner)
    rect(img, 28, 86, 55, 90, (30, 32, 38, 255))
    # flared rim
    rect(img, 18, 92, 61, 95, lip)
    rect(img, 20, 94, 59, 95, HOOD_SH)
    # outer bracket left
    rect(img, 18, 76, 24, 94, HOOD)
    rect(img, 18, 76, 21, 90, HOOD_SH)

    return img


def gen_can():
    img = new(12, 16)
    rect(img, 2, 2, 9, 13, CAN)
    rect(img, 2, 2, 2, 13, CAN_D)
    rect(img, 9, 2, 9, 13, CAN_D)
    rect(img, 1, 0, 10, 2, CAN_HI)
    rect(img, 3, 4, 8, 6, CAN_HI)
    return img


def gen_yeast_packet():
    img = new(22, 26)
    rect(img, 2, 4, 19, 24, PACK)
    rect(img, 2, 4, 19, 6, PACK_RED)
    rect(img, 4, 8, 17, 20, PACK_D)
    rect(img, 6, 10, 15, 12, WHITE)
    return img


def gen_yeast_torn():
    img = new(22, 26)
    rect(img, 2, 8, 19, 24, PACK)
    rect(img, 2, 4, 10, 10, PACK)
    rect(img, 12, 2, 19, 8, PACK_D)
    rect(img, 3, 12, 8, 18, DOUGH)
    return img


def gen_yeast_chain_icon():
    """16x16 UI icon for puzzle chain (yeast packet)."""
    img = new(16, 16)
    rect(img, 2, 3, 13, 15, PACK)
    rect(img, 2, 3, 13, 4, PACK_RED)
    rect(img, 3, 5, 12, 14, PACK_D)
    rect(img, 4, 6, 11, 7, WHITE)
    rect(img, 5, 9, 10, 10, WHITE)
    rect(img, 4, 12, 5, 13, (255, 220, 160, 255))
    rect(img, 10, 11, 11, 12, (255, 220, 160, 255))
    return img


def _put_ellipse(img, cx, cy, rx, ry, color):
    w, h = img.size
    for y in range(max(0, cy - ry), min(h, cy + ry + 1)):
        for x in range(max(0, cx - rx), min(w, cx + rx + 1)):
            dx = (x - cx) / max(rx, 1)
            dy = (y - cy) / max(ry, 1)
            if dx * dx + dy * dy <= 1.0:
                img.putpixel((x, y), color)


def _draw_dough_blob(img, cx, base_y, rx, ry):
    cy = base_y - ry
    _put_ellipse(img, cx, cy, rx, ry, DOUGH)
    _put_ellipse(img, cx - 2, cy - 1, max(2, rx - 3), max(2, ry - 2), DOUGH_D)
    _put_ellipse(img, cx + 1, cy - ry // 2, max(2, rx - 4), max(2, ry // 3), WHITE)
    for bx, by in ((cx - rx + 2, base_y - 2), (cx + 2, base_y - 3), (cx, base_y - 4)):
        if 0 <= bx < img.size[0] and 0 <= by < img.size[1]:
            img.putpixel((bx, by), WHITE)


def gen_dough_rise_frame(rise_h, wobble=0, steam=False, squeeze=False):
    fw, fh = 32, 80
    img = new(fw, fh)
    rect(img, 1, fh - 2, fw - 2, fh - 1, PACK_D)
    rect(img, 3, fh - 3, fw - 4, fh - 2, (200, 180, 140, 255))

    rx = 7 + rise_h // 5 + (2 if squeeze else 0)
    ry = max(4, rise_h // 2 + 2)
    cx = 16 + wobble
    base_y = fh - 3
    _draw_dough_blob(img, cx, base_y, rx, ry)

    if rise_h > 10:
        for i, (bx, by) in enumerate(
            ((cx - 3, base_y - rise_h // 2), (cx + 2, base_y - rise_h // 3), (cx, base_y - rise_h + 4))
        ):
            if i < rise_h // 12:
                rect(img, bx, by, bx, by, WHITE)

    if steam:
        for sx, sy, sh in ((cx - 5, fh - rise_h - 8, 4), (cx + 3, fh - rise_h - 10, 5), (cx, fh - rise_h - 6, 3)):
            for dy in range(sh):
                rect(img, sx + (dy % 2), sy + dy, sx + (dy % 2), sy + dy, (255, 255, 255, 140))
    return img


def gen_dough_rise_sheet():
    specs = [
        (6, 0, False, False),
        (12, 1, False, True),
        (20, -1, False, False),
        (28, 0, False, True),
        (36, 1, False, False),
        (44, 0, True, False),
        (52, -1, True, False),
        (58, 0, True, False),
        (64, 1, True, False),
        (70, 0, True, False),
    ]
    return sheet([gen_dough_rise_frame(h, w, st, sq) for h, w, st, sq in specs])


def main():
    assets = []
    singles = [
        ("toaster.png", gen_toaster(), [("toaster", 0, 0, 44, 28)], (0.5, 0.0)),
        ("hood.png", gen_hood(), [("hood", 0, 0, 64, 96)], (0.5, 0.0)),
        ("can.png", gen_can(), [("can", 0, 0, 12, 16)], (0.5, 0.0)),
        ("yeast_packet.png", gen_yeast_packet(), [("yeast_packet", 0, 0, 22, 26)], (0.5, 0.0)),
        ("yeast_packet_torn.png", gen_yeast_torn(), [("yeast_packet_torn", 0, 0, 22, 26)], (0.5, 0.0)),
    ]
    for p, img, sp, pivot in singles:
        img.save(os.path.join(ROOT, p))
        write_meta(p, sp, pivot=pivot)
        assets.append(p)

    pop = sheet(gen_toaster_pop_frames())
    pop_path = "toaster_pop.png"
    pop.save(os.path.join(ROOT, pop_path))
    names = [f"toaster_pop_{i}" for i in range(4)]
    sp = sprite_list_from_sheet(44, 28, 4, names)
    ids, g = write_meta(pop_path, sp)
    keys = [(i * 0.08, ids[names[i]]) for i in range(4)]
    anim_dir = os.path.join(ROOT, "..", "..", "Animations", "Toaster")
    os.makedirs(anim_dir, exist_ok=True)
    write_anim(os.path.join(anim_dir, "pop.anim"), "pop", g, keys, loop=False)
    assets.append(pop_path)

    icon = gen_yeast_chain_icon()
    icon_path = "yeast_chain_icon.png"
    icon.save(os.path.join(ROOT, icon_path))
    write_meta(icon_path, [("yeast_chain_icon", 0, 0, 16, 16)], pivot=(0.5, 0.5))
    assets.append(icon_path)

    dr = gen_dough_rise_sheet()
    dr_path = "dough_rise.png"
    dr.save(os.path.join(ROOT, dr_path))
    frame_count = 10
    dn = [f"dough_rise_{i}" for i in range(frame_count)]
    sp2 = sprite_list_from_sheet(32, 80, frame_count, dn)
    ids2, g2 = write_meta(
        dr_path,
        sp2,
        guid="80ccb0560ca7408f8017a4c9a4768f93",
        pivot=(0.5, 0.0),
        tex_h=80,
    )
    times = [0.0, 0.15, 0.3, 0.45, 0.58, 0.7, 0.82, 0.94, 1.06, 1.2]
    keys2 = [(times[i], ids2[dn[i]]) for i in range(frame_count)]
    anim_dir2 = os.path.join(ROOT, "..", "..", "Animations", "Dough")
    os.makedirs(anim_dir2, exist_ok=True)
    write_anim(os.path.join(anim_dir2, "rise.anim"), "rise", g2, keys2, loop=False)
    assets.append(dr_path)

    print("Generated:", ", ".join(assets))


if __name__ == "__main__":
    main()
