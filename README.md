# WYD Server

## Local Machine Setup

### Download the code

1. Move in the folder you want the repository to be
2. Download the repository from this GitHub,
```bash
git clone https://github.com/RedDuality/WYD_Server
```
3. import submodules:
```bash
git submodule update --init --recursive
```
move to the submodule develop branch

```bash
cd server/Core
git checkout develop
```
### Local mongo setup

1. Install mongodb on local machine

2. Enable Authentication:\
Modify mongod.conf to add security.\
File position and name changes based on the OS:

Linux:
```bash
 sudo nano /etc/mongod.conf
``` 
Mac:\
typically is /opt/homebrew/etc/mongod.conf or /usr/local/etc/mongod.conf

Windows:\
Look for mongod.cfg in your MongoDB installation directory

Add:
```yaml
# /etc/mongod.conf (or similar)

# ... other configurations ...

security:
  authorization: enabled

# ... other configurations ...
```

3. Restart mongodb

Linux:
```bash
 sudo systemctl start mongod
```

macOS (if installed via Homebrew):
```bash
brew services start mongodb-community 
```

Windows: \
Start the MongoDB service via Services Manager or run 
```bash
mongod.exe --config "path\to\mongod.cfg" 
```
in an administrative command prompt.


3. Connect to mongo, either by bash or by Compass
```bash
mongosh "mongodb://localhost:27017/"
```

In Compass, you have first to connect to the database using the url, then open the MongoDB shell

4. Create wyd_admin in the admin database

switch to the admin database
```bash
use admin
```
create admin user
```javascript
db.createUser(
  {
    user: "wyd_admin",
    pwd: "Test_Password",
    roles: [
      { role: "userAdminAnyDatabase", db: "admin" },
      { role: "readWriteAnyDatabase", db: "admin" },
      { role: "dbAdminAnyDatabase", db: "admin" },
      { role: "clusterAdmin", db: "admin" }
    ]
  }
);
```
5. create wyd_app_user

Connect mongosh as wyd_admin.

Via bash o compass.

bash:
```bash
mongosh "mongodb://localhost:27017/" --username wyd_admin --authenticationDatabase admin
```
then insert the password("Test_Password")

In Compass edit the connection using the created credentials, then connect and open the shell.


switch to the admin database
```bash
use admin
```

Then, create the program's user:

```javascript
db.createUser(
  {
    user: "wyd_app_user",
    pwd: "Test_User_Password",
    roles: [
      { role: "clusterAdmin", db: "admin" },
      { role: "readWrite", db: "wyd" }
    ]
  }
);
```

Test the creation of the user

```javascript
show users
```

exit from the console or disconnect from Compass.

6. Switch to the wyd database (will create it if it does not exists)
```bash
use wyd
```


## Push changes
### Upload changes
once your code is perfect, you have to push your updates to both the core repository and the current one.

#### 1. Update core repository

```bash
cd server/Core
git add .
git commit -m ""
git checkout develop
git push origin develop
```
#### 2. Update parent repository

Move to the parent folder and push the updates

## 🚀 Deployment on the Server

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
in local compass, 

connect with
```bash
mongodb://<admin username>:<admin password>@localhost:27017/admin?authSource=admin
```
using ssh tunnel as 
```bash
ssh -i PathToSshPrivateKey -L 27017:127.0.0.1:27017 root@<VM_IP>
```
---


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

