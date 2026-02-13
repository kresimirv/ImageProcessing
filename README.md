# ImageProcessing

**Simple Image Processing Client & Server applications using GDI+ in C#**

This project provides a basic image processing system composed of a server and a client application. It demonstrates how to send images from a desktop client to a server, receive processing results, and display them in a GUI using **GDI+** for rendering and manipulation.

---

## 🧠 Overview

The repository contains two main applications:

1. **ImageProcessing Server** – listens for connections and performs simple image processing tasks
2. **ImageProcessing Client** – connects to the server, sends images, and displays results

Both applications are built in **C#** with **Windows Forms** and use **GDI+ (`System.Drawing`)** for image rendering and display.

---

## 🚀 Features

✔ Desktop GUI for loading and displaying images  
✔ Client-Server communication over TCP  
✔ Basic image processing operations  
✔ Use of GDI+ for rendering and bitmap manipulation  
✔ Demonstrates socket programming and image serialization

---

## 🛠️ Requirements

To build and run both applications you need:

- **Visual Studio 2019 or later**
- **.NET Framework (Windows Desktop)**
- Windows OS (required for GDI+ / System.Drawing)

---

## 🛠️ Build & Run

### 1. Clone the repository

```bash
git clone https://github.com/kresimirv/ImageProcessing.git
cd ImageProcessing

