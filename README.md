# 🛠️ WYD Server Guide

This guide provides comprehensive instructions for setting up your WYD Server locally.

---

## 💻 Local Machine Setup

### 🍃 Local MongoDB Setup
First Setup:

#### 1. Install Docker Desktop
Follow instructions from its original website.
This will also provide a local Kubernetes cluster for development.

#### 2. Start kubernetes

From the Docker desktop application, install and start Kubernetes:
1. move to Main Page -> Settings-> Kubernetes
2. click on "Enable Kubernetes"
3. click on "Apply"
4. wait for it to download the needed images and start
#### 3. Install kubectl

For Windows and Mac it is already shipped and you only need to locate its path and add it to environment variables.
Usually it's /usr/local/bin/kubectl (Mac) or C:\Program Files\Docker\Docker\resources\bin\kubectl.exe (Windows).

For linux you have to install the kubectl binary and add it to loca/bin:

```bash
curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/amd64/kubectl"
sudo install -o root -g root -m 0755 kubectl /usr/local/bin/kubectl
```

Check the commands by running:

```bash
kubectl version --client
kubectl get nodes
```

#### 3. Create and Deploy the Database

First, you need to set the environment variables.\
Move to the repository's server/kube folder, then create secrets.yaml from secrets-blueprint.yaml:

```bash
cd server/kube
cp secrets-blueprint.yaml secrets.yaml
```
secrets-blueprint.yaml contains the default credentials for local testing development.\
Make sure the secrets you are using are the intended ones.

Then, deploy your database and secrets to the local cluster:

```bash
kubectl apply -f secrets.yaml
kubectl apply -f mongodb-deploy.yaml
```

Wait a few moments for the database Pod to start, and confirm it's running:

```bash
kubectl get pods
```
#### 4. Forward the Connection
Open a new terminal and run the following command to create a secure tunnel:

```bash
kubectl port-forward svc/mongodb-service 27017:27017
```

Keep this terminal open as long as you are developing, as the connection will close when the command is terminated. Your local backend can now connect to localhost:27017.

#### On Following Times (After a computer restart):
Simply start Docker Desktop, and the local Kubernetes cluster will be automatically restarted. The database Pod and all its data will persist. Then, just run the port-forwarding command from step 4 to re-establish the connection.

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
