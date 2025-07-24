# WYD Server

## Develop - Submodules
### Setup
After downloading the repository, on the local machine, import Core submodule:
```bash
git submodule update --init --recursive
```
### Upload changes
once your code is perfect, you have to push your updates to both the core repository and the current one.

#### 1. Update core repository

```bash
cd server/src/Core
git add .
git commit -m ""
git checkout develop
git push origin develop
```
#### 2. Update parent repository

Move to the parent folder and push the updates

## 🚀 First Installation

This section covers the initial setup of the WYD server on a new Virtual Machine (VM).

### 1. Prepare the VM

First, update your VM's package list and install **Docker** and **Docker Compose**.

Update and install dependencies
```bash
sudo apt update
```
Add Docker's GPG key and repository
```bash
sudo apt install -y ca-certificates curl gnupg lsb-release && \
sudo mkdir -p /etc/apt/keyrings && \
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg && \
echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | \
sudo tee /etc/apt/sources.list.d/docker.list > /dev/null && \
sudo apt update
```
Install Docker Engine and Compose Plugin
```bash
sudo apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
```
Start and enable Docker, add user to docker group
```bash
sudo systemctl start docker && \
sudo systemctl enable docker && \
sudo usermod -aG docker ${USER} 
````

### 2\. Deploy the Code

To get your application code onto the VM, you'll need to transfer it from your local machine.

```bash
scp -i PathToSshKey -r LocalPathToFolder/* root@VM_IP:/home/wyd/
```

**Future Improvement:** Consider using **Git** for version control and easier code deployment.

### 3\. Build and Run the Server

Once the code is on the VM, navigate to the directory and start your `rest_server` using Docker Compose.

```bash
cd /home/wyd
docker compose up -d --build rest_server
```

-----

## 🔄 Updates

Keeping your WYD server up-to-date is straightforward.

### 1\. Monitor for Changes

To automatically detect and apply code changes, use `docker compose watch`. Keep this console open to see updates in real-time.

```bash
cd /home/wyd
docker compose watch
```

### 2\. Update the Code

Transfer your latest code from your local machine to the VM.

```bash
scp -i PathToSshKey -r LocalPathToFolder/* root@<VM_IP>:/home/wyd/
```

***Future Improvement:** migrating to a **Git-based workflow** would allow for more efficient code updates, such as `git pull` commands directly on the VM.*


\
\
\
restart the container




-----

## 🔒 Firewall Configuration on the VM

Here's how to configure `ufw` (Uncomplicated Firewall).

### 1\. Enable the Firewall

Activate `ufw` to start enforcing your security rules.

```bash
sudo ufw enable
```

### 2\. Allow Essential Services

Permit necessary traffic for SSH and your web server.

```bash
sudo ufw allow ssh
sudo ufw allow 8080/tcp
```

### 3\. 📡 Database Access Control (MongoDB - Port 27017)
in compass, 

connect with
```bash
mongodb://<admin username>:<admin password>@localhost:27017/admin?authSource=admin
```
using ssh tunnel as 
```bash
ssh -i PathToSshPrivateKey -L 27017:127.0.0.1:27017 root@<VM_IP>
```