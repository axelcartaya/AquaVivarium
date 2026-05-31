# AquaVivarium 🐟🌿

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![Blazor](https://img.shields.io/badge/Blazor-512BD4?style=for-the-badge&logo=blazor)
![MudBlazor](https://img.shields.io/badge/MudBlazor-594AE2?style=for-the-badge&logo=MudBlazor&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC292B?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

AquaVivarium es una plataforma web integral diseñada para revolucionar la gestión de ecosistemas acuáticos. Orientada tanto a acuaristas principiantes como experimentados, la aplicación permite llevar un control exhaustivo de los parámetros del agua, consultar un amplio catálogo de especies y participar en una comunidad activa.

Su funcionalidad estrella es el **Simulador de Compatibilidad Biológica**, un motor híbrido que usa lógica y procesamiento con IA que pre-valida si una especie (pez o planta) puede sobrevivir y convivir adecuadamente en función de los litros, el pH, la temperatura, entre otros parámetros y además también valida compatbilidad inter-especie.

## Características Principales

* **Gestión de Acuarios:** CRUD completo para registrar tus acuarios con parametros, habitantes y características.
* **Catálogo y Simulador:** Base de datos jerárquica de fauna y flora con validación de compatibilidad biológica en tiempo real.
* **Comunidad:** Foro integrado para resolver dudas y compartir experiencias. Además de una sección de guías y consejos para el cuidado de los acuarios.
* **Seguridad:** Autenticación y autorización basada en roles mediante ASP.NET Core Identity.
* **Diseño Responsivo:** Interfaz *Mobile First* construida con MudBlazor.

---

## Demo

🔗 **Demo en vivo:** [https://aquavivarium.onrender.com/](https://aquavivarium.onrender.com/)

---

## ⚙️ Requisitos Previos

Para desplegar y ejecutar el proyecto en un entorno local, necesitas tener instaladas las siguientes herramientas:

* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (con el motor en ejecución).
* [Git](https://git-scm.com/downloads) para la clonación del repositorio.
* *(Opcional)* SDK de .NET 10 y Visual Studio 2026 si deseas compilar y modificar el código nativamente sin contenedores.

---

## 🐳 Ejecución en Local (Docker)

La forma más rápida y segura de levantar el proyecto es mediante la orquestación de contenedores. El entorno incluye un *script* automatizado que genera la base de datos e inserta los datos semilla (*Seed*) sin intervención manual.

1. Abre una terminal en el directorio raíz del proyecto.
2. Ejecuta el siguiente comando para construir las imágenes y levantar los contenedores en segundo plano:

```bash
docker-compose up -d --build