#!/usr/bin/env python
# -*- coding: utf-8 -*-
import subprocess
import os
import shutil
import sys

src_dir = os.path.join("D:", os.sep, "已完成项目", "BookManagementSystem")
csc_exe = r"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
mysql_dll = r"C:\Program Files (x86)\MySQL\MySQL Installer for Windows\MySql.Data.dll"
out_dir = os.path.join(src_dir, "bin")
out_exe = os.path.join(out_dir, "BookManagementSystem.exe")

print(f"Source: {src_dir}")
print(f"CSC: {csc_exe}")
print(f"MySQL DLL: {mysql_dll}")

if not os.path.exists(csc_exe):
    print("[ERROR] csc.exe not found!")
    sys.exit(1)
if not os.path.exists(mysql_dll):
    print("[ERROR] MySql.Data.dll not found!")
    sys.exit(1)

# Create output dir
os.makedirs(out_dir, exist_ok=True)

# Copy MySQL DLL
shutil.copy2(mysql_dll, os.path.join(out_dir, "MySql.Data.dll"))
print("Copied MySql.Data.dll to bin/")

# Source files
sources = [
    "Program.cs", "DBHelper.cs", "LoginForm.cs", "RegisterForm.cs",
    "MainMenuForm.cs", "ReaderCategoryForm.cs", "ReaderManagementForm.cs",
    "BookManagementForm.cs", "BorrAndRetForm.cs"
]

for f in sources:
    fp = os.path.join(src_dir, f)
    if not os.path.exists(fp):
        print(f"[ERROR] Missing: {fp}")
        sys.exit(1)

print("All sources found. Compiling...")

# Build command
cmd = [
    csc_exe,
    "/nologo",
    "/target:winexe",
    f"/out:{out_exe}",
    "/reference:System.dll",
    "/reference:System.Windows.Forms.dll",
    "/reference:System.Drawing.dll",
    "/reference:System.Data.dll",
    "/reference:System.Xml.dll",
    f"/reference:{mysql_dll}",
] + [os.path.join(src_dir, s) for s in sources]

result = subprocess.run(cmd, capture_output=True, text=True, encoding="gbk")
print(result.stdout)
if result.stderr:
    print(result.stderr)

if result.returncode == 0:
    print("\n=== BUILD SUCCESS ===")
    print(f"Output: {out_exe}")
else:
    print("\n=== BUILD FAILED ===")
    sys.exit(1)
