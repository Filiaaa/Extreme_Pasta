"""Delete broken .meta files and recreate with Unity-valid random GUIDs."""
import os
import re
import uuid

ROOT = os.path.dirname(os.path.abspath(__file__))
ASSETS = os.path.join(ROOT, "..", "..")

FOLDER_METAS = [
    "Animations/Fork.meta",
    "Animations/Microwave.meta",
    "Animations/Spring.meta",
]

SPRITE_METAS = [
    "Sprites/Interior/coca_cola.png.meta",
    "Sprites/Interior/fork.png.meta",
    "Sprites/Interior/popcorn_bag.png.meta",
    "Sprites/Interior/popcorn_mountain.png.meta",
    "Sprites/Interior/shelf.png.meta",
    "Sprites/Interior/shelf_props.png.meta",
    "Sprites/Interior/spring_hanging.png.meta",
    "Sprites/Interior/spring_launch.png.meta",
]

# old texture guids in .anim files -> png meta path
ANIM_GUID_MAP = {
    "f6070809101112131415161718192021": "Sprites/Interior/spring_hanging.png.meta",
    "07080910111213141516171819202122": "Sprites/Interior/spring_launch.png.meta",
    "80910111213141516171819202122232": "Sprites/Interior/fork.png.meta",
}

FOLDER_TEMPLATE = """fileFormatVersion: 2
guid: {guid}
folderAsset: yes
DefaultImporter:
  externalObjects: {{}}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"""

TEXTURE_TEMPLATE = """fileFormatVersion: 2
guid: {guid}
TextureImporter:
  internalIDToNameTable: []
  externalObjects: {{}}
  serializedVersion: 11
  mipmaps:
    mipMapMode: 0
    enableMipMap: 0
    sRGBTexture: 1
    linearTexture: 0
    fadeOut: 0
    borderMipMap: 0
    mipMapsPreserveCoverage: 0
    alphaTestReferenceValue: 0.5
    mipMapFadeDistanceStart: 1
    mipMapFadeDistanceEnd: 3
  bumpmap:
    convertToNormalMap: 0
    externalNormalMap: 0
    heightScale: 0.25
    normalMapFilter: 0
  isReadable: 0
  streamingMipmaps: 0
  streamingMipmapsPriority: 0
  vTOnly: 0
  ignoreMasterTextureLimit: 0
  grayScaleToAlpha: 0
  generateCubemap: 6
  cubemapConvolution: 0
  seamlessCubemap: 0
  textureFormat: 1
  maxTextureSize: 2048
  textureSettings:
    serializedVersion: 2
    filterMode: 0
    aniso: 1
    mipBias: 0
    wrapU: 1
    wrapV: 1
    wrapW: 1
  nPOTScale: 0
  lightmap: 0
  compressionQuality: 50
  spriteMode: {sprite_mode}
  spriteExtrude: 1
  spriteMeshType: 1
  alignment: 0
  spritePivot: {{x: {px}, y: {py}}}
  spritePixelsToUnits: 32
  spriteBorder: {{x: 0, y: 0, z: 0, w: 0}}
  spriteGenerateFallbackPhysicsShape: 1
  alphaUsage: 1
  alphaIsTransparency: 1
  spriteTessellationDetail: -1
  textureType: 8
  textureShape: 1
  singleChannelComponent: 0
  flipbookRows: 1
  flipbookColumns: 1
  maxTextureSizeSet: 0
  compressionQualitySet: 0
  textureFormatSet: 0
  ignorePngGamma: 0
  applyGammaDecoding: 0
  platformSettings:
  - serializedVersion: 3
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 0
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  - serializedVersion: 3
    buildTarget: Standalone
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  - serializedVersion: 3
    buildTarget: Server
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  - serializedVersion: 3
    buildTarget: Android
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  spriteSheet:
    serializedVersion: 2
    sprites: []
    outline: []
    physicsShape: []
    bones: []
    spriteID: 5e97eb03825dee720800000000000000
    internalID: 0
    vertices: []
    indices: 
    edges: []
    weights: []
    secondaryTextures: []
    nameFileIdTable: {{}}
  spritePackingTag: 
  pSDRemoveMatte: 0
  pSDShowRemoveMatteOption: 0
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"""

SPRITE_SETTINGS = {
    "coca_cola.png.meta": (1, 0.5, 0.5),
    "fork.png.meta": (2, 0.5, 0.0),
    "popcorn_bag.png.meta": (1, 0.5, 0.5),
    "popcorn_mountain.png.meta": (1, 0.0, 0.0),
    "shelf.png.meta": (2, 0.0, 0.0),
    "shelf_props.png.meta": (2, 0.5, 0.0),
    "spring_hanging.png.meta": (2, 0.5, 1.0),
    "spring_launch.png.meta": (2, 0.5, 0.0),
}


def new_guid():
    return uuid.uuid4().hex


def main():
    new_guids = {}

    for rel in FOLDER_METAS:
        path = os.path.join(ASSETS, rel.replace("/", os.sep))
        g = new_guid()
        new_guids[rel] = g
        with open(path, "w", encoding="utf-8", newline="\n") as f:
            f.write(FOLDER_TEMPLATE.format(guid=g))
        print("folder", rel, g)

    for rel in SPRITE_METAS:
        path = os.path.join(ASSETS, rel.replace("/", os.sep))
        name = os.path.basename(rel)
        mode, px, py = SPRITE_SETTINGS[name]
        g = new_guid()
        new_guids[rel] = g
        with open(path, "w", encoding="utf-8", newline="\n") as f:
            f.write(
                TEXTURE_TEMPLATE.format(
                    guid=g, sprite_mode=mode, px=px, py=py
                )
            )
        print("sprite", rel, g)

    # update animation texture references
    for old_g, meta_rel in ANIM_GUID_MAP.items():
        new_g = new_guids.get(meta_rel)
        if not new_g:
            continue
        anim_dir = os.path.join(ASSETS, "Animations")
        for dirpath, _, files in os.walk(anim_dir):
            for fn in files:
                if not fn.endswith(".anim"):
                    continue
                fp = os.path.join(dirpath, fn)
                text = open(fp, encoding="utf-8").read()
                if old_g not in text:
                    continue
                text = text.replace(old_g, new_g)
                with open(fp, "w", encoding="utf-8", newline="\n") as f:
                    f.write(text)
                print("anim updated", fp, old_g, "->", new_g)

    # remove broken material refs
    mat = os.path.join(ROOT, "Materials", "coca_cola.mat")
    mat_meta = mat + ".meta"
    for p in (mat, mat_meta):
        if os.path.isfile(p):
            os.remove(p)
            print("removed", p)

    print("Done. Switch to Unity and let it reimport.")


if __name__ == "__main__":
    main()
