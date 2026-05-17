"""Generate kitchen experiment sprites. Run: py Sprites/Interior/_generate_props.py"""
from PIL import Image
import os
import uuid
import struct

ROOT = os.path.dirname(os.path.abspath(__file__))
OUT = ROOT

# --- palette ---
T = (0, 0, 0, 0)
BODY = (226, 226, 226, 255)
BODY_HI = (246, 246, 246, 255)
BODY_SH = (200, 200, 200, 255)
EDGE = (79, 52, 19, 255)
EDGE_LT = (125, 83, 34, 255)
WIN = (58, 35, 42, 255)
WIN_HI = (92, 64, 56, 255)
BTN = (72, 171, 162, 255)
LED = (255, 80, 80, 255)
DISP_BG = (40, 120, 60, 255)
DISP_ON = (120, 255, 140, 255)
YELLOW = (255, 213, 79, 255)
YELLOW_D = (242, 202, 75, 255)
YELLOW_HI = (255, 240, 180, 255)
WHITE = (255, 255, 255, 255)
COLA = (190, 25, 35, 255)
COLA_D = (120, 10, 15, 255)
LABEL_W = (255, 255, 255, 255)
MENTOS_W = (245, 245, 250, 255)
MENTOS_B = (60, 130, 210, 255)
MENTOS_R = (220, 50, 50, 255)
SPRING = (190, 195, 205, 255)
SPRING_D = (140, 145, 155, 255)
SPRING_HI = (230, 235, 245, 255)
STRING_C = (210, 190, 150, 255)
POP = (255, 248, 220, 255)
POP_D = (255, 220, 120, 255)
POP_SH = (230, 180, 80, 255)
BAG = (220, 60, 50, 255)
BAG_ST = (255, 230, 100, 255)
FORK_C = (210, 215, 225, 255)
FORK_SH = (160, 165, 175, 255)
WOOD = (125, 83, 34, 255)
WOOD_D = (79, 52, 19, 255)
JAR = (140, 200, 220, 255)
JAR_LID = (180, 140, 90, 255)
HONEY = (255, 190, 60, 255)


def new(w, h):
    return Image.new("RGBA", (w, h), T)


def rect(img, x0, y0, x1, y1, c):
    w, h = img.size
    for y in range(y0, y1 + 1):
        for x in range(x0, x1 + 1):
            if 0 <= x < w and 0 <= y < h:
                img.putpixel((x, y), c)


def paste(dst, src, x, y):
    dst.paste(src, (x, y), src)


def sheet(frames, pad=0):
    fw, fh = frames[0].size
    w = len(frames) * fw + pad * (len(frames) - 1)
    s = new(w, fh)
    x = 0
    for f in frames:
        paste(s, f, x, 0)
        x += fw + pad
    return s


# 3x5 pixel digits for microwave display
_DIG = {
    "0": ["###", "# #", "# #", "# #", "###"],
    "1": [" # ", "## ", " # ", " # ", "###"],
    "2": ["###", "  #", "###", "#  ", "###"],
    "3": ["###", "  #", "###", "  #", "###"],
    "4": ["# #", "# #", "###", "  #", "  #"],
    "5": ["###", "#  ", "###", "  #", "###"],
    "6": ["###", "#  ", "###", "# #", "###"],
    "7": ["###", "  #", "  #", "  #", "  #"],
    "8": ["###", "# #", "###", "# #", "###"],
    "9": ["###", "# #", "###", "  #", "###"],
}


def draw_digit(img, digit, ox, oy, on=DISP_ON, off=T):
    rows = _DIG[str(digit)]
    for ry, row in enumerate(rows):
        for rx, ch in enumerate(row):
            if ch == "#":
                rect(img, ox + rx, oy + ry, ox + rx, oy + ry, on)


def draw_microwave(timer_digit=None, ding=False):
    img = new(32, 24)
    rect(img, 2, 3, 29, 21, BODY)
    rect(img, 2, 3, 29, 4, BODY_HI)
    rect(img, 2, 20, 29, 21, EDGE_LT)
    rect(img, 2, 21, 29, 21, EDGE)
    rect(img, 2, 3, 3, 21, EDGE)
    rect(img, 29, 3, 29, 21, EDGE)
    rect(img, 4, 5, 21, 18, WIN)
    rect(img, 5, 6, 20, 7, WIN_HI)
    if ding:
        rect(img, 8, 9, 11, 12, YELLOW_HI)
        rect(img, 14, 10, 17, 13, POP)
    rect(img, 22, 5, 27, 18, BODY_SH)
    rect(img, 22, 5, 27, 10, DISP_BG)
    if timer_digit is not None:
        if timer_digit >= 10:
            draw_digit(img, 1, 22, 6)
            draw_digit(img, 0, 26, 6)
        else:
            draw_digit(img, timer_digit, 23, 6)
    elif ding:
        draw_digit(img, 0, 23, 6)
    else:
        rect(img, 23, 7, 26, 8, DISP_ON)
    rect(img, 23, 12, 26, 13, BTN)
    rect(img, 23, 15, 24, 16, LED if ding else LED)
    rect(img, 5, 22, 7, 22, EDGE)
    rect(img, 25, 22, 27, 22, EDGE)
    return img


def gen_microwave_timer():
    frames = []
    for d in range(10, -1, -1):
        frames.append(draw_microwave(d if d > 0 else 0, ding=(d == 0)))
    return sheet(frames)


def gen_mentos():
    img = new(8, 12)
    rect(img, 1, 2, 6, 11, MENTOS_W)
    rect(img, 1, 2, 6, 3, MENTOS_B)
    rect(img, 2, 0, 5, 2, MENTOS_R)
    rect(img, 1, 2, 1, 11, EDGE)
    rect(img, 6, 2, 6, 11, EDGE)
    return img


def gen_cola():
    img = new(10, 20)
    rect(img, 3, 0, 6, 2, COLA_D)
    rect(img, 2, 2, 7, 16, COLA)
    rect(img, 3, 8, 6, 10, LABEL_W)
    rect(img, 2, 2, 2, 16, COLA_D)
    rect(img, 7, 2, 7, 16, COLA_D)
    rect(img, 1, 17, 8, 18, COLA_D)
    rect(img, 2, 18, 7, 19, COLA_D)
    return img


def draw_spring_coil(img, x0, y0, w, h, coils=4):
    cy = y0
    for i in range(coils):
        c = SPRING_HI if i % 2 == 0 else SPRING
        rect(img, x0, cy, x0 + w - 1, cy + 1, c)
        rect(img, x0 + 1, cy + 2, x0 + w - 2, cy + 2, SPRING_D)
        cy += 3
    return cy


def gen_spring_hanging():
    frames = []
    offsets = [0, 1, 0, -1]
    for off in offsets:
        img = new(16, 30)
        rect(img, 7, 0, 8, 5 + off, STRING_C)
        sy = 6 + off
        draw_spring_coil(img, 4, sy, 8, 12, coils=5)
        rect(img, 3, sy + 16, 12, sy + 17, SPRING_D)
        frames.append(img)
    return sheet(frames)


def gen_spring_launch():
    specs = [
        (16, 10, 4, 2),
        (16, 8, 4, 1),
        (16, 14, 4, 3),
        (16, 22, 4, 5),
        (16, 18, 4, 4),
        (16, 10, 4, 2),
    ]
    frames = []
    for w, h, sw, coils in specs:
        img = new(16, 32)
        base_y = 32 - h - 2
        rect(img, 2, base_y + h, 13, base_y + h + 1, EDGE_LT)
        draw_spring_coil(img, (16 - sw) // 2, base_y, sw, h, coils=coils)
        if h > 14:
            rect(img, 6, base_y - 4, 9, base_y - 1, POP)
        frames.append(img)
    return sheet(frames)


def gen_popcorn_mountain():
    img = new(40, 36)
    # fluffy mound bigger than cheese 32x28
    pts = [
        (4, 32), (8, 28), (12, 24), (16, 20), (20, 16), (24, 14),
        (28, 12), (32, 14), (36, 18), (38, 24), (36, 30), (30, 34),
        (20, 35), (10, 34), (4, 32),
    ]
    for i in range(len(pts) - 1):
        x0, y0 = pts[i]
        x1, y1 = pts[i + 1]
        for x in range(min(x0, x1), max(x0, x1) + 1):
            t = 0 if x1 == x0 else (x - x0) / (x1 - x0)
            y = int(y0 + (y1 - y0) * t)
            for yy in range(y, 36):
                c = POP if (x + yy) % 3 else POP_D
                if 0 <= x < 40 and 0 <= yy < 36:
                    img.putpixel((x, yy), c)
    for x in range(8, 34):
        for y in range(18, 34):
            if img.getpixel((x, y))[3] == 0:
                continue
            if (x * 7 + y * 3) % 5 == 0:
                img.putpixel((x, y), POP if img.getpixel((x, y)) == POP_D else YELLOW_HI)
    rect(img, 0, 33, 39, 35, EDGE)
    return img


def gen_popcorn_bag():
    img = new(14, 18)
    rect(img, 2, 0, 11, 2, BAG_ST)
    rect(img, 1, 2, 12, 16, BAG)
    rect(img, 3, 4, 10, 12, BAG_ST)
    rect(img, 2, 8, 3, 10, WHITE)
    rect(img, 1, 2, 1, 16, EDGE)
    rect(img, 12, 2, 12, 16, EDGE)
    rect(img, 4, 1, 9, 3, POP)
    return img


def draw_fork(angle_idx):
    """angle_idx 0..7 pendulum / bounce"""
    img = new(20, 28)
    tilts = [0, -2, -4, -6, 5, 3, 1, 0]
    lift = [0, 0, 1, 3, 4, 2, 1, 0]
    tx = tilts[angle_idx]
    ty = lift[angle_idx]
    px, py = 9 + tx, 26
    # handle
    rect(img, 8 + tx, py - 8, 11 + tx, py, FORK_SH)
    rect(img, 9 + tx, py - 14, 10 + tx, py - 8, FORK_C)
  # tines
    for i, dx in enumerate([-3, -1, 1, 3]):
        rect(img, 9 + tx + dx, 2 + ty, 10 + tx + dx, py - 14, FORK_C)
    rect(img, 6 + tx, 2 + ty, 13 + tx, 4 + ty, FORK_C)
    if angle_idx in (3, 4):
        rect(img, 7 + tx, 0 + ty, 12 + tx, 2 + ty, WHITE)
    return img


def gen_fork():
    return sheet([draw_fork(i) for i in range(8)])


def gen_shelf_plank(w=28, h=8):
    img = new(w, h)
    rect(img, 0, 2, w - 1, h - 1, WOOD)
    rect(img, 0, 0, w - 1, 2, WOOD_D)
    rect(img, 0, h - 1, w - 1, h - 1, EDGE)
    return img


def gen_shelf_with_items():
    plank = gen_shelf_plank(28, 8)
    img = new(28, 18)
    paste(img, plank, 0, 10)
    rect(img, 3, 2, 6, 9, JAR)
    rect(img, 3, 1, 6, 2, JAR_LID)
    rect(img, 10, 3, 13, 9, JAR)
    rect(img, 10, 2, 13, 3, JAR_LID)
    rect(img, 18, 4, 21, 9, HONEY)
    rect(img, 18, 3, 21, 4, JAR_LID)
    return img


def gen_shelf_sheet():
    img = new(96, 18)
    paste(img, gen_shelf_plank(28, 8), 0, 10)
    paste(img, gen_shelf_plank(40, 8), 28, 10)
    paste(img, gen_shelf_with_items(), 68, 0)
    return img


def gen_shelf_props():
    frames = []
    for label, body, lid in [
        ("jar_blue", JAR, JAR_LID),
        ("jar_honey", HONEY, JAR_LID),
        ("can", BODY_SH, EDGE),
    ]:
        if label == "can":
            im = new(6, 10)
            rect(im, 1, 1, 4, 8, COLA)
            rect(im, 1, 0, 4, 1, BODY_HI)
        else:
            im = new(8, 12)
            rect(im, 2, 2, 5, 10, body)
            rect(im, 2, 1, 5, 2, lid)
        frames.append(im)
    return sheet(frames, pad=2)


def stable_id(name):
    h = struct.unpack(">i", uuid.uuid5(uuid.NAMESPACE_DNS, name).bytes[:4])[0]
    if h == 0:
        h = 1
    return h


def new_guid():
    return uuid.uuid4().hex  # Unity requires exactly 32 lowercase hex chars


def write_meta(png_name, sprites, guid=None, ppu=32, pivot=(0.5, 0.5), tex_h=None):
    if guid is None:
        guid = new_guid()
    if len(guid) != 32:
        raise ValueError(f"Invalid Unity GUID length for {png_name}: {len(guid)}")
    """sprites: list of (name, x, y, w, h) with y = PIL top-left; converted for Unity."""
    if tex_h is None:
        tex_h = max(s[2] + s[4] for s in sprites)
    path = os.path.join(OUT, png_name + ".meta")
    lines = []
    lines.append("fileFormatVersion: 2")
    lines.append(f"guid: {guid}")
    lines.append("TextureImporter:")
    lines.append("  internalIDToNameTable: []")
    lines.append("  externalObjects: {}")
    lines.append("  serializedVersion: 11")
    for section in [
        "  mipmaps:",
        "    mipMapMode: 0",
        "    enableMipMap: 0",
        "    sRGBTexture: 1",
        "    linearTexture: 0",
        "    fadeOut: 0",
        "    borderMipMap: 0",
        "    mipMapsPreserveCoverage: 0",
        "    alphaTestReferenceValue: 0.5",
        "    mipMapFadeDistanceStart: 1",
        "    mipMapFadeDistanceEnd: 3",
        "  bumpmap:",
        "    convertToNormalMap: 0",
        "    externalNormalMap: 0",
        "    heightScale: 0.25",
        "    normalMapFilter: 0",
        "  isReadable: 0",
        "  streamingMipmaps: 0",
        "  streamingMipmapsPriority: 0",
        "  vTOnly: 0",
        "  ignoreMasterTextureLimit: 0",
        "  grayScaleToAlpha: 0",
        "  generateCubemap: 6",
        "  cubemapConvolution: 0",
        "  seamlessCubemap: 0",
        "  textureFormat: 1",
        "  maxTextureSize: 2048",
        "  textureSettings:",
        "    serializedVersion: 2",
        "    filterMode: 0",
        "    aniso: 1",
        "    mipBias: 0",
        "    wrapU: 1",
        "    wrapV: 1",
        "    wrapW: 1",
        "  nPOTScale: 0",
        "  lightmap: 0",
        "  compressionQuality: 50",
        f"  spriteMode: {2 if len(sprites) > 1 else 1}",
        "  spriteExtrude: 1",
        "  spriteMeshType: 1",
        "  alignment: 0",
        f"  spritePivot: {{x: {pivot[0]}, y: {pivot[1]}}}",
        f"  spritePixelsToUnits: {ppu}",
        "  spriteBorder: {x: 0, y: 0, z: 0, w: 0}",
        "  spriteGenerateFallbackPhysicsShape: 1",
        "  alphaUsage: 1",
        "  alphaIsTransparency: 1",
        "  spriteTessellationDetail: -1",
        "  textureType: 8",
        "  textureShape: 1",
        "  singleChannelComponent: 0",
        "  flipbookRows: 1",
        "  flipbookColumns: 1",
        "  maxTextureSizeSet: 0",
        "  compressionQualitySet: 0",
        "  textureFormatSet: 0",
        "  ignorePngGamma: 0",
        "  applyGammaDecoding: 0",
    ]:
        lines.append(section)
    lines.append("  platformSettings:")
    for target in ["DefaultTexturePlatform", "Standalone", "Server", "Android"]:
        comp = "0" if target == "DefaultTexturePlatform" else "1"
        lines.append("  - serializedVersion: 3")
        lines.append(f"    buildTarget: {target}")
        lines.append("    maxTextureSize: 2048")
        lines.append("    resizeAlgorithm: 0")
        lines.append("    textureFormat: -1")
        lines.append(f"    textureCompression: {comp}")
        lines.append("    compressionQuality: 50")
        lines.append("    crunchedCompression: 0")
        lines.append("    allowsAlphaSplitting: 0")
        lines.append("    overridden: 0")
        lines.append("    androidETC2FallbackOverride: 0")
        lines.append("    forceMaximumCompressionQuality_BC6H_BC7: 0")
    lines.append("  spriteSheet:")
    lines.append("    serializedVersion: 2")
    lines.append("    sprites:")
    id_map = {}
    nft = []
    for name, x, y, w, h in sprites:
        iid = stable_id(png_name + "/" + name)
        id_map[name] = iid
        nft.append(f"      {name}: {iid}")
        uy = tex_h - y - h
        lines.append("    - serializedVersion: 2")
        lines.append(f"      name: {name}")
        lines.append("      rect:")
        lines.append("        serializedVersion: 2")
        lines.append(f"        x: {x}")
        lines.append(f"        y: {uy}")
        lines.append(f"        width: {w}")
        lines.append(f"        height: {h}")
        lines.append("      alignment: 0")
        lines.append(f"      pivot: {{x: {pivot[0]}, y: {pivot[1]}}}")
        lines.append("      border: {x: 0, y: 0, z: 0, w: 0}")
        lines.append("      outline: []")
        lines.append("      physicsShape: []")
        lines.append("      tessellationDetail: 0")
        lines.append("      bones: []")
        lines.append(f"      spriteID: {uuid.uuid4().hex}")
        lines.append(f"      internalID: {iid}")
        lines.append("      vertices: []")
        lines.append("      indices: ")
        lines.append("      edges: []")
        lines.append("      weights: []")
    lines.append("    outline: []")
    lines.append("    physicsShape: []")
    lines.append("    bones: []")
    lines.append("    spriteID: 5e97eb03825dee720800000000000000")
    lines.append("    internalID: 0")
    lines.append("    vertices: []")
    lines.append("    indices: ")
    lines.append("    edges: []")
    lines.append("    weights: []")
    lines.append("    secondaryTextures: []")
    lines.append("    nameFileIdTable:")
    lines.extend(nft)
    lines.append("  spritePackingTag: ")
    lines.append("  pSDRemoveMatte: 0")
    lines.append("  pSDShowRemoveMatteOption: 0")
    lines.append("  userData: ")
    lines.append("  assetBundleName: ")
    lines.append("  assetBundleVariant: ")
    with open(path, "w", encoding="utf-8") as f:
        f.write("\n".join(lines) + "\n")
    return id_map, guid


def sprite_list_from_sheet(fw, fh, count, names, pad=0):
    sprites = []
    x = 0
    for i, name in enumerate(names):
        sprites.append((name, x, 0, fw, fh))
        x += fw + pad
    return sprites


def write_anim(path, clip_name, guid, keys, loop=True):
    """keys: list of (time, sprite_internal_id)"""
    os.makedirs(os.path.dirname(path), exist_ok=True)
    duration = keys[-1][0] + 1 / 60
    lines = [
        "%YAML 1.1",
        "%TAG !u! tag:unity3d.com,2011:",
        "--- !u!74 &7400000",
        "AnimationClip:",
        "  m_ObjectHideFlags: 0",
        "  m_CorrespondingSourceObject: {fileID: 0}",
        "  m_PrefabInstance: {fileID: 0}",
        "  m_PrefabAsset: {fileID: 0}",
        f"  m_Name: {clip_name}",
        "  serializedVersion: 6",
        "  m_Legacy: 0",
        "  m_Compressed: 0",
        "  m_UseHighQualityCurve: 1",
        "  m_RotationCurves: []",
        "  m_CompressedRotationCurves: []",
        "  m_EulerCurves: []",
        "  m_PositionCurves: []",
        "  m_ScaleCurves: []",
        "  m_FloatCurves: []",
        "  m_PPtrCurves:",
        "  - curve:",
    ]
    for t, iid in keys:
        lines.append(f"    - time: {t}")
        lines.append(f"      value: {{fileID: {iid}, guid: {guid}, type: 3}}")
    lines += [
        "    attribute: m_Sprite",
        "    path: ",
        "    classID: 212",
        "    script: {fileID: 0}",
        "  m_SampleRate: 60",
        "  m_WrapMode: 0",
        "  m_Bounds:",
        "    m_Center: {x: 0, y: 0, z: 0}",
        "    m_Extent: {x: 0, y: 0, z: 0}",
        "  m_ClipBindingConstant:",
        "    genericBindings:",
        "    - serializedVersion: 2",
        "      path: 0",
        "      attribute: 0",
        "      script: {fileID: 0}",
        "      typeID: 212",
        "      customType: 23",
        "      isPPtrCurve: 1",
        "    pptrCurveMapping:",
    ]
    for _, iid in keys:
        lines.append(f"    - {{fileID: {iid}, guid: {guid}, type: 3}}")
    lines += [
        "  m_AnimationClipSettings:",
        "    serializedVersion: 2",
        "    m_AdditiveReferencePoseClip: {fileID: 0}",
        "    m_AdditiveReferencePoseTime: 0",
        "    m_StartTime: 0",
        f"    m_StopTime: {duration}",
        "    m_OrientationOffsetY: 0",
        "    m_Level: 0",
        "    m_CycleOffset: 0",
        "    m_HasAdditiveReferencePose: 0",
        f"    m_LoopTime: {1 if loop else 0}",
        "    m_LoopBlend: 0",
        "    m_LoopBlendOrientation: 0",
        "    m_LoopBlendPositionY: 0",
        "    m_LoopBlendPositionXZ: 0",
        "    m_KeepOriginalOrientation: 0",
        "    m_KeepOriginalPositionY: 1",
        "    m_KeepOriginalPositionXZ: 0",
        "    m_HeightFromFeet: 0",
        "    m_Mirror: 0",
        "  m_EditorCurves: []",
        "  m_EulerEditorCurves: []",
        "  m_HasGenericRootTransform: 0",
        "  m_HasMotionFloatCurves: 0",
        "  m_Events: []",
    ]
    with open(path, "w", encoding="utf-8") as f:
        f.write("\n".join(lines) + "\n")
    ag = uuid.uuid4().hex
    with open(path + ".meta", "w", encoding="utf-8") as f:
        f.write(
            f"fileFormatVersion: 2\nguid: {ag}\nNativeFormatImporter:\n"
            "  externalObjects: {}\n  mainObjectFileID: 7400000\n"
            "  userData: \n  assetBundleName: \n  assetBundleVariant: \n"
        )
    return ag


def main():
    assets = []

    # microwave timer
    mt = gen_microwave_timer()
    p = "microwave_timer.png"
    mt.save(os.path.join(OUT, p))
    names = [f"microwave_timer_{i}" for i in range(11)]
    sp = sprite_list_from_sheet(32, 24, 11, names)
    ids, g = write_meta(p, sp)
    keys = [(i * 0.5, ids[names[i]]) for i in range(11)]
    write_anim(
        os.path.join(ROOT, "..", "..", "Animations", "Microwave", "timer.anim"),
        "timer",
        g,
        keys,
        loop=False,
    )
    assets.append(p)

    # singles
    singles = [
        ("mentos.png", gen_mentos(), [("mentos", 0, 0, 8, 12)]),
        ("coca_cola.png", gen_cola(), [("coca_cola", 0, 0, 10, 20)]),
        ("popcorn_mountain.png", gen_popcorn_mountain(), [("popcorn_mountain", 0, 0, 40, 36)]),
        ("popcorn_bag.png", gen_popcorn_bag(), [("popcorn_bag", 0, 0, 14, 18)]),
    ]
    for p, img, sp in singles:
        img.save(os.path.join(OUT, p))
        write_meta(p, sp, pivot=(0, 0) if "mountain" in p else (0.5, 0.5))
        assets.append(p)

    sh = gen_spring_hanging()
    p = "spring_hanging.png"
    sh.save(os.path.join(OUT, p))
    sn = [f"spring_hanging_{i}" for i in range(4)]
    sp = sprite_list_from_sheet(16, 30, 4, sn)
    ids, g = write_meta(p, sp, pivot=(0.5, 1.0))
    keys = [(i * 0.15, ids[sn[i]]) for i in range(4)]
    keys.append((0.6, ids[sn[0]]))
    write_anim(
        os.path.join(ROOT, "..", "..", "Animations", "Spring", "hanging.anim"),
        "hanging",
        g,
        keys,
    )
    assets.append(p)

    sl = gen_spring_launch()
    p = "spring_launch.png"
    sl.save(os.path.join(OUT, p))
    sn = [f"spring_launch_{i}" for i in range(6)]
    sp = sprite_list_from_sheet(16, 32, 6, sn)
    ids, g = write_meta(p, sp, pivot=(0.5, 0.0))
    keys = [(i * 0.08, ids[sn[i]]) for i in range(6)]
    write_anim(
        os.path.join(ROOT, "..", "..", "Animations", "Spring", "launch.anim"),
        "launch",
        g,
        keys,
        loop=False,
    )
    assets.append(p)

    fk = gen_fork()
    p = "fork.png"
    fk.save(os.path.join(OUT, p))
    sn = [f"fork_{i}" for i in range(8)]
    sp = sprite_list_from_sheet(20, 28, 8, sn)
    ids, g = write_meta(p, sp, pivot=(0.5, 0.0))
    keys = [
        (0, ids["fork_0"]),
        (0.08, ids["fork_1"]),
        (0.16, ids["fork_2"]),
        (0.24, ids["fork_3"]),
        (0.32, ids["fork_4"]),
        (0.42, ids["fork_5"]),
        (0.52, ids["fork_6"]),
        (0.62, ids["fork_7"]),
        (0.72, ids["fork_0"]),
    ]
    write_anim(
        os.path.join(ROOT, "..", "..", "Animations", "Fork", "swing.anim"),
        "swing",
        g,
        keys,
    )
    assets.append(p)

    ss = gen_shelf_sheet()
    p = "shelf.png"
    ss.save(os.path.join(OUT, p))
    sp = [
        ("shelf_28", 0, 10, 28, 8),
        ("shelf_40", 28, 10, 40, 8),
        ("shelf_items_28", 68, 0, 28, 18),
    ]
    write_meta(p, sp, pivot=(0, 0), tex_h=18)
    assets.append(p)

    pr = gen_shelf_props()
    p = "shelf_props.png"
    pr.save(os.path.join(OUT, p))
    sp = [
        ("prop_jar_blue", 0, 0, 8, 12),
        ("prop_jar_honey", 10, 0, 8, 12),
        ("prop_can", 20, 0, 6, 10),
    ]
    write_meta(p, sp, pivot=(0.5, 0.0))
    assets.append(p)

    print("Generated:", ", ".join(assets))


if __name__ == "__main__":
    main()
