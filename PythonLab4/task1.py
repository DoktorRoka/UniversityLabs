import os
import sys
import struct
import argparse

GENRES = [
    "Blues", "Classic Rock", "Country", "Dance", "Disco", "Funk", "Grunge",
    "Hip-Hop", "Jazz", "Metal", "New Age", "Oldies", "Other", "Pop", "R&B",
    "Rap", "Reggae", "Rock", "Techno", "Industrial", "Alternative", "Ska",
    "Death Metal", "Pranks", "Soundtrack", "Euro-Techno", "Ambient",
    "Trip-Hop", "Vocal", "Jazz+Funk", "Fusion", "Trance", "Classical",
    "Instrumental", "Acid", "House", "Game", "Sound Clip", "Gospel", "Noise",
    "AlternRock", "Bass", "Soul", "Punk", "Space", "Meditative",
    "Instrumental Pop", "Instrumental Rock", "Ethnic", "Gothic", "Darkwave",
    "Techno-Industrial", "Electronic", "Pop-Folk", "Eurodance", "Dream",
    "Southern Rock", "Comedy", "Cult", "Gangsta", "Top 40", "Christian Rap",
    "Pop/Funk", "Jungle", "Native American", "Cabaret", "New Wave",
    "Psychadelic", "Rave", "Showtunes", "Trailer", "Lo-Fi", "Tribal",
    "Acid Punk", "Acid Jazz", "Polka", "Retro", "Musical", "Rock & Roll",
    "Hard Rock"
]


def read_id3v1(filepath):
    with open(filepath, "rb") as f:
        f.seek(-128, 2)
        data = f.read(128)

    header = data[0:3].decode("ascii", errors="replace")
    if header != "TAG":
        return None

    title = data[3:33].decode("ascii", errors="replace").strip("\x00").strip()
    artist = data[33:63].decode("ascii", errors="replace").strip("\x00").strip()
    album = data[63:93].decode("ascii", errors="replace").strip("\x00").strip()
    year = data[93:97].decode("ascii", errors="replace").strip("\x00").strip()
    comment = data[97:127].decode("ascii", errors="replace").strip("\x00").strip()
    zero_byte = data[125]
    track = data[126] if zero_byte == 0 else 0
    genre_byte = data[127]

    return {
        "header": header,
        "title": title,
        "artist": artist,
        "album": album,
        "year": year,
        "comment": comment,
        "zero_byte": zero_byte,
        "track": track,
        "genre": genre_byte,
    }


def write_id3v1(filepath, tag):
    with open(filepath, "r+b") as f:
        f.seek(-128, 2)
        title = tag["title"].ljust(30, "\x00")[:30]
        artist = tag["artist"].ljust(30, "\x00")[:30]
        album = tag["album"].ljust(30, "\x00")[:30]
        year = tag["year"].ljust(4, "\x00")[:4]
        comment = tag["comment"].ljust(28, "\x00")[:28]
        zero_byte = b"\x00"
        track_byte = struct.pack("B", tag["track"])
        genre_byte = struct.pack("B", tag["genre"])
        data = b"TAG" + title.encode("ascii", errors="replace") + \
               artist.encode("ascii", errors="replace") + \
               album.encode("ascii", errors="replace") + \
               year.encode("ascii", errors="replace") + \
               comment.encode("ascii", errors="replace") + \
               zero_byte + track_byte + genre_byte
        f.write(data)


def hex_dump(data):
    lines = []
    for i in range(0, len(data), 16):
        chunk = data[i:i + 16]
        hex_part = " ".join(f"{b:02X}" for b in chunk)
        ascii_part = "".join(chr(b) if 32 <= b < 127 else "." for b in chunk)
        lines.append(f"{i:04X}  {hex_part:<48s}  {ascii_part}")
    return "\n".join(lines)


def main():
    parser = argparse.ArgumentParser(description="Read ID3v1 tags from mp3 files")
    parser.add_argument("directory", help="Directory containing mp3 files")
    parser.add_argument("-d", action="store_true", help="Show hex dump of tags")
    parser.add_argument("-g", type=int, default=None, metavar="GENRE",
                        help="Genre number to set if not specified in tag")
    args = parser.parse_args()

    if not os.path.isdir(args.directory):
        print(f"Directory not found: {args.directory}")
        sys.exit(1)

    mp3_files = [f for f in os.listdir(args.directory) if f.lower().endswith(".mp3")]

    if not mp3_files:
        print("No mp3 files found.")
        return

    for filename in sorted(mp3_files):
        filepath = os.path.join(args.directory, filename)
        tag = read_id3v1(filepath)
        if tag is None:
            print(f"{filename}: No ID3v1 tag found")
            continue

        modified = False

        if tag["track"] == 0:
            tag["track"] = mp3_files.index(filename) + 1
            modified = True
            print(f"  -> Auto-assigned track #{tag['track']}")

        if tag["genre"] == 255 and args.g is not None:
            tag["genre"] = args.g
            modified = True
            genre_name = GENRES[args.g] if 0 <= args.g < len(GENRES) else "Unknown"
            print(f"  -> Auto-assigned genre: {genre_name} ({args.g})")

        if modified:
            write_id3v1(filepath, tag)

        genre_name = GENRES[tag["genre"]] if 0 <= tag["genre"] < len(GENRES) else "None"
        print(f"[{tag['artist']}] - [{tag['title']}] - [{tag['album']}]")

        if args.d:
            with open(filepath, "rb") as f:
                f.seek(-128, 2)
                raw = f.read(128)
            print(f"  Hex dump of ID3v1 tag for {filename}:")
            print(hex_dump(raw))
            print()


if __name__ == "__main__":
    main()
