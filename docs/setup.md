# 🛠️ WYD Server Guide

This guide provides **step-by-step instructions** to set up and run your **WYD Server locally**.

---

## 💻 Local Machine Setup

### 🍃 MongoDB with Docker

#### 1. Install Docker Desktop

Download and install **Docker Desktop** from the [official website](https://www.docker.com/products/docker-desktop/).
This is the **only prerequisite tool** required.

#### 2. Start the Database

From the `server` directory (where the `docker-compose.yml` file is located), run:

```bash
docker compose up -d
```

* MongoDB will run on **localhost:27017**.
* Credentials are already configured inside `docker-compose.yml`.

#### 3. Restart the Database (After Reboot)

* Open **Docker Desktop** → ensure the Docker daemon is running → restart the container from the UI.
* Or re-run:

  ```bash
  docker compose up -d
  ```

#### 4. Stop the Database

Either stop it from **Docker Desktop**, or run:

```bash
docker compose down
```

### 📥 Download the Code

1. Navigate to your preferred project directory:

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

4. Switch the **Core submodule** to the `develop` branch:

   ```bash
   cd server/Core
   git checkout develop
   ```

---

## ▶️ Run the Program

1. **Open VSCode** → open the project root (or `/server`).
2. **Setup environment** → copy `appsettings-blueprint.json` → `appsettings.json` and configure variables:

   * Must match your **docker-compose** and **mongo-init** credentials.
   * Add any required **external service credentials**.
3. **Navigate** to `src/Program.cs`.
4. **Run and Debug**:

   * Install **.NET SDK 9.x**.
   * Restart VSCode if needed.
   * Run in **Development** configuration.

