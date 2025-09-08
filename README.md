# StoreSphere – SaaS E-Commerce Platform

**StoreSphere** is a multi-tenant SaaS e-commerce solution designed to support multiple merchants under a single system instance.  
The platform follows a **modular monolith architecture** with clear bounded contexts and extensibility for future microservices.

---

## 1. Introduction

### 1.1 Purpose
This project defines the requirements and architecture of **StoreSphere**, a SaaS e-commerce platform that enables merchants to create and manage independent online stores while sharing the same system instance.

### 1.2 Scope
- **Core Features:** Product catalog, orders, checkout, payments  
- **Supporting Features:** Customer accounts, promotions/discounts  
- **Generic Services:** Identity & access management, merchant onboarding, billing  
- **Deployment:** Modular monolith with bounded contexts, evolvable to microservices  

### 1.3 Stakeholders
- **Merchants:** Manage stores, catalogs, and subscriptions  
- **Customers:** Shop, place orders, manage profiles  
- **Administrators:** Manage tenants, subscriptions, and billing  

---

## 2. System Overview

- **Architecture:** SaaS, multi-tenant (single DB instance, isolated schemas per tenant)  
- **Design Style:** Modular monolith with bounded contexts  
- **Contexts:** Identity & Access, Merchant & Billing, Customer, Catalog, Sales  

---

## 3. Functional Requirements

### 3.1 Use Cases
- **Merchant Onboarding**
  - Register merchant, assign unique `TenantId`
  - Activate subscription plan  

- **Customer Registration & Login**
  - Sign up / Sign in with StoreSphere Identity Provider
  - Manage profiles & addresses  

- **Catalog Management**
  - Add/update/delete products
  - Categorize products
  - Define promotions & discounts  

- **Shopping & Checkout**
  - Browse catalog & add to cart
  - Apply discounts/promotions
  - Checkout & pay securely
  - Receive order confirmation  

- **Billing & Subscription**
  - Generate invoices for merchants
  - Process subscription payments
  - Manage plan upgrades/downgrades  

---

## 4. Non-Functional Requirements

- **Scalability:** Handle thousands of tenants  
- **Security:** Strong authentication & RBAC  
- **Tenant Isolation:** Separate schemas per tenant  
- **Extensibility:** Path to microservices migration  
- **Reliability:** Durable order & payment flows  

---

## 5. Architecture & Design

### 5.1 High-Level Architecture
- Modular monolith with 5 bounded contexts  
- Event-driven communication between contexts  
- Shared Kernel (TenantId, audit base classes)  
- ACL layer for integrations (payments, notifications)  

### 5.2 Event Flow
- Domain events raised inside aggregates (e.g., `OrderPlaced`)  
- Application layer handles persistence & triggers integration events  
- Integration events enable cross-context communication  

---

## 6. Data Model

**Key Entities:**
- **IdentityAccess:** Users, Roles, Tenants  
- **MerchantBilling:** Merchants, Subscriptions, Invoices  
- **Customer:** Customers, Addresses, Loyalty  
- **Catalog:** Products, Categories, Promotions  
- **Sales:** Orders, OrderItems, Payments, Shipments  

> 📌 See ERD diagram for details (attach `docs/erd.png`).  

---

## 7. Class Diagrams (Domain Layer)

- **OrderAggregate:** Order, OrderItem  
- **CustomerAggregate:** Customer, Address  
- **ProductAggregate:** Product, Category  
- **MerchantAggregate:** Merchant, Subscription  
- **IdentityAggregate:** User, Role  

---

## 8. Deployment & SaaS Considerations

- **Tenant Model:** One schema per tenant in shared DB server  
- **Shared Services:** StoreSphere Identity, billing, notifications  
- **API Layer:** Multi-tenant aware (inject `TenantId`)  
- **Future Proofing:** Ready for migration to microservices for scale  

---

## 🚀 Getting Started

1. Clone repo  
   ```bash
   git clone https://github.com/your-org/storesphere.git
