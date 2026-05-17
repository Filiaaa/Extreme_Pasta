"""Generate coca_cola fizz sprite sheet + animation. Run: py Sprites/Interior/_gen_cola_fizz.py"""
import os
import sys

ROOT = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, ROOT)

from _generate_props import (  # noqa: E402
    gen_cola,
    rect,
    sheet,
    sprite_list_from_sheet,
    write_anim,
    write_meta,
    T,
    WHITE,
)

FW, FH = 10, 20
BUBBLE = (255, 255, 255, 255)
FOAM = (255, 245, 235, 255)
FIZZ = (220, 240, 255, 200)


def add_fizz_frame(img, frame_idx):
    bubbles = [
        (4, 15),
        (5, 12),
        (3, 10),
        (6, 14),
        (4, 8),
        (5, 6),
    ]
    for i in range(min(frame_idx + 1, len(bubbles))):
        x, y = bubbles[i]
        rect(img, x, y, x, y, BUBBLE)
        if frame_idx > i:
            rect(img, x, y - 1, x, y - 1, FIZZ)

    if frame_idx >= 2:
        rect(img, 3, 0, 6, 0, FOAM)
        rect(img, 2, 1, 7, 1, FOAM)
    if frame_idx >= 3:
        rect(img, 2, 0, 7, 0, WHITE)
        rect(img, 1, 1, 8, 2, FOAM)
    if frame_idx >= 5:
        rect(img, 1, 0, 8, 2, WHITE)
        rect(img, 0, 2, 9, 3, FOAM)


def gen_cola_fizz_sheet():
    frames = []
    for i in range(6):
        frame = gen_cola()
        if i > 0:
            add_fizz_frame(frame, i)
        frames.append(frame)
    return sheet(frames)


def main():
    img = gen_cola_fizz_sheet()
    png_name = "coca_cola_fizz.png"
    img.save(os.path.join(ROOT, png_name))

    names = [f"coca_cola_fizz_{i}" for i in range(6)]
    sprites = sprite_list_from_sheet(FW, FH, 6, names)
    ids, tex_guid = write_meta(png_name, sprites, pivot=(0.5, 0.5))

    keys = [(i * 0.15, ids[names[i]]) for i in range(6)]
    anim_dir = os.path.join(ROOT, "..", "..", "Animations", "CocaCola")
    anim_path = os.path.join(anim_dir, "fizz.anim")
    anim_guid = write_anim(anim_path, "fizz", tex_guid, keys, loop=False)

    print(f"Texture guid: {tex_guid}")
    print(f"Anim guid: {anim_guid}")
    print(f"Saved {png_name} ({img.size[0]}x{img.size[1]})")


if __name__ == "__main__":
    main()
