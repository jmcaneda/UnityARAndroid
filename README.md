# UnityARAndroid
# 📱 Proyecto AR en Unity 6.2 para Android

## 🧰 Configuración Inicial

### 🎯 Objetivo
Desarrollar una experiencia de Realidad Aumentada modular y documentada, compatible con Android 10, utilizando Unity 6.2, AR Foundation y ARCore.

---

## 🧱 Entorno de Desarrollo

### 🧩 Unity
- **Versión:** Unity 6.2
- **Template:** AR Mobile
- **Nombre del proyecto:** `UnityARAndroid`
- **Nombre de la Scene:** `ARAndroid`

### 📦 Módulos instalados desde Unity Hub
- Android Build Support
- Android SDK & NDK Tools
- OpenJDK
- ARCore XR Plugin (desde Package Manager)

---

## 📱 Dispositivo de Pruebas

### 🔍 Información del dispositivo
- **Modelo:** Redmi Note 8 Pro
- **Versión de Android:** 10 (QP1A.190711.020)
- **Modo desarrollador:** Activado

### ⚙️ Configuraciones necesarias
- [x] Activar opciones de desarrollador
- [x] Activar depuración USB
- [x] Permitir instalación de apps vía USB
- [x] Conectar por cable y verificar con `adb devices`

## ⚙️ Guia de implantacion
- [x] UnityARAndroid.pdf

# 📱 Resumen funcional de la aplicación AR

### 🎯 ¿Qué hace la app?
La aplicación permite **colocar, manipular y eliminar objetos 3D en un entorno de realidad aumentada** usando la cámara del móvil y gestos táctiles.  
Está diseñada para dispositivos Android compatibles con **ARCore** y utiliza el **nuevo Input System de Unity** para interpretar los toques en pantalla.

---

## 🚀 Cómo funciona

1. **Inicio**
   - La app comprueba si tu móvil soporta ARCore.
   - Si es compatible, activa la cámara y detecta superficies planas, solo horizontales (suelo, mesa, pared).

2. **Colocar objetos**
   - Toca la pantalla sobre una superficie detectada.
   - Se colocará un objeto 3D (The train, objeto creado con Blender) en esa posición.
   - La app evita que se coloquen objetos demasiado cerca unos de otros.

3. **Seleccionar y manipular**
   - Con un dedo puedes:
     - **Seleccionarlo**, con un único toque, el train cambia de color.
     - **Moverlo** arrastrando sobre la superficie.
   - Con dos dedos puedes:
     - **Escalarlo** con gesto de pinza (acercar/alejar dedos).
     - **Rotarlo** girando los dedos.

4. **Eliminar objetos**
   - Haz **doble toque** sobre un objeto seleccionado para eliminarlo con efectos visuales.
   - También puedes activar una **mirilla** en el centro de la pantalla y disparar al objeto que esté ahí.

5. **Mensajes en pantalla**
   - La app muestra mensajes informativos en la interfaz (ej. “Objeto colocado”, “Objeto eliminado”, “Mirilla activada”) para guiarte en todo momento.

---

## 📦 Requisitos para probarla
- Dispositivo Android compatible con **ARCore**.  
- Cámara activa y permisos concedidos.  
- Buen nivel de luz para que la cámara detecte superficies.  
- Espacio plano (mesa, suelo) para colocar objetos.  

---

## 💡 Experiencia del usuario
En pocos segundos podrás:
- Detectar tu entorno con la cámara.  
- Colocar objetos virtuales sobre superficies reales.  
- Manipularlos con gestos naturales.  
- Eliminar objetos con un doble toque o disparo desde la mirilla.  

La aplicación convierte tus gestos táctiles en acciones claras dentro del mundo aumentado, ofreciendo una experiencia intuitiva y divertida.


