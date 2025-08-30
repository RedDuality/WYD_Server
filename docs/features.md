## 🔍 Inspect the Database with MongoDB Compass

If needed, you can connect to the local MongoDB instance using **MongoDB Compass**.

1. **Open Compass** on your local machine.

2. Create a new connection with the following string:

   ```text
   mongodb://wyd_admin:Test_Password@localhost:27017/admin?authSource=admin
   ```

   🔐 Replace `wyd_admin` and `Test_Password` with the credentials defined in your `docker-compose.yml` / `mongo-init` files.

3. Click **Connect** → you’ll now be able to explore the WYD database collections directly.

---

## ⬆️ Push Changes

### 1. Update the Core Repository

```bash
cd server/Core
git add .
git commit -m "Your descriptive commit message here"
git checkout develop
git push origin develop
```

### 2. Update the Parent Repository

```bash
cd ../..   # Go to project root
git add .
git commit -m "Update Core submodule and other changes"
git push origin main   # or your branch
```

---

## ⬇️ Retrieve Core Changes

If someone else updated **Core**, sync with:

```bash
git submodule update --remote --merge
```
---
