# Supply Chain Management (SCM) Solution for Dynamics 365

![Dynamics 365](https://img.shields.io/badge/Dynamics%20365-CE-blue)
![C#](https://img.shields.io/badge/Backend-C%23%20%7C%20.NET-purple)
![TypeScript](https://img.shields.io/badge/Frontend-TypeScript%20%7C%20React-blue)
![Build Status](https://img.shields.io/badge/Build-Unmanaged-green)

## 📖 Overview

This repository contains the source code and solution files for a custom **Supply Chain Management (SCM)** module built on **Microsoft Dataverse**. 

Unlike standard model-driven apps, this project focuses on **Pro-Code extensibility**, implementing complex business logic through **C# Plugins**, **Custom APIs**, and **PCF Controls**. It demonstrates a fully integrated architecture handling supplier validation, inventory automation, and external system synchronization.

---

## 🏗️ Technical Architecture

### 1. Server-Side Business Logic (C# .NET)
The backend logic is decoupled using the **Service-Repository Pattern** to ensure maintainability and testability.

*   **Data Integrity (Pre-Validation):** 
    *   Implemented synchronous plugins to prevent the deletion of "Key Partner" suppliers.
    *   Utilized `QueryExpression` to check for active associated products before allowing deletion.
*   **Process Automation (Post-Operation):**
    *   Automated `Task` creation upon new supplier onboarding.
    *   Logic handles `OutputParameters` to associate tasks with newly created records dynamically.
*   **Audit & Tracking:**
    *   Implemented **Pre-Images** to track price changes on Products. 
    *   System automatically logs audit trails into the description field only when a price drop is detected.

### 2. Custom API (Action)
*   **`scm_ApproveSupplier`:**
    *   A custom unbound action that encapsulates complex credit limit validation logic.
    *   Exposed as a Web API endpoint, allowing external systems (e.g., SAP/Postman) or frontend JS to trigger approval logic securely.

### 3. Frontend Customization (PCF & Fluent UI)
*   **Stock Slider Control:** 
    *   A custom component built with **TypeScript** and **React**.
    *   Replaces the standard integer input with a Fluent UI slider for better UX on inventory management.
    *   Supports full data binding (`notifyOutputChanged`).
*   **Client Scripting:**
    *   Dynamic form logic using `Xrm.Navigation` and `formContext` to handle field visibility and requirement levels based on user input.

### 4. Integration
*   **Webhooks:** Asynchronous integration with external endpoints (simulated via Webhook.site) to push real-time data upon record creation.
*   **Virtual Tables:** Integrated external OData v4 services (`ODataPublic`) to view external product data within D365 without data replication.

---


