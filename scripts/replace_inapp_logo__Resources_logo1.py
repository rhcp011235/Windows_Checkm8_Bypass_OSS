"""
replace_inapp_logo__Resources_logo1.py
---------------------------------------
Generates a new logo image and injects it into Titan.Properties.Resources.resx
as the "logo1" resource (the large icon shown in the middle of the running app).

logo1 is displayed by pictureBox1 inside guna2Panel1:
  Location : (18, 50) on the form
  Display  : 228 x 152 px (Zoom mode — image keeps aspect ratio)
  Source   : Titan.Properties.Resources.logo1 (PNG, 1142 x 1142 stored)

The resource is serialized using .NET BinaryFormatter wrapping a
System.Drawing.Bitmap. This script patches the binary blob in-place,
preserving the wrapper so .NET can still deserialize it correctly.

Requirements:
  pip install Pillow

Usage:
  python scripts/replace_inapp_logo__Resources_logo1.py
"""

import re, base64, io, os, struct
from PIL import Image, ImageDraw, ImageFont

# --- CONFIGURATION ---
LINE1       = "rhcp"       # top text
LINE2       = "011235"     # bottom text
IMG_SIZE    = 1142                        # stored PNG canvas (square px)
BG_COLOR    = (25, 25, 25, 255)           # background fill (R,G,B,A)
TEXT_COLOR1 = (210, 210, 210, 255)        # top line color
TEXT_COLOR2 = (160, 160, 160, 255)        # bottom line color
FONT_PATH   = "/System/Library/Fonts/Supplemental/Arial Bold.ttf"
RESX_PATH   = os.path.join(os.path.dirname(__file__), "..",
                            "Titan.Properties.Resources.resx")
# ---------------------


def generate_png(size):
    img = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    pad = size // 12
    r = size // 6
    draw.rounded_rectangle([pad, pad, size - pad - 1, size - pad - 1],
                            radius=r, fill=BG_COLOR)

    f1 = ImageFont.truetype(FONT_PATH, int(size * 0.285))
    f2 = ImageFont.truetype(FONT_PATH, int(size * 0.245))
    b1 = draw.textbbox((0, 0), LINE1, font=f1)
    b2 = draw.textbbox((0, 0), LINE2, font=f2)
    gap = size // 20
    total_h = (b1[3] - b1[1]) + gap + (b2[3] - b2[1])
    yo = (size - total_h) // 2

    draw.text(((size - (b1[2] - b1[0])) // 2 - b1[0], yo - b1[1]),
              LINE1, font=f1, fill=TEXT_COLOR1)
    draw.text(((size - (b2[2] - b2[0])) // 2 - b2[0],
               yo + (b1[3] - b1[1]) + gap - b2[1]),
              LINE2, font=f2, fill=TEXT_COLOR2)

    buf = io.BytesIO()
    img.save(buf, format="PNG")
    return buf.getvalue()


def patch_resx(resx_path, resource_name, new_png_bytes):
    resx = open(resx_path, "rb").read().decode("utf-8", errors="ignore")

    pattern = rf'(name="{re.escape(resource_name)}"[^>]*>.*?<value>)(.*?)(</value>)'
    m = re.search(pattern, resx, re.DOTALL)
    if not m:
        raise ValueError(f'Resource "{resource_name}" not found in {resx_path}')

    old_data = base64.b64decode(m.group(2).strip())

    # Locate PNG inside the BinaryFormatter blob
    png_offset = old_data.find(b"\x89PNG")
    if png_offset == -1:
        raise ValueError("No PNG signature found in existing resource blob")

    iend = old_data.rfind(b"IEND")
    footer = old_data[iend + 8:]   # trailing BinaryFormatter end marker (0x0b)
    header = bytearray(old_data[:png_offset])

    # Patch the 4-byte LE length field in the header
    old_png_size = iend + 8 - png_offset
    old_len_bytes = struct.pack("<I", old_png_size)
    len_pos = bytes(header).rfind(old_len_bytes)
    if len_pos == -1:
        raise ValueError("Could not locate PNG length field in BinaryFormatter header")

    header[len_pos:len_pos + 4] = struct.pack("<I", len(new_png_bytes))

    new_data = bytes(header) + new_png_bytes + footer
    new_b64 = base64.b64encode(new_data).decode()

    new_resx = resx[:m.start(2)] + new_b64 + resx[m.end(2):]
    open(resx_path, "w", encoding="utf-8").write(new_resx)

    return len(old_data), len(new_data)


if __name__ == "__main__":
    resx = os.path.normpath(RESX_PATH)
    print(f"Generating {IMG_SIZE}x{IMG_SIZE} PNG...")
    new_png = generate_png(IMG_SIZE)
    print(f"PNG size: {len(new_png):,} bytes")

    old_sz, new_sz = patch_resx(resx, "logo1", new_png)
    print(f"Patched:  {resx}")
    print(f"  blob {old_sz:,} bytes  →  {new_sz:,} bytes")
    print("Rebuild the project for changes to take effect.")
