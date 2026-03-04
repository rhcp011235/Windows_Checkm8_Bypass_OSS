"""
replace_exe_icon__app_ico.py
----------------------------
Generates a new app.ico and writes it to the project root.

app.ico is the icon embedded into CheckM8.exe at build time.
It appears as:
  - The file icon in Windows Explorer
  - The taskbar icon when the app is running
  - The title bar icon (pulled dynamically via Icon.ExtractAssociatedIcon)

The ICO contains all 9 standard Windows sizes:
  16x16, 24x24, 32x32, 48x48, 64x64, 72x72, 96x96, 128x128, 256x256

Requirements:
  pip install Pillow

Usage:
  python scripts/replace_exe_icon__app_ico.py
"""

from PIL import Image, ImageDraw, ImageFont
import struct, io, os

# --- CONFIGURATION ---
LINE1       = "rhcp"       # top text
LINE2       = "011235"     # bottom text
BG_COLOR    = (25, 25, 25, 255)       # background fill (R,G,B,A)
TEXT_COLOR1 = (210, 210, 210, 255)    # top line color
TEXT_COLOR2 = (160, 160, 160, 255)    # bottom line color
FONT_PATH   = "/System/Library/Fonts/Supplemental/Arial Bold.ttf"
OUTPUT_PATH = os.path.join(os.path.dirname(__file__), "..", "app.ico")
# ---------------------


def make_frame(size):
    img = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    pad = max(1, size // 12)
    r = max(2, size // 6)
    draw.rounded_rectangle([pad, pad, size - pad - 1, size - pad - 1],
                            radius=r, fill=BG_COLOR)

    if size >= 48:
        f1 = ImageFont.truetype(FONT_PATH, max(5, int(size * 0.285)))
        f2 = ImageFont.truetype(FONT_PATH, max(4, int(size * 0.245)))
        b1 = draw.textbbox((0, 0), LINE1, font=f1)
        b2 = draw.textbbox((0, 0), LINE2, font=f2)
        gap = max(1, size // 20)
        total_h = (b1[3] - b1[1]) + gap + (b2[3] - b2[1])
        yo = (size - total_h) // 2
        draw.text(((size - (b1[2] - b1[0])) // 2 - b1[0], yo - b1[1]),
                  LINE1, font=f1, fill=TEXT_COLOR1)
        draw.text(((size - (b2[2] - b2[0])) // 2 - b2[0],
                   yo + (b1[3] - b1[1]) + gap - b2[1]),
                  LINE2, font=f2, fill=TEXT_COLOR2)
    elif size >= 24:
        f = ImageFont.truetype(FONT_PATH, max(4, int(size * 0.40)))
        t = LINE1[:2]
        b = draw.textbbox((0, 0), t, font=f)
        draw.text(((size - (b[2] - b[0])) // 2 - b[0],
                   (size - (b[3] - b[1])) // 2 - b[1]),
                  t, font=f, fill=TEXT_COLOR1)
    else:
        try:
            f = ImageFont.truetype(FONT_PATH, max(4, int(size * 0.50)))
        except Exception:
            f = ImageFont.load_default()
        t = LINE1[0]
        b = draw.textbbox((0, 0), t, font=f)
        draw.text(((size - (b[2] - b[0])) // 2 - b[0],
                   (size - (b[3] - b[1])) // 2 - b[1]),
                  t, font=f, fill=TEXT_COLOR1)
    return img


def build_ico(sizes):
    frames = [(s, make_frame(s)) for s in sizes]
    n = len(frames)
    header = struct.pack("<HHH", 0, 1, n)
    dir_entries = b""
    images_data = b""
    data_offset = 6 + n * 16

    for sz, img in frames:
        buf = io.BytesIO()
        img.save(buf, format="PNG")
        png = buf.getvalue()
        w = sz if sz < 256 else 0
        h = sz if sz < 256 else 0
        dir_entries += struct.pack("<BBBBHHII",
                                   w, h, 0, 0, 1, 32, len(png), data_offset)
        images_data += png
        data_offset += len(png)

    return header + dir_entries + images_data


if __name__ == "__main__":
    sizes = [256, 128, 96, 72, 64, 48, 32, 24, 16]
    ico_data = build_ico(sizes)
    out = os.path.normpath(OUTPUT_PATH)
    with open(out, "wb") as f:
        f.write(ico_data)
    print(f"Written: {out}  ({len(ico_data):,} bytes, {len(sizes)} sizes)")
    print("Rebuild the project for changes to take effect.")
