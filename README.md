# 🛠️ WYD Server Guide

This guide provides comprehensive instructions for setting up your WYD Server locally.

---

## 💻 Local Machine Setup

### 🍃 Local MongoDB Setup
First Setup:

#### 1. Install Docker Desktop
Follow the instructions from the official Docker Desktop website. This is the only prerequisite tool needed.

#### 2. Start the database

Open a terminal in the server directory where the docker-compose.yml file is located and run :

```bash
docker compose up
```
The server will be running on localhost:27017, with the credentials set in the docker-compose.yml file.

#### On Following Times (After a computer restart):
Simply open Docker Desktop to ensure the Docker daemon is running, and then navigate to the directory and run docker compose up again. Your database data is persisted in a Docker volume and will not be lost.

---
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
## ▶️ Start the Program

1. **Open VSCode**: Open the root folder of the repo (or `/server`) in VSCode.
2. **Setup environment**: copy appsettings-blueprint.json into a new appsettings.json file, making sure your environment variables are the same of secrets.yaml
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
