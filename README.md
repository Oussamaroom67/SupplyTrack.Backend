

# **SupplyTrack – Cahier des Charges**

## 🎯 Introduction

**SupplyTrack** est un système de gestion de chaîne d'approvisionnement interne conçu pour **optimiser et tracer tous les flux internes** de matières premières et produits finis dans l'entreprise.

### Objectif

Optimiser et tracer tous les flux internes depuis l'approvisionnement fournisseur jusqu'aux transferts internes et retours, **en assurant qualité et traçabilité**.

---

## 📝 Périmètre Fonctionnel

#### ✅ Inclus

* Approvisionnement fournisseurs
* Contrôle qualité à la réception
* Gestion multi-entrepôts
* Processus de production (création produit fini)
* Transferts internes
* Retours et produits défectueux
* Traçabilité complète des mouvements
* Gestion des utilisateurs et rôles avec authentification et autorisation

#### ❌ Exclu

* Gestion clients externes
* Ventes et distribution externe
* Facturation

---

## 📚 Langage Ubiquitaire (Ubiquitous Language)

| Terme                     | Définition                                                                 |
| ------------------------- | -------------------------------------------------------------------------- |
| **Supplier**              | Fournisseur de matières premières ou composants                            |
| **PurchaseOrder (PO)**    | Commande fournisseur (Aggregate Root)                                      |
| **PurchaseLine**          | Ligne de commande d’un produit spécifique (Entité interne)                 |
| **Product**               | Matière première, composant ou produit fini (AR)                           |
| **Warehouse**             | Entrepôt interne (AR)                                                      |
| **StockLevel**            | Quantité disponible pour un produit dans un entrepôt (VO / Entité interne) |
| **InternalTransfer**      | Transfert de produit entre entrepôts (Entité interne)                      |
| **ProductionOrder**       | Ordre de production pour fabriquer un produit fini (AR)                    |
| **ProductionBatch**       | Lot de production unique et traçable (Entité interne)                      |
| **BillOfMaterials (BOM)** | Liste des composants nécessaires pour un produit (VO)                      |
| **ReturnedProduct**       | Produit retourné ou défectueux (AR)                                        |
| **NonConformity**         | Défaut identifié sur un produit (Entité interne)                           |
| **User**                  | Utilisateur du système avec rôle et permissions (AR)                       |
| **Role**                  | Rôle d’utilisateur définissant ses droits (VO)                             |
| **Permission**            | Permission spécifique liée au rôle (VO)                                    |

---

## 🏗️ Architecture DDD – Bounded Contexts

| Contexte             | Aggregate           | Root Entity     | Entités internes / VO                                        | Règles métier                                                                                                                                   |
| -------------------- | ------------------- | --------------- | ------------------------------------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| **Procurement**      | PurchaseAggregate   | PurchaseOrder   | PurchaseLine, Supplier, OrderStatus (VO)                     | Commande confirmée non modifiable, fournisseur agréé, réception conforme, réception partielle autorisée, produits défectueux mis en quarantaine |
| **Inventory**        | ProductAggregate    | Product         | Component (VO), Stock (VO), SKU (VO)                         | Stock ≥ 0, ajout automatique produit fini, traçabilité mouvements                                                                               |
| **Inventory**        | WarehouseAggregate  | Warehouse       | StoredProduct, InternalTransfer                              | Vérification stock avant transfert, mise à jour stock source/destination, suivi transfert                                                       |
| **Production**       | ProductionAggregate | ProductionOrder | ProductionBatch, BillOfMaterials (VO)                        | Consommation composants automatique, lot unique et traçable, production impossible sans tous composants                                         |
| **Quality**          | ReturnAggregate     | ReturnedProduct | NonConformity, ReturnRequest, Quantity (VO), ReturnType (VO) | Retrait immédiat stock défectueux, création retour fournisseur, suivi historique qualité                                                        |
| **Security / Users** | UserAggregate       | User            | Role, Permission (VO)                                        | Authentification sécurisée, hash mot de passe, contrôle accès selon rôle/permissions                                                            |

---

## 🔄 Processus Métier

### 1️⃣ Approvisionnement

1. Détection automatique du besoin (stock sous seuil)
2. Création commande fournisseur (quantité optimale, validation gestionnaire)
3. Suivi commande et livraison
4. Réception et contrôle qualité
5. Mise à jour stock matières premières
6. Gestion écarts et produits défectueux

**Règles métier :**

* Réception partielle autorisée
* Produits défectueux mis en quarantaine
* Tout écart documenté

---

### 2️⃣ Production

1. Vérification disponibilité composants
2. Réservation automatique des composants
3. Lancement production (lot)
4. Consommation composants et suivi avancement
5. Contrôle qualité produits finis
6. Ajout stock produit fini
7. Gestion rebuts et défauts

**Règles métier :**

* Production impossible sans tous composants
* Lot unique et traçable
* Consommation composants automatique

---

### 3️⃣ Transferts Internes

1. Vérification disponibilité stock source
2. Réservation produits à transférer
3. Préparation et expédition avec bon de transfert
4. Réception dans entrepôt destination
5. Mise à jour stock source et destination

**Règles métier :**

* Impossible de transférer plus que stock disponible
* Transfert en cours non annulable
* Écarts tracés et justifiés

---

### 4️⃣ Retours / Produits Défectueux

1. Identification produits défectueux
2. Retrait immédiat du stock actif
3. Création retour fournisseur si applicable
4. Suivi historique et traçabilité

---

### 5️⃣ Gestion Utilisateurs / Auth

1. Création utilisateur avec rôle et permissions
2. Login avec email + mot de passe
3. JWT généré pour chaque session
4. Vérification des permissions pour chaque action
5. Gestion des rôles et activation / désactivation utilisateurs

**Règles métier :**

* Mot de passe stocké haché (ex: bcrypt)
* Permissions limitées selon rôle
* Admin peut créer/éditer tous utilisateurs et rôles

---

## 👥 Acteurs et Responsabilités

| Acteur                             | Responsabilités                                           | Cas d'usage principaux                                     |
| ---------------------------------- | --------------------------------------------------------- | ---------------------------------------------------------- |
| **Admin**                          | Gestion complète des utilisateurs et rôles                | Créer utilisateurs, attribuer rôles, gérer permissions     |
| **Gestionnaire achats**            | Surveiller stocks, créer commandes, négocier fournisseurs | Recevoir alertes, créer commandes, suivre livraison        |
| **Magasinier / Responsable stock** | Réceptionner, contrôler, inventorier                      | Valider réceptions, mettre à jour stocks                   |
| **Responsable logistique interne** | Planifier transferts entre entrepôts                      | Optimiser flux internes, planifier transferts              |
| **Responsable qualité**            | Contrôler qualité et gérer non-conformités                | Inspecter réceptions, décider retours, maintenir standards |

---

## 📋 Cas d'Usage Principaux

* **CU-01 : Approvisionnement automatique**
  Détection stock faible → Calcul quantité optimale → Sélection fournisseur → Création commande → Suivi livraison

* **CU-02 : Réception avec écarts**
  Contrôle quantité et qualité → Mise à jour stock produits conformes → Gestion produits défectueux / retours

* **CU-03 : Production produit fini**
  Vérification composants → Lancement production → Contrôle qualité produits finis → Mise à jour stock produit fini

* **CU-04 : Transferts internes**
  Analyse niveaux stock par entrepôt → Calcul transferts optimaux → Planification et suivi

* **CU-05 : Authentification et gestion utilisateurs**
  Création utilisateur → Attribution rôle → Login → Vérification permissions

---

## 📊 Indicateurs KPI

* **Approvisionnement :** taux rupture stock, délai moyen livraison, conformité fournisseur
* **Stock :** taux rotation, précision inventaire, coût stockage
* **Production :** rendement production, temps cycle, coût unitaire
* **Qualité :** taux retours, délai traitement retours, coût non-qualité
* **Sécurité :** nombre de sessions actives, tentatives login échouées, utilisateurs actifs/inactifs

---

## 🔄 Flux Métier Global

```
[Fournisseur] 
    ↓ Commande / Livraison
[Réception & Contrôle Qualité] 
    ↓ Stock matières premières
[Transfert vers production]
[Ligne de Production] 
    ↓ Transformation
[Contrôle Qualité Produits Finis]
    ↓ Stock produits finis
[Transferts internes entre entrepôts]
[Retours / Produits défectueux]
[Gestion Utilisateurs / Auth]
```

---

## 🗄️ Structure de la Base de Données (DDD / Clean Architecture)

### Aggregate Roots → DbSet

* **PurchaseOrders** (Procurement)
* **Products** (Inventory)
* **Warehouses** (Inventory)
* **ProductionOrders** (Production)
* **ReturnedProducts** (Quality)
* **Users** (Security)

### Entités internes / VO (pas de DbSet)

* `PurchaseLine`, `Supplier`, `OrderStatus` → inclus dans `PurchaseOrder`
* `Component`, `Stock`, `SKU` → inclus dans `Product`
* `StoredProduct`, `InternalTransfer` → inclus dans `Warehouse`
* `ProductionBatch`, `BillOfMaterials` → inclus dans `ProductionOrder`
* `NonConformity`, `ReturnRequest`, `Quantity`, `ReturnType` → inclus dans `ReturnedProduct`
* `Role`, `Permission` → inclus dans `User`

### Relations principales

* `PurchaseOrders → Suppliers`
* `PurchaseLines → Products` (via PurchaseOrder)
* `StockLevels → Products, Warehouses`
* `InternalTransfers→ Products, Warehouses`
* `ProductionOrders → Products`
* `ProductionComponents → ProductionOrders, Products`
* `ReturnedProducts → Products, Warehouses`
* `NonConformities → ReturnedProducts`
* `Users → Roles → Permissions`




