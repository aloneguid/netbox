from pathlib import Path
import os

OUT_FILENAME = "NetBox.cs"

cs = []
usings = []
code_lines = []

for path in Path('src/NetBox').rglob('*.cs'):
    if "obj" in str(path.parent) or "Test" in path.name:
        continue

    cs.append(str(path))

for cs_path in cs:

    code_lines.append(f"\n\n// FILE: {cs_path}\n\n")

    with open(cs_path, "r", encoding="utf-8-sig") as cs_fo:
        lines = cs_fo.readlines()

        for line in lines:
            if line.startswith("using"):
                usings.append(line)
            else:
                code_lines.append(line)

usings = list(sorted(set(usings)))

banner = f"""/*
 _   _      _   ____
| \ | | ___| |_| __ )  _____  __
|  \| |/ _ \ __|  _ \ / _ \ \/ /
| |\  |  __/ |_| |_) | (_) >  <
|_| \_|\___|\__|____/ \___/_/\_\\   v{os.getenv("v", "?")} by @aloneguid

https://github.com/aloneguid/netbox
*/

"""

all = [banner]
all.extend(usings)
all.extend(code_lines)

with open(OUT_FILENAME, "w") as f:
    f.writelines(all)


