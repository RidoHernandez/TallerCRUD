# 🚗 TallerCRUD - Sistema de Gestión para Taller Mecánico (WPF + MySQL)

Proyecto desarrollado en **C# con WPF (.NET)** que implementa un sistema tipo **CRUD** para la administración de un taller mecánico.

El sistema permite gestionar información relacionada con refacciones, servicios, clientes, vehículos y órdenes de servicio, utilizando una base de datos relacional en **MySQL**.

---

## 📷 Vista General

Este proyecto cuenta con una interfaz moderna y minimalista, diseñada en **XAML** con estilos personalizados tipo "CSS", simulando un diseño profesional.

---

## 🎯 Objetivo del Proyecto

Desarrollar un sistema de escritorio que permita:

- Registrar refacciones, servicios, clientes y vehículos.
- Consultar y filtrar información en tablas dinámicas (DataGrid).
- Editar registros existentes.
- Eliminar registros de manera segura.
- Validar datos antes de almacenarlos en la base de datos.
- Facilitar el uso mediante una interfaz intuitiva.

---

## 🛠️ Tecnologías Utilizadas

### 💻 Frontend / Interfaz Gráfica
- **C#**
- **WPF (Windows Presentation Foundation)**
- **XAML**
- **ResourceDictionary (Styles)** para diseño y estilos tipo CSS

### 🗄️ Base de Datos
- **MySQL**
- Diseño relacional con llaves primarias y foráneas

### 🌐 Conexión
- **MySql.Data / MySqlConnector** (para conexión desde C# hacia MySQL)

### 🔧 Control de Versiones
- **Git**
- **GitHub**

---

## 📌 Funcionalidades Principales

✅ CRUD completo para Refacciones  
✅ Tabla interactiva (DataGrid) para mostrar datos  
✅ Selección de registros y carga automática en campos  
✅ Búsqueda y filtrado de información  
✅ Validación de datos en Code-Behind  
✅ Interfaz moderna y minimalista  

---

## 🧾 Estructura del Sistema (Base de Datos)

El sistema se basa en una base de datos que incluye entidades como:

- **Clientes**
- **Vehículos**
- **Mecánicos**
- **Servicios**
- **Refacciones**
- **Órdenes de Servicio**

Incluye relaciones del tipo:

- Un cliente puede tener varios vehículos.
- Un vehículo puede tener varias órdenes de servicio.
- Una orden puede usar varias refacciones.
- Una orden puede incluir varios servicios.
- Una orden puede requerir varios mecánicos.

---

## 🚀 Cómo ejecutar el proyecto

### 1️⃣ Clonar el repositorio
```bash
git clone https://github.com/RidoHernandez/TallerCRUD.git
