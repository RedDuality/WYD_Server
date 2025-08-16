# 🛠️ WYD Server Guide

This guide provides comprehensive instructions for setting up your WYD Server locally and deploying it to a Virtual Machine (VM).

---

## 💻 Local Machine Setup

### 📥 Download the Code

Follow these steps to get the server code onto your local machine:

1. Navigate to your desired directory:

   ```bash
   cd /path/to/your/project/folder
   ```

2. Clone the repository:

   ```bash
   git clone https://github.com/RedDuality/WYD_Server
   ```

3. Initialize and update submodules:

   ```bash
   git submodule update --init --recursive
   ```

4. Switch to the `develop` branch for the Core submodule:

   ```bash
   cd server/Core
   git checkout develop
   ```

---

### 🍃 Local MongoDB Setup

Set up your local MongoDB instance with authentication.

#### 1. Install MongoDB

Ensure MongoDB is installed on your local machine.

- **Linux**: follow the instruction from the official site
- **Windows**: download and install the .msi file from the official site

#### 2. Enable Authentication

Modify your `mongod.conf` file:

- **Linux**: `/etc/mongod.conf`  
  Run: `sudo nano /etc/mongod.conf`
- **macOS**: Usually `/opt/homebrew/etc/mongod.conf` or `/usr/local/etc/mongod.conf`
- **Windows**: Look for `/bin/mongod.cfg` in your MongoDB installation directory, might be necessary to change write permissions.

Add the following lines:

```yaml
security:
  authorization: enabled
```

Then restart MongoDB:

- **Linux**:
  ```bash
  sudo systemctl start mongod
  ```
- **macOS**:
  ```bash
  brew services start mongodb-community
  ```
- **Windows**: Use Services Manager to locate the MongoDB Server service and restart it

#### 3. Connect to MongoDB

```bash
mongosh "mongodb://localhost:27017/"
```

Or use MongoDB Compass: first add a new connection with this link, then open the related shell
.

---

### 👤 Create Users

#### 1. Create `wyd_admin`

Inside the mondoDb shell, run:

```js
use admin

db.createUser({
  user: "wyd_admin",
  pwd: "Test_Password",
  roles: [
    { role: "userAdminAnyDatabase", db: "admin" },
    { role: "readWriteAnyDatabase", db: "admin" },
    { role: "dbAdminAnyDatabase", db: "admin" },
    { role: "clusterAdmin", db: "admin" }
  ]
});
```

Exit the shell:

```bash
exit
```

#### 2. Create `wyd_app_user`

Reconnect as `wyd_admin`:

In compass, disconnect and then edit the connection to use the credential we just created(wyd_admin, Test_Password, "admin" database).

If you are using the terminal, run:
```bash
mongosh "mongodb://localhost:27017/" --username wyd_admin --authenticationDatabase admin
```

Then (in the shell) run:

```js
use admin

db.createUser({
  user: "wyd_app_user",
  pwd: "Test_User_Password",
  roles: [
    { role: "enableSharding", db: "admin" },
    { role: "readWrite", db: "wyd" }
  ]
});
```

Verify the correct creation of the two users:

```js
show users
```

Switch to `wyd` database (creates it if not exists):

```js
use wyd
```
You can now close the shell

---

## ▶️ Start the Program

1. **Open VSCode**: Open the root folder of the repo (or `/server`) in VSCode.
2. **Navigate** to `src/Program.cs`.
3. **Run and Debug**:
   - Install .NET SDK 9.x.
   - Restart VSCode if needed.
   - Ensure you're running the **Development** configuration.

---

## ⬆️ Push Changes

### 1. Update Core Repository

```bash
cd server/Core
git add .
git commit -m "Your descriptive commit message here"
git checkout develop
git push origin develop
```

### 2. Update Parent Repository

```bash
cd ../.. # Go to root
git add .
git commit -m "Update Core submodule and other changes"
git push origin main # or your main branch
```

---

## ⬇️ Retrieve Core Changes

If others updated Core, run:

```bash
git submodule update --remote --merge
```

---

## 🚀 Deployment on the Server

### 1. Prepare the VM

Update packages and install Docker & Compose:

```bash
sudo apt update

sudo apt install -y ca-certificates curl gnupg lsb-release && \
sudo mkdir -p /etc/apt/keyrings && \
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg && \
echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | \
sudo tee /etc/apt/sources.list.d/docker.list > /dev/null && \
sudo apt update

sudo apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

sudo systemctl start docker
sudo systemctl enable docker
sudo usermod -aG docker ${USER}
```

### 2. Deploy the Code

Transfer from local to VM:

```bash
scp -i PathToSshKey -r LocalPathToFolder/* root@VM_IP:/home/wyd/
```

💡 *Consider switching to Git for future deployments.*

### 3. Build and Run the Server

```bash
cd /home/wyd
docker compose up -d --build rest_server
```

---

## 🔒 Firewall Configuration

### 1. Enable UFW

```bash
sudo ufw enable
```

### 2. Allow Essential Services

```bash
sudo ufw allow ssh
sudo ufw allow 8080/tcp
```

### 3. 📡 MongoDB Access via SSH Tunnel

On your local machine:

```bash
ssh -i PathToSshPrivateKey -L 27017:127.0.0.1:27017 root@<VM_IP>
```

Then in Compass:

```text
mongodb://wyd_admin:Test_Password@localhost:27017/admin?authSource=admin
```

🔐 Replace `Test_Password` accordingly.

---

## 🔄 Updates

### 1. Watch for Changes

Run on VM to watch for live updates:

```bash
cd /home/wyd
docker compose watch
```

### 2. Update Code on VM

```bash
scp -i PathToSshKey -r LocalPathToFolder/* root@<VM_IP>:/home/wyd/
```

💡 *Switch to Git for smoother deployment.*

### 3. Restart Server

If `watch` isn’t running:

```bash
cd /home/wyd
docker compose restart rest_server
```

from the server folder:
docker build -t redduality/wyd-rest-server:latest --target final .
docker push redduality/wyd-rest-server:latest
---