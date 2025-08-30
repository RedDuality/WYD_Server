# 🚀 Configure the CI/CD Pipeline with GitHub Actions

This guide shows how to configure **automatic deployments** with GitHub Actions and how to access the **MongoDB database with Compass**.

---

## 🔑 1. Set Up SSH Key-Based Authentication for GitHub

1. **Generate a new SSH key pair** on your local machine (or any machine you’ll use for GitHub Actions):

   ```bash
   ssh-keygen -t ed25519 -C "server-identifier-github-actions-key"
   ```

   * Save it as `~/.ssh/server-identifier-github` (or similar).
   * ❌ Do **not** set a passphrase (automation can’t enter one).

2. **Copy the public key** to your remote server:

   * From **Linux/macOS**:

     ```bash
     ssh-copy-id -i ~/.ssh/server-identifier-github-actions.pub <user>@<server_ip>
     ```
   * From **Windows**:

     ```bash
     type '/path/to/server-identifier-github-actions.pub' | ssh -i '/path/to/keyfile' <user>@<server_ip> "cat >> .ssh/authorized_keys"
     ```

---

## 🔒 2. Store Secrets in GitHub

Your GitHub Actions workflow needs secure credentials.
Go to **GitHub → Repository → Settings → Secrets and variables → Actions**.

➡️ Add the following repository secrets:

* **DOCKERHUB\_USERNAME** → Your Docker Hub username
* **DOCKERHUB\_TOKEN** → Docker Hub Personal Access Token (with Read & Write permissions)
* **SSH\_PRIVATE\_KEY** → Content of the private SSH key generated earlier (`~/.ssh/server-identifier-github`)
* **SSH\_HOST** → Your server IP (e.g., `192.168.1.100`)
* **SSH\_USER** → SSH username for your server (e.g., `root`)
* **SUBMODULE\_TOKEN** → GitHub token (repo scope) to fetch submodules

---

## ⚙️ 3. Create the GitHub Actions Workflow File

Inside your project, create the workflow file:

```bash
mkdir -p .github/workflows
nano .github/workflows/deploy.yml
```

This will contain the pipeline definition.

---
